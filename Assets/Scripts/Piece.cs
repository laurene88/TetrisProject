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


    //wasd here, maybe change to include arrows
    //TODO


    public void Update()
    {
        this.board.Clear(this);
       
        if (Input.GetKeyDown(KeyCode.A)){
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.D)){
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S)){ //soft drop
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            HardDrop();
        }

        this.board.Set(this);
    }

    private void HardDrop(){
        while (Move(Vector2Int.down)){ //returns bool, so while can successfully move down.
            continue;
        } //will stop when it cant go further.
    }

    public bool Move(Vector2Int translation){
        //update position, but need to check it is valid first.
        // in bounds? already a piece there?
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        //give this to game board to then check if valid.
        bool valid = this.board.IsValidPosition(this, newPosition); 

        if (valid) {
            this.position = newPosition;
        }

        //return bool if move succeeded
        return valid;
    }
}
