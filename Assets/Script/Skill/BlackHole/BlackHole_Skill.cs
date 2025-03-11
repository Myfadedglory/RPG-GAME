using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Skill.BlackHole
{
    public class BlackholeSkill : Skill
    {
        [SerializeField] private BlackholeConfig blackholeConfig;

        private BlackholeSkillController currentBlackhole;

        public override bool CanUseSkill()
        {
            return blackholeConfig.blackhole.GetSkillCondition() && base.CanUseSkill();
        }

        protected override void UseSkill()
        {
            base.UseSkill();

            var newBlackhole = Instantiate(blackholeConfig.prefab, Player.transform.position, Quaternion.identity);

            currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

            currentBlackhole.SetUpBlackHole(
                blackholeConfig
            );
        }

        public bool BlackholeFinished()
        {
            return currentBlackhole && currentBlackhole.PlayerCanExitState;
        }
    }
}
