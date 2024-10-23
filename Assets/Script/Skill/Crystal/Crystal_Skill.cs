using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
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

    public GameObject CreateCrystal(GameObject prefab)
    {
        var newCrystal = Instantiate(prefab, player.transform.position, Quaternion.identity);

        newCrystal.GetComponent<Crystal_Skill_Controller>().SetUpCrystal(
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

    public override void UseSkill()
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

            (currentCrystal.transform.position, player.transform.position) = 
                (player.transform.position, currentCrystal.transform.position);

            if (cloneInsteadOfCrystal)
            {
                SkillManger.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.CrystalExitTimeOver();
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
        if (cooldownTimer >= 0) return;

        cooldownTimer = multiStackCooldown;
        
        RefilCrystal();
    }
}
