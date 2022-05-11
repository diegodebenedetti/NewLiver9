using System.Collections;
using System.Collections.Generic;
using TurtleGames.Framework.Runtime.Core;
using UnityEngine;
using UnityEngine.Events;

public class GameStagesManager : Singleton<GameStagesManager>
{

    public GameStage StartStage;

    GameStage currentStage;

    public UnityAction<GameStage> OnChangeStage;

    // Start is called before the first frame update
    void Start()
    {
        SetStage(StartStage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStage(GameStage newStage){

        currentStage = newStage;

        OnChangeStage?.Invoke(newStage);
    }
}
