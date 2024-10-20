using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_MoveState : MoveState
{
    private Enemy1 enemy;
    public E1_MoveState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_MoveState _stateData, Enemy1 _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        this.enemy = _enemy;
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
        
        if (entity.CheckPlayerBehind())
        {
            entity.Flip();
        }

        if (isPlayerInMinAgrorange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }

        else 
        if (isDetectingWall || !isDetectingLedge)
        {

            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
