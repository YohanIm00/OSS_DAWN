using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaitingState : CustomerState
{
    private float maxTime;
    public override void Enter(CustomerStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        // ShowEmoji();
        maxTime = customer.menuWaitingTime;
        direction = Vector2.zero;
        Animate();
    }

    public override void Exit() {}

    public override void _Update()
    {
        if (customer.isReceived)
            stateMachine.ChangeState(stateMachine.Enjoy);
        
        customer.menuWaitingTime -= Time.deltaTime;
        customer.timer.fillAmount = customer.menuWaitingTime / maxTime;

        if (customer.menuWaitingTime < 0)
            stateMachine.ChangeState(stateMachine.Anger);
    }
}
