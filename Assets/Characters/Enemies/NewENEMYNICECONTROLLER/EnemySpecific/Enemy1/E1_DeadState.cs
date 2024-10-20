using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_DeadState : DeadState
{
    private Enemy1 enemy;
    public E1_DeadState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataForDeadState _stateData, Enemy1 _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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

        //stateMachine.ChangeState(enemy.moveState);
        //entity.aliveGO.SetActive(false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
