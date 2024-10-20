using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_ChargeState : ChargeState
{

    private bool ground;
    private Enemy1 enemy;
    public E1_ChargeState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_ChargeState _stateData, Enemy1 _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        
        ground = enemy.CheckGround();
        if (performCloseRangeAction && !enemy.isHopping && ground)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
            //AudioManager.Instance.PlaySound("EnemyMeleeAttack");
        }
        else
        if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else
        if (isChargeTimeOver)
        {
            if (isPlayerInMinAgrorange)
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
