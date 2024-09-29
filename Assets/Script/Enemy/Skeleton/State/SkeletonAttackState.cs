using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : SkeletonState
{
    public SkeletonAttackState(Enemy entity, FSM _fsm, string _animBoolName, Enemy_Skeleton _enemy) : base(entity, _fsm, _animBoolName, _enemy)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected().distance > enemy.attackDistance)
            fsm.SwitchState(enemy.battleState);


        if (isAnimationFinished)
          fsm.SwitchState(enemy.battleState);
    }
}
