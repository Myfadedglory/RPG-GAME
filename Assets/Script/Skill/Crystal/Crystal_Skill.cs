using System.Collections.Generic;
using UnityEngine;

namespace Script.Skill.Crystal
{
    public class CrystalSkill : Skill
    {
        [SerializeField] private GameObject crystalPrefab;
        [SerializeField] private float crystalDuration = 5;
        [SerializeField] private Vector2 maxSize;
        [SerializeField] private float growSpeed = 5;

        [Header("Crystal Mirage")]
        [SerializeField] private bool cloneInsteadOfCrystal;

        [Header("Explosive Crystal")]
        [SerializeField] private bool canExplode;

        [Header("Moveable Crystal")]
        [SerializeField] private bool canMove;
        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private float crystalDetectDistance = 25;

        [Header("Multi Stacking Crystal")]
        [SerializeField] private bool canUseMultiStacks;
        [SerializeField] private int amountOfStacks = 3;
        [SerializeField] private float multiStackCooldown = 5;
        [SerializeField] private float useTimeWindow;
        [SerializeField] private List<GameObject> crystalLeft;

        private GameObject currentCrystal;
        private bool chooseRandomtarget;

        public void CreateCrystal()
        {
            currentCrystal = CreateCrystal(crystalPrefab);
        }

        private GameObject CreateCrystal(GameObject prefab)
        {
            var newCrystal = Instantiate(prefab, Player.transform.position, Quaternion.identity);

            newCrystal.GetComponent<CrystalSkillController>().SetUpCrystal(
                canMove,
                canExplode,
                growSpeed,
                moveSpeed,
                maxSize,
                chooseRandomtarget,
                crystalDuration,
                crystalDetectDistance,
                ChooseClosestEnemy,
                ChooseRandomEnemy
            );

            return newCrystal;
        }

        public void ChooseRandomTarget()
        {
            chooseRandomtarget = true;
        }

        protected override void UseSkill()
        {
            base.UseSkill();

            if(CanUseMultiCrystal())
                return;

            if(currentCrystal == null)
            {
                CreateCrystal();
            }
            else
            {
                if(canMove)
                    return;

                (currentCrystal.transform.position, Player.transform.position) = 
                    (Player.transform.position, currentCrystal.transform.position);

                if (cloneInsteadOfCrystal)
                {
                    SkillManager.instance.Clone.CreateClone(currentCrystal.transform, Vector3.zero);
                    Destroy(currentCrystal);
                }
                else
                {
                    currentCrystal.GetComponent<CrystalSkillController>()?.CrystalExitTimeOver();
                }
            }
        }

        private bool CanUseMultiCrystal()
        {
            if (canUseMultiStacks)
            {
                if(crystalLeft.Count > 0)
                {
                    if (crystalLeft.Count == amountOfStacks)
                        Invoke(nameof(ResetAbility), useTimeWindow);

                    cooldown = 0;

                    var crystalToSpawn = crystalLeft[^1];   //get the lasest one

                    CreateCrystal(crystalToSpawn);

                    crystalLeft.Remove(crystalToSpawn);

                    if(crystalLeft.Count <= 0)
                    {
                        cooldown = multiStackCooldown;

                        RefilCrystal();
                    }
                }
                return true;
            }
            return false;
        }

        private void RefilCrystal()
        {
            for(int i=0; i< amountOfStacks; i++)
            {
                crystalLeft.Add(crystalPrefab);
            }
        }

        private void ResetAbility()
        {
            if (CooldownTimer >= 0) return;

            CooldownTimer = multiStackCooldown;
        
            RefilCrystal();
        }
    }
}
