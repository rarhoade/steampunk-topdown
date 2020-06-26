using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform playerTransform;
    public static GameHandler Instance {get; private set;}
    public List<Character> PlayerCharacters;

    void Awake(){
        Instance = this;
    }

    public int AddCharacter(Character newPlayer){
        PlayerCharacters.Add(newPlayer);
        return PlayerCharacters.Count - 1;
    }

    public float FetchCharStat(int PlayerIndex, int StatIdx){
        return PlayerCharacters[PlayerIndex].FetchStatOnEnum(StatIdx);
    }
}
