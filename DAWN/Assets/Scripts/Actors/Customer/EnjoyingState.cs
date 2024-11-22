using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnjoyingState : CustomerState
{
    private float maxTime;

    public override void Enter(CustomerStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        GetComponent<BoxCollider2D>().enabled = false;
        customer.enjoyingTime += Random.Range(0, 2);
        maxTime = customer.enjoyingTime;
        ShowEmoji(DataManager.instance.emojis["Enjoy"]);  // Implement this after creating DataManager
        direction = Vector2.zero;
        Animate();
        // GameManager... // Implement this after creating GameManager

    }

    public override void Exit() {}

    public override void _Update()
    {
        customer.enjoyingTime -= Time.deltaTime;
        customer.timer.fillAmount = customer.enjoyingTime / maxTime;
        if (customer.enjoyingTime < 0)
            stateMachine.ChangeState(stateMachine.Leave);
    }
}