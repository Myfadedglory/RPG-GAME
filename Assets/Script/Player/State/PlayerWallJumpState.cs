using Script.Utilities;

namespace Script.Player.State
{
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            StateTimer = .4f;

            Entity.SetVelocity(Entity.jumpForce.x * - Entity.FacingDir / Entity.wallJumpMutiplier, Entity.jumpForce.y * Entity.wallJumpMutiplier ,Entity.needFlip);
        }
        
        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
                Fsm.SwitchState(Entity.AirState);
        }
    }
}
