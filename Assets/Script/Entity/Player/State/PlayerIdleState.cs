using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player.State
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Script.Entity.Player.Player entity, Fsm fsm, string animBoolName) : base(entity, fsm, animBoolName)
        {
        }
        
        public override void Update()
        {
            base.Update();

            if (!(XInput == 0 || (Mathf.Approximately(XInput, Entity.FacingDir)) && Entity.IsWallDetected()))
                Fsm.SwitchState(Entity.MoveState);
        }
    }
}
