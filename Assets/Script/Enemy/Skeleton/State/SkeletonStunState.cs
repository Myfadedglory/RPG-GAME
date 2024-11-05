using Script.Utilities;
using UnityEngine;

namespace Script.Enemy.Skeleton.State
{
    public class SkeletonStunState : SkeletonState
    {
        public SkeletonStunState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = Enemy.stunDuration;
            
            Enemy.SetVelocity(-Enemy.FacingDir * Enemy.stunDirection.x, Enemy.stunDirection.y, false);
        }

        public override void Update()
        {
            base.Update();

            if( StateTimer < 0 )
                Fsm.SwitchState(Enemy.IdleState);
        }
    }
}
