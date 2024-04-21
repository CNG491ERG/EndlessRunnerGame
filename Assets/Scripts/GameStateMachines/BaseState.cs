using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected static int parkourCounter = 0;
    protected static int bossCounter = 0;
    protected SceneLoader sceneLoader;
    protected float stateStartTime;
    public int GetParkourCounter()
    {
        return parkourCounter;
    }
    public int GetBossCounter()
    {
        return bossCounter;
    }
    public abstract void EnterState(GameStateMachineScript stateMachine);
    public abstract void UpdateState(GameStateMachineScript stateMachine);

}
