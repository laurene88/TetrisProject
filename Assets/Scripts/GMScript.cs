using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{

    public int currentLevel = 0;
    public LevelDetails currentlevelData;
    public int score = 0;
    public int lineCounter = 0;

    [SerializeField]
    public LevelDetails[] allLevelData = new LevelDetails[10];

    //ideally move this out and check in board? so its not an update only runs when needed.
    public void Update(){
        if (lineCounter >= currentlevelData.goalLines){
            ChangeLevel();
        lineCounter = 0;
        }
    }
    
    private void ChangeLevel(){
        currentLevel++;
        currentlevelData = allLevelData[currentLevel];

    }

}

