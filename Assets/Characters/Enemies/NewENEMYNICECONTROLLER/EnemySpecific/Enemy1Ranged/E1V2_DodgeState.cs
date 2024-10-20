using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1V2_DodgeState : DodgeState
{
    private Enemy1Ranged enemy;
    public E1V2_DodgeState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_DodgeState stateData, Enemy1Ranged enemy) : base(_entity, _stateMachine, _animBoolName, stateData)
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

        if(isDodgeOver)
        {
            if(isPlayerInMaxAgroRange && performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if (isPlayerInMaxAgroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }
            else if (!isPlayerInMaxAgroRange)
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
