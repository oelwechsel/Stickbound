using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1V2_MeleeAttackState : MeleeAttackState
{
    private Enemy1Ranged enemy;
    public E1V2_MeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, DataFor_MeleeAttackState _stateData, Enemy1Ranged enemy) : base(_entity, _stateMachine, _animBoolName, _attackPosition, _stateData)
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

        if(isAnimationFinished)
        {
            if(isPlayerinMinAgrorange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else if (!isPlayerinMinAgrorange)
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
            AudioManager.Instance.PlaySound("BossMeleeAttack");
        }
    }
}
