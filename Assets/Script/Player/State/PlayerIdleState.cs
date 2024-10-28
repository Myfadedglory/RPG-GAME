using Script.Utilities;

namespace Script.Player.State
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
        
        public override void Update()
        {
            base.Update();

            if (!(XInput == 0 || (XInput == Entity.FacingDir) && Entity.IsWallDetected()))
                Fsm.SwitchState(Entity.MoveState);
        }
    }
}
