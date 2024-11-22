using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerStateMachine : MonoBehaviour
{
    public CustomerState Enter;
    public CustomerState Order;
    public CustomerState Wait;
    public CustomerState Enjoy;
    public CustomerState Leave;
    public CustomerState Anger;

    public CustomerState currentState;
    
    private void Start()
    {
        Enter = gameObject.AddComponent<EnteringState>();
        Order = gameObject.AddComponent<OrderingState>();
        Wait = gameObject.AddComponent<WaitingState>();
        Enjoy = gameObject.AddComponent<EnjoyingState>();
        Leave = gameObject.AddComponent<LeavingState>();
        Anger = gameObject.AddComponent<AnnoyingState>();

        ChangeState(Enter);
    }

    public void ChangeState(CustomerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }
}
