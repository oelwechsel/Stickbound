using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected DataFor_PlayerDetectedState stateData;

    protected bool isPlayerInMinAgrorange;

    protected bool isPlayerInMaxAgrorange;

    protected bool performLongRangeAction; // z.B Charge attack oder bogenschuss

    protected bool performCloseRangeAction; // z.B. Melee attack

    protected bool isDetectingLedge;

    protected bool isDetectingPlayer;

    public PlayerDetectedState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, DataFor_PlayerDetectedState _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        this.stateData = _stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgrorange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgrorange = entity.CheckPlayerInMaxAgroRange();
        isDetectingLedge = entity.CheckLedge();
        isDetectingPlayer = entity.CheckPlayerInMinAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        performLongRangeAction = false;
        entity.SetVelocity(0f); // HIER VLL ETWAS ANDERES



    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
        
}
