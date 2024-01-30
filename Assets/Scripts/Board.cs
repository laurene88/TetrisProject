using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    //define array of tetromino data to customise in editor
    public TetrominoData[] tetrominoes;
    //need to refrence our tile map
    public Tilemap tilemap {get; private set;}
    public Piece activePiece {get; private set;}
    public Vector3Int spawnPosition;

    private void Awake(){
        //get tilemap which is on child of this 'board'
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        //need to initialise all of the data
        for (int i = 0; i < this.tetrominoes.Length; i++){
            this.tetrominoes[i].Initialise();
            Debug.Log("initialise"+i);
        }
    }


    public void Start ()
    {
        SpawnPiece();
    }


    public void SpawnPiece()
    {
        //need random tetromino
        int random = Random.Range(0,tetrominoes.Length);
        Debug.Log(tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        this.activePiece.Initialise(this,this.spawnPosition, data);
        Set(activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i< piece.cells.Length; i++){
            //set tile on tilemap
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //but need to offset based on position of piece.
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
}
