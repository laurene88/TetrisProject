using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I, 
    O,
    T,
    J,
    L,
    S,
    Z,
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino; //type to associate data for (our enum)
    public Tile tile; //select which tile we want to draw
    public Vector2Int[] cells {get; private set;}; //to set tetromino shape
    // changed from a field to a property by defining get/set so wont show up in editor.

    public void Initialise(){
        this.cells = Data.cells[this.tetromino];
        // looking up cells associated with that tetronimo 
    }
}