using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "playerWaiting")]
public class PlayerWaitingSO : PlayerStateSO
{   
    public override void Execute(PlayerController playerController, PlayerAction playerAction)
    {
        // This will be fulfilled after playerAction.hitCustomer is activated
    }
    public override void Enter() { }
    public override void Exit() { }
}
