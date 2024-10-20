using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get ; private set; }    
    public E1_MoveState moveState { get ; private set; }

    public E1_PlayerDetectedState playerDetectedState { get ; private set; }

    public E1_ChargeState chargeState { get ; private set; }

    public E1_LookForPlayerState lookForPlayerState { get ; private set; }

    public E1_MeleeAttackState meleeAttackState { get ; private set; }

    public E1_StunState stunState { get ; private set; }

    public E1_DeadState deadState { get ; private set; }









    [SerializeField]
    private DataFor_IdleState idleStateData;

    [SerializeField] 
    private DataFor_MoveState moveStateData;

    [SerializeField]
    private DataFor_PlayerDetectedState playerDetectedStateData;

    [SerializeField]
    private DataFor_ChargeState chargeStateData;

    [SerializeField]
    private DataFor_LookForPlayerState lookForPlayerStateData;

    [SerializeField]
    private DataFor_MeleeAttackState meleeAttackStateData;

    [SerializeField]
    private DataFor_StunState stunStateData;

    [SerializeField]
    private DataForDeadState deadStateData;

    

    [SerializeField]
    private Transform meleeAttackPosition;
    


    public AttackDetails attackDetails;


   

    public override void Start()
    {
        base.Start();

        

        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E1_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this, stateMachine,"playerDetected",playerDetectedStateData, this);
        chargeState = new E1_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(this, stateMachine,"lookForPlayer",lookForPlayerStateData, this);
        meleeAttackState = new E1_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition,meleeAttackStateData,this);
        stunState = new E1_StunState(this, stateMachine,"stun",stunStateData, this);
        deadState = new E1_DeadState(this, stateMachine,"dead",deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Update()
    {
        base.Update();

        
        isHopping = !CheckGround();

        

    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);


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

    }

    

    public override void Respawn()
    {
        base.Respawn();

        meleeAttackState.FinishAttack();
        
        if (stateMachine.currentState == meleeAttackState)
        {
            stateMachine.ChangeState(moveState);
        }
        else if (stateMachine.currentState == meleeAttackState)
        {

            stateMachine.ChangeState(moveState);
        }
        else
        {
            stateMachine.Initialize(moveState);
        }
        
        
    }

    public override bool CheckPlayerInMinAgroRange()
    {
        //return base.CheckPlayerInMinAgroRange();
        return Physics2D.BoxCast(playerCheck.position, new Vector2(2f, 2f), 0f, aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public override bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.BoxCast(playerCheck.position, new Vector2(2f, 2f), 0f, aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
        //return base.CheckPlayerInMaxAgroRange();
    }

    public override bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.BoxCast(playerCheck.position, new Vector2(2f, 2f), 0f, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
        //return base.CheckPlayerInCloseRangeAction();
    }

    public override bool CheckPlayerBehind()
    {
        return Physics2D.Raycast(playerBehindCheck.position, aliveGO.transform.right * -1, entityData.playerBehindCheckDistance, entityData.whatIsPlayer);
        //return base.CheckPlayerBehind();
    }

    public override void CheckTouchDamage()
    {
        base.CheckTouchDamage();
    }

    public override bool CheckWall()
    {
        //return base.CheckWall();
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatisGround);
    }
}
