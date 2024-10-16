using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttack = 4;
    [SerializeField] private float cloneAttackCoolDown = 0.3f;
    [Space]
    [SerializeField] private GameObject balckholePrefab;
    [SerializeField] private float maxSize = 15;
    [SerializeField] private float maxDuration = 20;
    [SerializeField] private float growSpeed = 1f;
    [SerializeField] private float shrinkSpeed = 3f;

    private Blackhole_Skill_Controller currentBlackhole;

    public override global::System.Boolean CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(balckholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetUpBlackHole(
            player,
            maxSize,
            maxDuration, 
            growSpeed, 
            shrinkSpeed,
            amountOfAttack ,
            cloneAttackCoolDown
            );
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackhole) return false;
        return currentBlackhole.PlayerCanExitState;
    }
}
