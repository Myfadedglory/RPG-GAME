using Script.Skill;
using Script.Utilities;

namespace Script.Entity.Player
{
    public class PlayerAnimationTriggers : AnimationTriggers<Player> 
    {
        private void ThrowSword()
        {
            SkillManager.instance.Sword.CreateSword();
        }
    }
}
