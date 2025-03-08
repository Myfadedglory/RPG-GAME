using Script.Utilities;

namespace Script.Entity.Player.State
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
        
        public override void Update()
        {
            base.Update();

            Entity.SetVelocity(XInput * Entity.moveSpeed, Rb.velocity.y);

            if (XInput == 0 || Entity.IsWallDetected())
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
