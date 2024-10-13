using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill : Skill
{
    [SerializeField] private int amountOfAttack = 4;
    [SerializeField] private float cloneAttackCoolDown = 0.3f;
    [Space]
    [SerializeField] private GameObject balckHolePrefab;
    [SerializeField] private float maxSize = 15;
    [SerializeField] private float growSpeed = 1f;
    [SerializeField] private float shrinkSpeed = 3f;

    public override global::System.Boolean CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(balckHolePrefab);

        BlackHole_Skill_Controller newBlackHoleScript = newBlackHole.GetComponent<BlackHole_Skill_Controller>();

        newBlackHoleScript.SetUpBlackHole(maxSize, growSpeed, shrinkSpeed,amountOfAttack ,cloneAttackCoolDown);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
