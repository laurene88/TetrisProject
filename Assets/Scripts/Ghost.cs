using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;


    public Tilemap tilemap {get; private set;}
    public Vector3Int position {get; private set;}
    //have an array of cells here as well
    public Vector3Int[] cells {get; private set;}

    private void Awake(){
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4]; // HARD CODED
    }

     //clear the piece
    //copy data from piece we are tracking
    //simulate a hard drop
    //set it
    //late update called after all the others (should update after the real piece)
    private void LateUpdate(){
        Clear();
        Copy();
        Drop();
        Set();
    }
   

    private void Clear(){
        for (int i = 0; i< this.cells.Length; i++){
            //set tile on tilemap
            Vector3Int tilePosition = this.cells[i] + this.position;
            //but need to offset based on position of piece.
            this.tilemap.SetTile(tilePosition, null);
        }
    }


    private void Copy(){
        //copy the tracking piece rotation.
        for (int i = 0; i < this.cells.Length; i++){
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    private void Drop(){
        //simulates a hard drop
        Vector3Int position = this.trackingPiece.position;
        //loop through every row in board tile map, bottom to top
        //when we find a valid position we put it there. 
        int currentRow = position.y;
        int bottom = -this.board.boardSize.y/2 -1; //offset by half in negative direction
            
            this.board.Clear(this.trackingPiece);//clear trakcing piece, to check, otherwise will block itself!

            //looping DOWN.
         for (int row = currentRow; row >= bottom; row--){
            position.y = row;

            
            if (this.board.IsValidPosition(this.trackingPiece, position)){
                this.position = position;
            } else {
                //cannot go any further
                break;
            }
        }
        this.board.Set(this.trackingPiece); //add it back after testing valid place
    }


    private void Set(){ //setting ghost piece tile.
        for (int i = 0; i< this.cells.Length; i++){
            //set tile on tilemap
            Vector3Int tilePosition = this.cells[i] + this.position;
            //but need to offset based on position of piece.
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
