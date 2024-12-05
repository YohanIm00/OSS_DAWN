using System;
using UnityEngine;

[Serializable]
public class PlayerStateMachine
{
    public PlayerStateSO currentState;
    public PlayerServingSO servingState;
    public PlayerWaitingSO waitingState;

    public void Initialize()
    {
        currentState = waitingState;
        Debug.Log("Player's state : " + currentState.name);
        currentState.Enter();
    }

    public void TransitionTo(PlayerStateSO nextState)
    {
        currentState.Exit();
        currentState = nextState;
        Debug.Log("Player's state : " + currentState.name);
        nextState.Enter();
    }

    public void Update(PlayerController playerController, PlayerAction playerAction)
    {
        currentState?.Execute(playerController, playerAction);
    }
}