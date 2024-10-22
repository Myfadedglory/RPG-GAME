using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 5;
    [SerializeField] private Vector2 maxSize = new(3,3);
    [SerializeField] private float growSpeed = 5;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moveable Crystal")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed = 3;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks = 3;
    [SerializeField] private float multiStackCooldown = 5;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new();


    private GameObject currentCrystal = new();

    public override void UseSkill()
    {
        base.UseSkill();

        if(CanUseMultiCrystal())
            return;

        if(currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);  
            
            var currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

            currentCrystalScript.SetUpCrystal(
                crystalDuration, 
                canExplode, 
                canMove, 
                growSpeed,
                moveSpeed,
                maxSize,
                FindClosestEnemy
            );
        }
        else
        {
            (currentCrystal.transform.position, player.transform.position) = 
                (player.transform.position, currentCrystal.transform.position);

            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.CrystalExitTimeOver();
        }
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if(crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }

                cooldown = 0;
                var crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                var newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetUpCrystal(
                    crystalDuration,
                    canExplode, 
                    canMove,
                    growSpeed,
                    moveSpeed, 
                    maxSize, 
                    FindClosestEnemy
                );

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
        if (cooldownTimer > 0) return;
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
