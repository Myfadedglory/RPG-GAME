using Script.Utilities;

namespace Script.Entity.Player.State
{
    public class PlayerDashState : PlayerState
    {
        public PlayerDashState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            Entity.Skill.Dash.CreateCloneOnDashStart();

            StateTimer = Entity.Skill.Dash.daskConfig.dashDuration;
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);

            Entity.Skill.Dash.CreateCloneOnDashArrive();

            Entity.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();

            if(!Entity.IsGroundDetected() && Entity.IsWallDetected())
                Fsm.SwitchState(Entity.WallSlide);

            Entity.SetVelocity(Entity.Skill.Dash.daskConfig.defaultDashSpeed * Entity.DashDir , 0 );

            if (StateTimer < 0) 
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
