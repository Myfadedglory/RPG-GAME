using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.3f;
    private bool skillUsed;

    private float defaultGravity;

    public PlayerBlackholeState(Player player, FSM fsm, string animBoolName) : base(player, fsm, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        skillUsed = false;

        stateTimer = flyTime;

        defaultGravity = rb.gravityScale;

        rb.gravityScale = 0;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        rb.gravityScale = defaultGravity;

        entity.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if (SkillManger.instance.blackHole.BlackholeFinished())
        {
            fsm.SwitchState(entity.JumpState);
            return;
        }

        if (stateTimer > 0)
        {
            entity.SetVelocity(0, 15, false);
        }
        else
        {
            entity.SetVelocity(0, -0.1f, false);
            if (!skillUsed)
            {
                if (!SkillManger.instance.blackHole.CanUseSkill()) return;
                skillUsed = true;
            }
        } 
    }
}