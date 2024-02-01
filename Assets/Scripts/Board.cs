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
    public Vector2Int boardSize = new Vector2Int(10,20);

    // use RectInt as it has a useful built in function we can use. 
    public RectInt Bounds // htis is a c# property. need position (bottom left) & size.
        {
            get{ //offsetting by half size of board to move from 0,0, centre to bottom corner.
                Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
                return new RectInt(position, boardSize);
            }
        }

    private void Awake(){
        //get tilemap which is on child of this 'board'
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        //need to initialise all of the data
        for (int i = 0; i < this.tetrominoes.Length; i++){
            this.tetrominoes[i].Initialise();
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

    //a copy of set but with putting null in instead.
        public void Clear(Piece piece)
    {
        for (int i = 0; i< piece.cells.Length; i++){
            //set tile on tilemap
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //but need to offset based on position of piece.
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int checkposition)
    {
        RectInt bounds = this.Bounds;

        //need to test each cell of the piece is valid.
        for (int i = 0; i < piece.cells.Length ;i++){
            Vector3Int tilePosition = piece.cells[i] + checkposition;

            if (!bounds.Contains((Vector2Int)tilePosition)){ //if tile position is not in bounds.
                return false;
            }
            if (this.tilemap.HasTile(tilePosition)){ //already a tile in position.
                return false;
            }
        }
        return true;
    }

    public void ClearLines(){
        //loop through every row in tile map, & check if all cols full.
        // if so, is full & we then clear & shift everything down.
        // iterate from bottom row to top, can use our bounds rect
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        while (row < bounds.yMax)
        {
            //check if row full
            if (IsLineFull(row))
            {
                LineClear(row); //do not iterate the row if we do a line clear.
            } else {
                row++; //ONLY IF CLEAR. as need to retest, as tiles will fall to this row.
            }
        }
    }

    public bool IsLineFull(int row){
        //iterates through cols to check if row full
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin ; col < bounds.xMax; col++){
            Vector3Int position = new Vector3Int(col, row, 0);//tile position we are checking
            if (!this.tilemap.HasTile(position)){
                //is not full
                return false;
            }
        }
        //every tile is set
        return true;
    }

    //TODO WTF IS TILEBASE
    public void LineClear(int row){

        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin ; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null); //delete tile.
        }   

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin ; col < bounds.xMax; col++){
                //get position of tile, but get the one above it.
                Vector3Int position = new Vector3Int(col, row+1, 0); //get position of the tile
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row,0); //back to our current row.
                this.tilemap.SetTile(position, above); //set spot to tile which is above.
                }   
            row++;
        }
    }

}
