using System.Threading.Tasks;
using Script.Utilities;
using UnityEngine;

namespace Script.Entity.Player
{
    public class PlayerState : EntityState<Player>
    {
        protected float XInput;
        protected float YInput;

        protected static bool isBusy;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        protected PlayerState(Script.Entity.Player.Player player, Fsm fsm, string animBoolName)
            : base(player, fsm, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();

            XInput = Input.GetAxisRaw("Horizontal");
            YInput = Input.GetAxisRaw("Vertical");

            Anim.SetFloat(YVelocity, Rb.velocity.y);
        }

        protected static async void BusyFor(float seconds)
        {
            isBusy = true;
            await Task.Delay((int)(seconds * 1000));
            isBusy = false;
        }
    }
}
