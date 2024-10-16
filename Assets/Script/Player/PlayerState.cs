using System.Threading.Tasks;
using UnityEngine;

public class PlayerState : EntityState<Player>
{
    protected float xInput;
    protected float yInput;

    protected static bool isBusy;

    public PlayerState(Player player, FSM fsm, string animBoolName)
        : base(player, fsm, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState); 
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    public override void Update()
    {
        base.Update();

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public async void BusyFor(float seconds)
    {
        isBusy = true;
        await Task.Delay((int)(seconds * 1000));
        isBusy = false;
    }
}
