using Script.Utilities;

namespace Script.Entity.Player.State
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Player player, Fsm fsm, string animBoolName) : base(player, fsm, animBoolName)
        {
        }
        public override void Update()
        {
            base.Update();

            Entity.SetZeroVelocity();
        }
    }
}
