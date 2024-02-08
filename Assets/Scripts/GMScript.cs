using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{
    public GameObject mapManager;
    public MapManagerScript mmScript;
    public Board board;
    public int currentLevel = 0;
    public LevelDetails currentlevelData;
    public int score = 0;
    public int lineCounter = 0;

    [SerializeField]
    public LevelDetails[] allLevelData = new LevelDetails[10];


    public void Start(){
        currentLevel = 0;
        currentlevelData = allLevelData[currentLevel];
    }

    //ideally move this out and check in board? so its not an update only runs when needed.
    public void Update(){
        if (lineCounter >= currentlevelData.goalLines){
            lineCounter = 0;
            ChangeLevel();
        }
    }
    
    private void ChangeLevel(){
        currentLevel++;
        currentlevelData = allLevelData[currentLevel];
        mmScript = mapManager.GetComponent<MapManagerScript>();
        mmScript.ResetTileColours();
        //Piece[] allPieces = FindObjectsOfType<Piece>();
        //Debug.Log("i made an array of all the pieces, there are: "+allPieces.Length);
        //PIECES ARE NOT REAL OBJECTS. THEY ARE GROUPS OF TILES ON THE BOARD.
        //foreach (Piece piece in allPieces){
          //  Debug.Log("piece colour number: "+piece.blockColorInt);
            //board.ResetSetTileColor(piece);
        //}
    }

}

