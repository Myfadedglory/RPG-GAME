using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : SkeletonState
{
    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy entity, FSM fsm, string animBoolName, Enemy_Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        enemy.CloseCounterImage();

        player = PlayerManger.instance.player.transform;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance <= enemy.attackDistance)
            {
                enemy.SetXZeroVelocity();

                if (CanAttack())
                    fsm.SwitchState(enemy.attackState);

                return;
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.hatredDistance)
                fsm.SwitchState(enemy.idleState);
        }

        if(fsm.currentState != enemy.attackState)
        {
            if(player.position.x == enemy.transform.position.x)
                return;
            else if (player.position.x > enemy.transform.position.x)
                moveDir = 1;               
            else if (player.position.x < enemy.transform.position.x)
                moveDir = -1;

            enemy.SetVelocity(enemy.moveSpeed * moveDir * enemy.speedMutipulier, rb.velocity.y, enemy.needFlip);
        }
    }

    private bool CanAttack()
    {
        if(Time.time > enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.lastTimeAttacked = Time.time;

            return true;
        }

        return false;
    }
}
