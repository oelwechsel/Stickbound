using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Ranged : Entity
{
    public E1V2_MoveState moveState {  get; private set; }
    public E1V2_IdleState idleState { get; private set; }

    public E1V2_PlayerDetectedState playerDetectedState { get; private set; }

    public E1V2_MeleeAttackState meleeAttackState { get; private set; }

    public E1V2_LookForPlayerState lookForPlayerState { get; private set;}

    public E1V2_StunState stunState { get; private set; }

    public E1V2_DeadState deadState { get; private set; }

    public E1V2_DodgeState dodgeState { get; private set;}

    public E1V2_RangedAttackState rangedAttackState { get; private set;}






    [SerializeField]
    private DataFor_MoveState moveStateData;

    [SerializeField]
    private DataFor_IdleState idleStateData;

    [SerializeField]
    private DataFor_PlayerDetectedState playerDetectedStateData;

    [SerializeField]
    private DataFor_MeleeAttackState meleeAttackStateData;

    [SerializeField]
    private DataFor_LookForPlayerState lookForPlayerStateData;

    [SerializeField]
    private DataFor_StunState stunStateData;

    [SerializeField]
    private DataForDeadState deadStateData;

    [SerializeField]
    public DataFor_DodgeState dodgeStateData;

    [SerializeField]
    private DataFor_RangedAttackState rangedAttackStateData;





    [SerializeField]
    private Transform meleeAttackPosition;

    [SerializeField]
    private Transform rangedAttackPosition;

    public BossHealthbarUI healthBarUI;


    public override void  Start()
    {
        base.Start();


        moveState = new E1V2_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E1V2_IdleState(this,stateMachine, "idle",idleStateData, this);
        playerDetectedState = new E1V2_PlayerDetectedState(this,stateMachine,"playerDetected",playerDetectedStateData, this);
        meleeAttackState = new E1V2_MeleeAttackState(this,stateMachine,"meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E1V2_LookForPlayerState(this,stateMachine,"lookForPlayer", lookForPlayerStateData, this);
        stunState = new E1V2_StunState(this,stateMachine,"stun",stunStateData, this);
        deadState = new E1V2_DeadState(this,stateMachine,"dead",deadStateData, this);
        dodgeState = new E1V2_DodgeState(this,stateMachine,"dodge",dodgeStateData, this);
        rangedAttackState = new E1V2_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

       
        stateMachine.Initialize(moveState);

    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);




        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else
        if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if (!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
        else if (CheckPlayerInMaxAgroRange() && stateMachine.currentState == rangedAttackState)
        {
            stateMachine.ChangeState(rangedAttackState);
        }
        else if (CheckPlayerInMinAgroRange() && stateMachine.currentState == meleeAttackState)
        {
            stateMachine.ChangeState(meleeAttackState);
        }
        

        //else
        //if(currentHealth <= 40f)
        //{
        //    dodgeStateData.dodgeCooldown = 1.5f;
        //    dodgeStateData.dodgeSpeed = 30f;
        //}

        healthBarUI.UpdateHealthBar(currentHealth, entityData.maxHealth);
    }

    public override void Respawn()
    {
        base.Respawn();
        meleeAttackState.FinishAttack();
        rangedAttackState.FinishAttack();
        if (stateMachine.currentState == meleeAttackState)
        {
            stateMachine.ChangeState(moveState);
        }
        else if(stateMachine.currentState == meleeAttackState)
        {

            stateMachine.ChangeState(moveState);
        }
        else
        {
            stateMachine.Initialize(moveState);
        }
        healthBarUI.UpdateHealthBar(currentHealth, entityData.maxHealth);

        //stateMachine.Initialize(moveState);
        //Debug.Log(StartPos.position);

        //dodgeStateData.dodgeCooldown = 3f;
        // dodgeStateData.dodgeSpeed = 25f;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void DamageHop(float velocity, float damageDirection)
    {
        //base.DamageHop(velocity, damageDirection);
        isHopping = true;
        velocityWorkspace.Set(0 , velocity);
        rb.velocity = velocityWorkspace;
    }

    public override void CheckTouchDamage()
    {
        
    }
}
