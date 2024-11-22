using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyingState : CustomerState
{
    public override void Enter()
    {
        base.Enter();
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
        // Make each Customer to follow wayPoints
    }
}
