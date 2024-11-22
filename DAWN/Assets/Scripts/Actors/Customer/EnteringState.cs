using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteringState : CustomerState
{
    public override void Enter(CustomerStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void Exit() { GetComponent<BoxCollider2D>().enabled = true; }

    public override void _Update()
    {
        Move();
        
        if(customer.wayPoints.Count == 0)
            stateMachine.ChangeState(stateMachine.Order);
    }

    void Move()
    {
        if (customer.wayPoints[customer.wayPoints.Count - 1].GetComponent<Sit>() != null)
            customer.wayPoints[customer.wayPoints.Count - 1].GetComponent<Sit>().isUsing = true;

        if (customer.wayPoints.Count != 0)
        {
            direction = (customer.wayPoints[0].position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, customer.wayPoints[0].position, customer.moveSpeed * Time.deltaTime);
            if ((transform.position - customer.wayPoints[0].position).magnitude < 0.1f)
            {
                transform.position = customer.wayPoints[0].position;
                customer.wayPoints.RemoveAt(0);
            }

            Animate();
        }
    }
}
