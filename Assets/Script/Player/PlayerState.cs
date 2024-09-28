using UnityEngine;

public class PlayerState : EntityState<Player>
{
    protected float xInput;
    protected float yInput;

    public PlayerState(Player player, FSM _fsm, string _animBoolName)
        : base(player, _fsm, _animBoolName)
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

}
