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
    public int rotationIndex {get; private set;}
    public int blockColorInt;


    public float stepDelay = 1f; //slow default for level 1.
    public float lockDelay = 0.5f; //this allows a bit of sliding possible

    private float stepTime;
    private float lockTime;

    //tile maps use Vector3Ints (not 2)        
    public void Initialise(Board board, Vector3Int position, TetrominoData data, int blockColorInt, float pieceSpeed)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.blockColorInt = blockColorInt;
        this.stepDelay = pieceSpeed;
        
        this.stepTime = Time.time + this.stepDelay; //1sec later than current
        this.lockTime = 0f; //will increase, after reaches lock delay itll lock. 
        this.rotationIndex = 0; //make sure to set this eachtime so spawns as default.
        if (this.cells == null){
            this.cells = new Vector3Int[data.cells.Length];
        } //initialising array, then need to put data in. copy from.
        for (int i = 0; i < data.cells.Length; i++){
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }


    public void Update()
    {
        this.board.Clear(this);

        //before other stuff, resets if moves/rotates.
        this.lockTime += Time.deltaTime;

        //rotations
        //think of each rotation as an index (4 options)
        if (Input.GetKeyDown(KeyCode.Tab)){ //CHANGED to only rotate in 1 direction, using tab key.
            Rotate(1);
        //} else if (Input.GetKeyDown(KeyCode.D)){
          //  Rotate(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)){
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)){ //soft drop
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Return)){
            HardDrop();
        }

        if (Time.time >= this.stepTime){
            Step();
        }

        this.board.Set(this);
    }


    private void Step(){
        this.stepTime = Time.time + this.stepDelay;
        //pushes out the step time further.
        Move(Vector2Int.down);
        if (this.lockTime >= this.lockDelay){
            Lock();
        }
    }


    private void Lock(){
        this.board.Set(this);
        this.board.ClearLines(); //when a piece locks, check for lines. in between set & spawn
        this.board.SpawnPiece();
        
    }

    private void HardDrop(){
        while (Move(Vector2Int.down)){ //returns bool, so while can successfully move down.
            continue;
        } //will stop when it cant go further.
        Lock();
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
            this.lockTime = 0f; //reset if valid.
        }

        //return bool if move succeeded
        return valid;
    }

    public void Rotate(int direction){
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);
       
       //test wall kicks, revert rotation if fails. 
       //so wont allow to rotate through the wall
        if (!TestWallKicks(rotationIndex, direction)) {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }


    private void ApplyRotationMatrix(int direction)
    {
    for (int i = 0; i < this.cells.Length; i++){ //use cell data that we copied over, cause now we gonna change it.
            Vector3 cell = this.cells[i];
            //just vector3s as the I & O rotate differently, need offset by .5, so need float.
            int x, y;

            //std rotation matrix. multiply x & y using cos/sin sin/cos etc.
            // thsi is already defined in data class for us. so just go thru & multuply w cells. 
            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    //offset by half a unit, (as rotates around a diff point) then rotate
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    //ceil to int as we offset by half down.
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }
            
            //now we have our new rotated coordinates we just need to set them
            // ? TODO
            this.cells[i] = new Vector3Int(x,y,0);
        }
    }
    //COPY THIS & GO OVER, TODO
    private int Wrap(int input, int min, int max){
        if (input < min){
            return max - (min - input) % (max - min);
        }
        else{
            return min + (input - min) % (max - min);
        }
    }

    //TODO go over this section yikes.
    private bool TestWallKicks(int rotationIndex, int rotationDirection){
        //need to know which index to know which test to run
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++){
            // each test is defining a movement/rotation.
            // run through 1-5 & see if 
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];
            if (Move(translation)){
                return true; // in valid positon good. 
            }
        }
        return false; // was never true, whole translation fails. 
    }


    //find which index/set we need to test (of wallkick data)
    private int GetWallKickIndex(int rotationIndex, int rotationDirection){
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0 ){
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
                        //get length in dimension 0
    }
}
