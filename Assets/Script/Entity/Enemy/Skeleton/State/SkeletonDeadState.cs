using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonDeadState : SkeletonState
    {
        public SkeletonDeadState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);
        }

        public override void Update()
        {
            base.Update();

            Enemy.SetZeroVelocity();
        }
    }
}
