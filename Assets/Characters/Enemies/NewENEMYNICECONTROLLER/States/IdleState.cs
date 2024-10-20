using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected DataFor_IdleState stateData;

    protected bool flipAfterIdle;

    protected bool isIdleTimeOver;

    protected bool isPlayerInMinAgrorange;



    protected float idleTime;

    

    public IdleState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_IdleState _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgrorange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        
        entity.SetVelocity(0f);
        isIdleTimeOver = false;
        
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if(flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
