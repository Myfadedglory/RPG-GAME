using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : SkeletonState
{
    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy entity, FSM _fsm, string _animBoolName, Enemy_Skeleton _enemy) : base(entity, _fsm, _animBoolName, _enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
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
            if (player.position.x > enemy.transform.position.x)
                moveDir = 1;
            else if (player.position.x < enemy.transform.position.x)
                moveDir = -1;

            enemy.SetVelocity(enemy.skeletonMoveSpeed * moveDir * enemy.speedMutipulier, rb.velocity.y);
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
