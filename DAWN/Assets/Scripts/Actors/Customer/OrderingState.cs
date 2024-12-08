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
        Meow();
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

    private void Meow()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                AudioManager.instance.PlaySfx(AudioManager.SFX.OrderMeow0);
                break;
            case 1:
                AudioManager.instance.PlaySfx(AudioManager.SFX.OrderMeow1);
                break;
            case 2:
                AudioManager.instance.PlaySfx(AudioManager.SFX.OrderMeow2);
                break;
            case 3:
                AudioManager.instance.PlaySfx(AudioManager.SFX.OrderMeow3);
                break;
            case 4:
                AudioManager.instance.PlaySfx(AudioManager.SFX.OrderMeow4);
                break;
        }
    }
}
