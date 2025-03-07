using System.Collections.Generic;
using System.Linq;
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

            if (TryUseMultiCrystal())
                return;

            if (TryCreateCrystal())
                return;

            if (ShouldBlockCrystalMove())
                return;

            SwapPlayerAndCrystalPositions();

            HandleCrystalEffect();
        }

        private bool TryUseMultiCrystal()
        {
            if (!crystalConfig.multipleCrystal.GetSkillCondition() || crystalLeft.Count <= 0)
                return false;

            if (IsFullCrystalStack())
                ScheduleResetAbility();

            ConsumeCrystal();

            return true;
        }

        private bool TryCreateCrystal()
        {
            if (currentCrystal) 
                return false;

            CreateCrystal();
            return true;
        }

        private bool ShouldBlockCrystalMove() => crystalConfig.crystalMove.GetSkillCondition();

        private void SwapPlayerAndCrystalPositions()
        {
            var crystalTransform = currentCrystal.transform;
            var playerTransform = Player.transform;
    
            (crystalTransform.position, playerTransform.position) = 
                (playerTransform.position, crystalTransform.position);
        }

        private void HandleCrystalEffect()
        {
            if (crystalConfig.crystalMirage.GetSkillCondition())
                CreateMirageAndDestroyCrystal();
            else
                TriggerCrystalExit();
        }

        private bool IsFullCrystalStack() => crystalLeft.Count == crystalConfig.crystalStackAmount;

        private void ScheduleResetAbility() => Invoke(nameof(ResetAbility), crystalConfig.useMultipleWindow);

        private void ConsumeCrystal()
        {
            cooldown = 0;
            var crystalToSpawn = crystalLeft.Last();
    
            CreateCrystal(crystalToSpawn);
            crystalLeft.RemoveAt(crystalLeft.Count - 1);

            if (crystalLeft.Count == 0)
                ResetMultiCrystalCooldown();
        }

        private void ResetMultiCrystalCooldown()
        {
            cooldown = crystalConfig.multipleCooldown;
            RefillCrystal();
        }

        private void CreateMirageAndDestroyCrystal()
        {
            SkillManager.instance.Clone.CreateClone(currentCrystal.transform, Vector3.zero);
            Destroy(currentCrystal);
            currentCrystal = null;
        }

        private void TriggerCrystalExit() => currentCrystal.GetComponent<CrystalSkillController>()?.CrystalExitTimeOver();

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
