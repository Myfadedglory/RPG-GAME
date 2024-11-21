using Script.Utilities;

namespace Script.Entity.Enemy.Skeleton.State
{
    public class SkeletonHitState : SkeletonState
    {

        private IState previousState;

        public SkeletonHitState(Enemy entity, Fsm fsm, string animBoolName, Skeleton enemy) : base(entity, fsm, animBoolName, enemy)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = Enemy.hitDuration;

            Enemy.CloseCounterAttackWindow();
        }

        public override void Update()
        {
            base.Update();

            if (IsAnimationFinished)
                Fsm.SwitchState(Enemy.IdleState);
        }
    }
}
