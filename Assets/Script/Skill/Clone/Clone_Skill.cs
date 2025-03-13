using Script.Config;
using UnityEngine;

namespace Script.Skill.Clone
{
    public class CloneSkill : Skill
    {
        [SerializeField] private CloneConfig cloneConfig;

        public override bool CanUseSkill()
        {
            return cloneConfig.clone.GetSkillCondition() && base.CanUseSkill();
        }

        public void CreateClone(Transform newTransform, Vector3 offset = default)
        {
            if(cloneConfig.cloneCrystal.GetSkillCondition())
            {
                SkillManager.instance.Crystal.CreateCrystal();
                SkillManager.instance.Crystal.ChooseRandomTarget();
                return;
            }

            var newClone = Instantiate(cloneConfig.prefab);

            newClone.GetComponent<CloneSkillController>().SetUpClone(
                newTransform,
                cloneConfig,
                ChooseClosestEnemy, 
                offset
            );
        }
    }
}
