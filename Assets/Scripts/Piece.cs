using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //use this piece as player piece
    //but it changes look each time.

    //also need a reference to the gameboard
        //this controls just the piece relevant info on the board.
    public Board board {get; private set;}
    public TetrominoData data {get; private set;}
    public Vector3Int position {get; private set;}
    //have an array of cells here as well
    public Vector3Int[] cells {get; private set;}

    //tile maps use Vector3Ints (not 2)        
    public void Initialise(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        if (this.cells == null){
            this.cells = new Vector3Int[data.cells.Length];
        } //initialising array, then need to put data in. copy from.
        for (int i = 0; i < data.cells.Length; i++){
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }


}
