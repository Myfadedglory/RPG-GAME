using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Skill.Clone
{
    public class Clone_Skill : Skill
    {
        [Header("Clone info")]
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private float cloneDuration;
        [SerializeField] private bool canAttack;
        [SerializeField] private float clonerDetectDistance = 10f;

        [SerializeField] private bool createCloneOnDashStart;
        [SerializeField] private bool createCloneOnDashOver;

        [Header("Duplicate clone info")]
        [SerializeField] private bool canDuplicateClone;
        [SerializeField] private float chanceToDuplicate;

        [Header("Crystal instead of clone")]
        public bool crystalInsteadOfClone;

        public void CreateClone(Transform newTransform, Vector3 offset = default)
        {
            if(crystalInsteadOfClone)
            {
                SkillManager.instance.Crystal.CreateCrystal();
                SkillManager.instance.Crystal.ChooseRandomTarget();
                return;
            }

            var newClone = Instantiate(clonePrefab);

            newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(
                newTransform,
                cloneDuration,
                clonerDetectDistance, 
                canAttack, 
                ChooseClosestEnemy, 
                canDuplicateClone,
                chanceToDuplicate,
                offset
            );
        }
    }
}
