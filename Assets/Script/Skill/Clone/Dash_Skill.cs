using UnityEngine;

namespace Script.Skill.Clone
{
    public class DashSkill : Skill
    {
        [SerializeField] private SkillCondition dash;
        [SerializeField] private SkillCondition dashStartMirage;
        [SerializeField] private SkillCondition dashArriveMirage;

        public override bool CanUseSkill()
        {
            return dash.GetSkillCondition() && base.CanUseSkill();
        }

        public void CreateCloneOnDashStart()
        {
            if (dashStartMirage.GetSkillCondition())
            {
                SkillManager.instance.Clone.CreateClone(Player.transform);
            }
        }

        public void CreateCloneOnDashArrive()
        {
            if (dashArriveMirage.GetSkillCondition())
            {
                SkillManager.instance.Clone.CreateClone(Player.transform);
            }
        }
    }
}
