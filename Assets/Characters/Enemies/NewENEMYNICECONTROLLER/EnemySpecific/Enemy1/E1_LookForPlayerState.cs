using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_LookForPlayerState : LookForPlayerState
{
    private Enemy1 enemy;
    public E1_LookForPlayerState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_LookForPlayerState _stateData, Enemy1 _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.enemy = _enemy;
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
        else
        if (isAllTurnsTimeDone)
        {
            //Debug.Log("lol");
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
