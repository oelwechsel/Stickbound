using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected DataFor_MeleeAttackState stateData;

    protected AttackDetails attackDetails;

    public MeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, DataFor_MeleeAttackState _stateData) : base(_entity, _stateMachine, _animBoolName, _attackPosition)
    {
       this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGO.transform.position;
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

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);
    
    foreach (Collider2D collider in detectedObjects)
        {
            //AudioManager.Instance.PlaySound("EnemyMeleeAttack");
            collider.transform.SendMessage("Damage", attackDetails);
        }
    
    }
}
