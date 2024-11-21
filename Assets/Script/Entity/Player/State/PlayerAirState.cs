using Script.Utilities;

namespace Script.Entity.Player.State
{
    public class PlayerAirState : PlayerState
    {
        public PlayerAirState(Script.Entity.Player.Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
        
        public override void Exit(IState newState)
        {
            base.Exit(newState);

            Entity.SetXZeroVelocity();
        }

        public override void Update()
        {
            base.Update();

            if(Entity.IsWallDetected())
                Fsm.SwitchState(Entity.WallSlide);

            if(XInput != 0)
                Entity.SetVelocity(Entity.airMoveMutiplier * XInput * Entity.moveSpeed , Rb.velocity.y , Entity.needFlip);

            if(Entity.IsGroundDetected())
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
