using Script.Utilities;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonMoveState : SkeletonGroundedState
    {
        public SkeletonMoveState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Update()
        {
            base.Update();

            Enemy.SetVelocity(Enemy.moveSpeed * Enemy.FacingDir, Rb.velocity.y);

            if (Enemy.IsGroundDetected() && !Enemy.IsWallDetected()) return;
            
            Enemy.Flip();

            Fsm.SwitchState(Enemy.IdleState);
        }
    }
}
