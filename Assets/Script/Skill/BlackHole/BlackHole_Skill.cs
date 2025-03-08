using Unity.VisualScripting;
using UnityEngine;

namespace Script.Skill.BlackHole
{
    public class BlackholeSkill : Skill
    {
        [SerializeField] private BlackholeConfig config;

        private BlackholeSkillController currentBlackhole;

        public override bool CanUseSkill()
        {
            return config.blackhole.GetSkillCondition() && base.CanUseSkill();
        }

        protected override void UseSkill()
        {
            base.UseSkill();

            var newBlackhole = Instantiate(config.prefab, Player.transform.position, Quaternion.identity);

            currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

            currentBlackhole.SetUpBlackHole(
                config
            );
        }

        public bool BlackholeFinished()
        {
            return currentBlackhole && currentBlackhole.PlayerCanExitState;
        }
    }
}
