using Script.Utilities;

namespace Script.Player.State
{
    public class PlayerDashState : PlayerState
    {
        public PlayerDashState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            Entity.Skill.Clone.CreateCloneOnDashStart();

            StateTimer = Entity.dashDuration;
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);

            Entity.Skill.Clone.CreateCloneOnDashOver();

            Entity.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();

            if(!Entity.IsGroundDetected() && Entity.IsWallDetected())
                Fsm.SwitchState(Entity.WallSlide);

            Entity.SetVelocity(Entity.dashSpeed * Entity.DashDir , 0 , Entity.needFlip);

            if (StateTimer < 0) 
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
