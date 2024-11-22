using UnityEngine;

public class AnnoyingState : CustomerState
{
    public override void Enter(CustomerStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        customer.init(customer.sit, true);
        ShowEmoji(DataManager.instance.emojis["Anger"]);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void _Update()
    {
        throw new System.NotImplementedException();
    }

    void Move()
    {
        if(customer.sit != null)
            customer.sit.isUsing = false;
        
        if(customer.wayPoints.Count != 0)
        {
            direction = (customer.wayPoints[0].position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, customer.wayPoints[0].position, customer.moveSpeed * Time.deltaTime);
            if ((transform.position - customer.wayPoints[0].position).magnitude < 0.05f)
                customer.wayPoints.RemoveAt(0);
        }

        Animate();
    }
}
