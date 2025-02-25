using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class SkillTreeSlotUI : MonoBehaviour
    {
        private bool unlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;
        [SerializeField] private Image skillImage;
        [SerializeField] private string skillName;
        [SerializeField] private string skillDescription;
        [SerializeField] private Color lockedColor;

        private void OnValidate()
        {
            gameObject.name = $"Skill - {skillName}";
        }
        
        private void Start()
        {
            skillImage = GetComponent<Image>();
            skillImage.color = lockedColor;

            GetComponent<Button>().onClick.AddListener(UnlockSkill);
        }
        
        public void UnlockSkill()
        {
            if (shouldBeUnlocked.Any(item => !item.unlocked))
            {
                return;
            }

            if (shouldBeLocked.Any(item => item.unlocked))
            {
                return;
            }

            unlocked = true;
            skillImage.color = Color.white;
        }
    }
}