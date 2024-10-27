using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState<T> : IState where T : Entity
{
    protected float stateTimer { get; set; }
    protected bool isAnimationFinished { get; set; }
    protected string animBoolName {  get; private set; }
    protected FSM fsm { get; private set; }
    protected T entity { get; private set; }
    protected Animator anim { get; private set; }
    protected Rigidbody2D rb {  get; private set; }

    public EntityState(T entity , FSM fsm , string animBoolName)
    {
        this.animBoolName = animBoolName;
        this.entity = entity;
        this.fsm = fsm;
        anim = entity.Anim;
        rb = entity.Rb;
    }

    public virtual void Enter(IState lastState)
    {
        anim.SetBool(animBoolName, true);
        isAnimationFinished = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit(IState newState)
    {
        anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }
}
