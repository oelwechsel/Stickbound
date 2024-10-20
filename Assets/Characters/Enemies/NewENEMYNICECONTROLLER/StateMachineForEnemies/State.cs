using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    public float startTime { get; protected set; }

    protected string animBoolName;

    public State (Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName)
    {
        this.entity = _entity;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        startTime = Time.time; // Everytime enter in subclasses gets called: Stores the startTime
        entity.anim.SetBool(animBoolName, true);

        DoChecks();
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
