using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderingState : CustomerState
{
    private float maxTime;

    public override void Enter(CustomerStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        ShowEmoji(DataManager.instance.emojis["Order"]);
        maxTime = customer.orderWaitingTime;
        direction = Vector2.zero;
        Animate();
    }

    public override void Exit() {}

    public override void _Update()
    {
        if (customer.isOrdered)
            stateMachine.ChangeState(stateMachine.Wait);
        customer.orderWaitingTime -= Time.deltaTime;
        customer.timer.fillAmount = customer.orderWaitingTime / maxTime;
        if (customer.orderWaitingTime < 0)
            stateMachine.ChangeState(stateMachine.Anger);
    }
}
