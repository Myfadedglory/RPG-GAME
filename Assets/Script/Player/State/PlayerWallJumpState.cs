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

            Entity.SetVelocity(Entity.horizonJumpForce * - Entity.FacingDir / Entity.wallJumpMutiplier, Entity.verticalJumpForce * Entity.wallJumpMutiplier ,Entity.needFlip);
        }
        
        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
                Fsm.SwitchState(Entity.AirState);
        }
    }
}
