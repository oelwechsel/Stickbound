using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;

    protected bool isAnimationFinished;

    protected bool isPlayerinMinAgrorange;

    public AttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition) : base(_entity, _stateMachine, _animBoolName)
    {
        this.attackPosition = _attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerinMinAgrorange = entity.CheckPlayerInMinAgroRange();    
    }

    public override void Enter()
    {
        base.Enter();

        entity.atsm.attackState = this;

        isAnimationFinished = false;

        entity.SetVelocity(0f);
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

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }
}
