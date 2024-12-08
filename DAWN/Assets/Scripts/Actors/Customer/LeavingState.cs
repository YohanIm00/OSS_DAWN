using UnityEngine;

public class LeavingState : CustomerState
{
    public override void Enter(CustomerStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        customer.init(customer.sit, true);
        HideEmoji();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public override void Exit() {}

    public override void _Update()
    {
        Move();
        if (customer.wayPoints.Count == 0)
            Destroy(gameObject);
    }

    void Move() // I guess this part should have to be refactored by evaluating each state thus omitting or merging them.
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
