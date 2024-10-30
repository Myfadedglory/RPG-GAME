using Script.Utilities;

namespace Script.Player.State
{
    public class PlayerBlackholeState : PlayerState
    {
        private float flyTime = 0.3f;
        private bool skillUsed;

        private float defaultGravity;

        public PlayerBlackholeState(Player player, Fsm fsm, string animBoolName) : base(player, fsm, animBoolName)
        {
        }

        public override void Enter(IState lastState)
        {
            base.Enter(lastState);

            skillUsed = false;

            StateTimer = flyTime;

            defaultGravity = Rb.gravityScale;

            Rb.gravityScale = 0;
        }

        public override void Exit(IState newState)
        {
            base.Exit(newState);

            Rb.gravityScale = defaultGravity;

            Entity.MakeTransparent(false);
        }

        public override void Update()
        {
            base.Update();

            if (SkillManger.instance.BlackHole.BlackholeFinished())
            {
                Fsm.SwitchState(Entity.JumpState);
                return;
            }

            if (StateTimer > 0)
            {
                Entity.SetVelocity(0, 15, false);
            }
            else
            {
                Entity.SetVelocity(0, -0.1f, false);
                
                if (skillUsed || !SkillManger.instance.BlackHole.CanUseSkill()) return;
                
                skillUsed = true;
            } 
        }
    }
}