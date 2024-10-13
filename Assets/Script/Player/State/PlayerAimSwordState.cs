using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, FSM fsm, string animBoolName) : base(player, fsm, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        entity.skill.sword.ActiveDots(true);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        entity.SetXZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1)) 
            fsm.SwitchState(entity.idleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < entity.transform.position.x && entity.facingDir == 1)
            entity.Flip();
        else if(mousePosition.x > entity.transform.position.x && entity.facingDir != 1)
            entity.Flip();
    }
}
