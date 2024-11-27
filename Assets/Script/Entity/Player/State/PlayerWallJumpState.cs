using Script.Utilities;

namespace Script.Entity.Player.State
{
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(Script.Entity.Player.Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = .4f;

            Entity.SetVelocity(Entity.jumpForce.x * - Entity.FacingDir / Entity.wallJumpMultiplier, Entity.jumpForce.y * Entity.wallJumpMultiplier ,Entity.needFlip);
        }
        
        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
                Fsm.SwitchState(Entity.AirState);
        }
    }
}
