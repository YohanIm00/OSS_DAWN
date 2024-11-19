using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerStateSO : ScriptableObject
{
    public abstract void Execute(PlayerController playerController, PlayerAction playerAction);
    public abstract void Enter();
    public abstract void Exit();
}
