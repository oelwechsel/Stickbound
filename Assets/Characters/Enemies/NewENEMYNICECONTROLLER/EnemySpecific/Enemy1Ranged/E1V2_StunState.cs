using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1V2_StunState : StunState
{
    private Enemy1Ranged enemy;
    public E1V2_StunState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_StunState _stateData, Enemy1Ranged enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStunTimeOver)
        {
            if(isPlayerInMinAgrorange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
