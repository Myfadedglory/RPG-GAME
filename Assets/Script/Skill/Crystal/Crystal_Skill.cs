using System.Collections.Generic;
using UnityEngine;

namespace Script.Skill.Crystal
{
    public class CrystalSkill : Skill
    {
        [SerializeField] private CrystalConfig crystalConfig;
        [SerializeField] private List<GameObject> crystalLeft;

        private GameObject currentCrystal;
        private bool chooseRandomTarget;

        public override bool CanUseSkill()
        {
            return crystalConfig.crystal.GetSkillCondition() && base.CanUseSkill();
        }

        public void CreateCrystal()
        {
            currentCrystal = CreateCrystal(crystalConfig.prefab);
        }

        private GameObject CreateCrystal(GameObject prefab)
        {
            var newCrystal = Instantiate(prefab, Player.transform.position, Quaternion.identity);

            newCrystal.GetComponent<CrystalSkillController>().SetUpCrystal(
                crystalConfig,
                chooseRandomTarget,
                ChooseClosestEnemy,
                ChooseRandomEnemy
            );

            return newCrystal;
        }

        public void ChooseRandomTarget()
        {
            chooseRandomTarget = true;
        }

        protected override void UseSkill()
        {
            base.UseSkill();

            // 多重水晶使用逻辑
            if (crystalConfig.multipleCrystal.GetSkillCondition() && crystalLeft.Count > 0)
            {
                MultipleCrystal();
                return;
            }

            // 水晶创建逻辑
            if (!currentCrystal)
            {
                CreateCrystal();
                return;
            }

            // 移动限制检查
            if (crystalConfig.crystalMove.GetSkillCondition())
                return;

            // 位置交换
            (currentCrystal.transform.position, Player.transform.position) = 
                (Player.transform.position, currentCrystal.transform.position);

            CloneInsteadOfCrystal();
        }

        private void CloneInsteadOfCrystal()
        {
            // 水晶效果处理
            if (crystalConfig.crystalMirage.GetSkillCondition())
            {
                SkillManager.instance.Clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
                currentCrystal = null; // 显式清除引用
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkillController>()?.CrystalExitTimeOver();
            }
        }

        private void MultipleCrystal()
        {
            // 处理满栈重置
            if (crystalLeft.Count == crystalConfig.crystalStackAmount)
                Invoke(nameof(ResetAbility), crystalConfig.useMultipleWindow);

            // 消耗水晶
            cooldown = 0;
            var crystalToSpawn = crystalLeft[^1];
            CreateCrystal(crystalToSpawn);
            crystalLeft.RemoveAt(crystalLeft.Count - 1);

            // 重置冷却
            if (crystalLeft.Count == 0)
            {
                cooldown = crystalConfig.multipleCooldown;
                RefillCrystal();
            }
        }

        private void RefillCrystal()
        {
            for(var i=0; i< crystalConfig.crystalStackAmount; i++)
            {
                crystalLeft.Add(crystalConfig.prefab);
            }
        }

        private void ResetAbility()
        {
            if (CooldownTimer >= 0) return;

            CooldownTimer = crystalConfig.multipleCooldown;
        
            RefillCrystal();
        }
    }
}
