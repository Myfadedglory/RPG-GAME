using Script.Utilities;

namespace Script.Player.State
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
        
        public override void Update()
        {
            base.Update();

            Entity.SetVelocity(XInput * Entity.moveSpeed, Rb.velocity.y ,Entity.needFlip);

            if (XInput == 0 || Entity.IsWallDetected())
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
