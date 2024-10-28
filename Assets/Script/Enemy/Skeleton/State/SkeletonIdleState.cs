using Script.Utilities;

namespace Script.Enemy.Skeleton.State
{
    public class SkeletonIdleState : SkeletonGroundedState
    {
        public SkeletonIdleState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = Enemy.idleTime;
        }

        public override void Update()
        {
            base.Update();

            if(StateTimer < 0) 
                Fsm.SwitchState(Enemy.MoveState);
        }
    }
}
