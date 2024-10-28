using Script.Utilities;

namespace Script.Player
{
    public class PlayerAnimationTriggers : AnimationTriggers<Player> 
    {
        private void ThrowSword()
        {
            SkillManger.instance.Sword.CreateSword();
        }
    }
}
