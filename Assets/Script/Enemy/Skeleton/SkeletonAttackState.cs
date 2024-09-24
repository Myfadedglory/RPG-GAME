using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetXZeroVerlocity();
    }

    public override void Exit()
    {
        base.Exit();

       enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected().distance > enemy.attackDistance)
            stateMachine.ChangeState(enemy.battleState);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
