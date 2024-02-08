using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (fileName = "LevelDetailsSO")]
public class LevelDetails : ScriptableObject
{
    public int levelNumber;
    public int goalLines;
    //Sets Piece - stepDelay
    // the smaller this number the faster they fall.
    public float levelStepDelay;

    public Tile[] levelTiles = new Tile[3];

    //tile sets to use. 
    //end of level - line count?
}
