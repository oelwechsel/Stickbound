using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1V2_RangedAttackState : RangedAttackState
{
    private Enemy1Ranged enemy;
    public E1V2_RangedAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, DataFor_RangedAttackState stateData, Enemy1Ranged enemy) : base(_entity, _stateMachine, _animBoolName, _attackPosition, stateData)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (entity.gameManager.respawn)
        {
            FinishAttack();
            stateMachine.ChangeState(enemy.moveState);
        }
        
        if(isAnimationFinished)
        {
            if(isPlayerinMinAgrorange)
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        if (!entity.gameManager.respawn)
        {
            AudioManager.Instance.PlaySound("EnemyRangedAttack");
        }
        
    }
}
