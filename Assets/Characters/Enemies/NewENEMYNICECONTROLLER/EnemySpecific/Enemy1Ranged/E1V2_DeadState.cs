using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1V2_DeadState : DeadState
{
    private Enemy1Ranged enemy;
    public E1V2_DeadState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataForDeadState _stateData, Enemy1Ranged enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        AudioManager.Instance.PlaySound("CheckPointAberEigDash");
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
