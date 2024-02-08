using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "tiledataSO")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles = new TileBase[10];
    public int tileColorNumber;
    //public int levelAssocWithTile;
}
