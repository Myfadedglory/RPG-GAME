using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool unlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;
        [SerializeField] private Image skillImage;
        [SerializeField] private string skillName;
        [SerializeField] private string skillDescription;
        [SerializeField] private Color lockedColor;
        [SerializeField] private Color unLockedColor;

        private UI ui;

        private void OnValidate()
        {
            gameObject.name = $"Skill - {skillName}";
        }
        
        private void Start()
        {
            ui = GetComponentInParent<UI>();
            skillImage = GetComponent<Image>();
            skillImage.color = lockedColor;

            GetComponent<Button>().onClick.AddListener(UnlockSkill);
        }

        private void Update()
        {
            if (CanUnlockSkill() && !unlocked)
            {
                skillImage.color = unLockedColor;
            }
        }

        public void UnlockSkill()
        {
            if (!CanUnlockSkill())
            {
                return;
            }

            unlocked = true;
            skillImage.color = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (CanUnlockSkill())
            {
                ui.skillTooltip.ShowToolTip(skillDescription, skillName);
            }
            else
            {
                ui.skillTooltip.ShowToolTip("you need to unlock preview skill before see the description", "Unknown");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.skillTooltip.HideToolTip();
        }

        private bool CanUnlockSkill()
        {
            if (shouldBeUnlocked.Any(item => !item.unlocked))
            {
                return false;
            }

            if (shouldBeLocked.Any(item => item.unlocked))
            {
                return false;
            }
            
            return true;
        }
    }
}