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

            Enemy.Fx.InvokeRepeating("RedColorBlink", 0, .1f);

            StateTimer = Enemy.stunDuration;

            Rb.velocity = new Vector2(-Enemy.FacingDir * Enemy.stunDirection.x, Enemy.stunDirection.y);
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);

            Enemy.Fx.Invoke("CancelRedColorBlink", 0);
        }

        public override void Update()
        {
            base.Update();

            if( StateTimer < 0 )
                Fsm.SwitchState(Enemy.IdleState);
        }
    }
}
