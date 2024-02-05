using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDetails : ScriptableObject
{
    int levelNumber = 0;

    //Sets Piece - stepDelay
    // the smaller this number the faster they fall.
    float levelStepDelay = 1f;

    Tilemap levelTileMap;

    //tile sets to use. 
    //end of level - line count?
}
