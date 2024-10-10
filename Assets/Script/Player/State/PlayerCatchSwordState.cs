using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player player, FSM _fsm, string _animBoolName) : base(player, _fsm, _animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        sword = entity.sword.transform;

        if (sword.position.x < entity.transform.position.x && entity.facingDir == 1)
            entity.Flip();
        else if (sword.position.x > entity.transform.position.x && entity.facingDir == -1)
            entity.Flip();

        entity.SetVelocity(entity.swordReturnForce * -entity.facingDir ,rb.velocity.y  , !entity.needFlip);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if(isAnimationFinished)
            fsm.SwitchState(entity.idleState);
    }
}
