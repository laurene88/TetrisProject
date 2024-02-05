using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (fileName = "LevelDetailsSO")]
public class LevelDetails : ScriptableObject
{
    public int levelNumber;

    //Sets Piece - stepDelay
    // the smaller this number the faster they fall.
    public float levelStepDelay = 1f;

    public Tile[] levelTiles = new Tile[3];

    //tile sets to use. 
    //end of level - line count?
}
