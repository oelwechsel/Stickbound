using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1V2_LookForPlayerState : LookForPlayerState
{
    private Enemy1Ranged enemy;
    public E1V2_LookForPlayerState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_LookForPlayerState _stateData, Enemy1Ranged enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        if (isPlayerinMinAgrorange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
