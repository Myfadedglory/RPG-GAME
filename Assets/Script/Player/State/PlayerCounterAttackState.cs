using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player entity, FSM _fsm, string _animBoolName) : base(entity, _fsm, _animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        stateTimer = entity.counterAttackDuration;

        entity.anim.SetBool("CounterSuccess", false);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        entity.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(entity.attackCheck.position, entity.attackCheckDistance);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().CanBeStun())
                {
                    stateTimer = 10;    //无意义，只是一个比较大的值
                    entity.anim.SetBool("CounterSuccess", true);
                }
        }

        if (stateTimer < 0 || isAnimationFinished)
            fsm.SwitchState(entity.idleState);

        if (!isAnimationFinished && Input.GetKeyDown(KeyCode.Q))
            return;
    }
}
