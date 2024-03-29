using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManagerScript : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;
    public GameObject gm;
    public GMScript gmScript;
    public Board board;


    [SerializeField]
    private List<TileData> tileDatas;

    [SerializeField]
    private Dictionary<TileBase, TileData> dataFromTiles;

    // https://www.youtube.com/watch?v=XIqtZnqutGg&ab_channel=ShackMan
    private void Awake(){

        gmScript = gm.GetComponent<GMScript>();
        //making our tile type/tile data dictionary.
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas) //each of the three diff tile datas (1,2,3)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

    //print out all dictionary in Debug.Log.
    // https://www.reddit.com/r/Unity2D/comments/xjtcbg/how_do_i_use_debuglog_to_view_a_dictionary/
        //foreach(var key in dataFromTiles.Keys)
        //{
          //  Debug.Log($"Key: {key}, Value: {dataFromTiles[key]}");
        //}
      
      //Debug.Log(dataFromTiles.Count);
    }

    public void ResetTileColours(){
       // TileBase[] allTilesInPlay = FindObjectsOfType<TileBase>();
           //     TileBase[] allTilesInPlay = map.GetTilesBlock(bounds);
        BoundsInt bounds = map.cellBounds;
        foreach (var position in map.cellBounds.allPositionsWithin) {
            if (map.HasTile(position)) {
                Tile t = (Tile)map.GetTile(position);
                int tileColorNumber = dataFromTiles[t].tileColorNumber;
                ChangeTileColor(position,tileColorNumber);
            }
        }
    }


        public void ChangeTileColor(Vector3Int position, int tileColorNumber){
            map.SetTile(position, gmScript.currentlevelData.levelTiles[tileColorNumber-1]);
        }

    }
