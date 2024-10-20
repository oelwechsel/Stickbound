using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected DataFor_RangedAttackState stateData;

    protected GameObject projectile;
    protected Projectile projectileSkript;


    public RangedAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, DataFor_RangedAttackState stateData) : base(_entity, _stateMachine, _animBoolName, _attackPosition)
    {
        this.stateData = stateData;
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        projectile = GameObject.Instantiate(stateData.projectile,attackPosition.position,attackPosition.rotation);
        projectileSkript = projectile.GetComponent<Projectile>();
        projectileSkript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);
    }
}
