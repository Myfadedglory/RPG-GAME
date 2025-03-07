using UnityEngine;

namespace Script.Skill.Clone
{
    public class CloneSkill : Skill
    {
        [SerializeField] private CloneConfig config;

        public override bool CanUseSkill()
        {
            return config.clone.GetSkillCondition() && base.CanUseSkill();
        }

        public void CreateClone(Transform newTransform, Vector3 offset = default)
        {
            if(config.cloneCrystal.GetSkillCondition())
            {
                SkillManager.instance.Crystal.CreateCrystal();
                SkillManager.instance.Crystal.ChooseRandomTarget();
                return;
            }

            var newClone = Instantiate(config.prefab);

            newClone.GetComponent<CloneSkillController>().SetUpClone(
                newTransform,
                config,
                ChooseClosestEnemy, 
                offset
            );
        }
    }
}
