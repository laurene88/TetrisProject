using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GMScript : MonoBehaviour
{
    public GameObject mapManager;
    public MapManagerScript mmScript;
    public Board board;
    public int currentLevel = 1;
    public LevelDetails currentlevelData;
    public int levellineCounter = 0;
    public int gameLineCounter = 0;
    public int score = 0;
    public TMP_Text scoreLabel;
    public TMP_Text lineNumberText;




    [SerializeField]
    public LevelDetails[] allLevelData = new LevelDetails[10];


    public void Start(){
        currentLevel = 1;
        currentlevelData = allLevelData[currentLevel-1];
    }

    //ideally move this out and check in board? so its not an update only runs when needed.
  //  public void Update(){
    //    if (levellineCounter >= currentlevelData.goalLines){
      //      levellineCounter = 0;
          //  ChangeLevel();
        //}
    //}
    
    public void ChangeLevel(){
        currentLevel++;
        currentlevelData = allLevelData[currentLevel];
        mmScript = mapManager.GetComponent<MapManagerScript>();
        mmScript.ResetTileColours();
        levellineCounter = 0;
        board.pieceSpeed = currentlevelData.levelStepDelay;
        //Piece[] allPieces = FindObjectsOfType<Piece>();
        //Debug.Log("i made an array of all the pieces, there are: "+allPieces.Length);
        //PIECES ARE NOT REAL OBJECTS. THEY ARE GROUPS OF TILES ON THE BOARD.
        //foreach (Piece piece in allPieces){
          //  Debug.Log("piece colour number: "+piece.blockColorInt);
            //board.ResetSetTileColor(piece);
        //}
    }

  // Updates visible scores, score and line counter.
  public void updateScore(int i){
    score = score + (i * currentLevel);
    scoreLabel.text = score.ToString();
    lineNumberText.text = gameLineCounter.ToString();
  }

}

