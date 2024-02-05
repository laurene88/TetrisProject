using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{

    public int currentLevel = 1;
    public LevelDetails currentlevelData;

    [SerializeField]
    public ScriptableObject[] allLevelData = new ScriptableObject[10];

}

