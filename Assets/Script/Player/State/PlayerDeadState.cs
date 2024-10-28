using Script.Utilities;

namespace Script.Player.State
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Player player, Fsm fsm, string animBoolName) : base(player, fsm, animBoolName)
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

            Entity.SetZeroVelocity();
        }
    }
}
