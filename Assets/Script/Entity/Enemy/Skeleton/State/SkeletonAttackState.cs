using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonAttackState : SkeletonState
    {
        public SkeletonAttackState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }
        
        public override void Exit(IState newState)
        {
            base.Exit(newState);

            Enemy.lastTimeAttacked = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (Enemy.IsPlayerDetected().distance > Enemy.attackDistance)
                Fsm.SwitchState(Enemy.BattleState);

            if (IsAnimationFinished)
                Fsm.SwitchState(Enemy.BattleState);
        }
    }
}
