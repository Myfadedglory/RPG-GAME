using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerWallSlideState : PlayerState
    {
        public PlayerWallSlideState(Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fsm.SwitchState(Entity.WallJump);

                return;
            }

            if (XInput != 0 && !Mathf.Approximately(Entity.FacingDir, XInput))
                Fsm.SwitchState(Entity.IdleState);

            if (YInput < 0)
                Entity.SetVelocity(0, Rb.velocity.y);
            else
                Entity.SetVelocity(0, Entity.wallSlideMultiplier * Rb.velocity.y);

            if (Entity.IsGroundDetected())
                Fsm.SwitchState(Entity.IdleState);
        }
    }
}
