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

        public void OnPointerEnter(PointerEventData eventData)
        {
            ui.skillTooltip.ShowToolTip(skillDescription, skillName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.skillTooltip.HideToolTip();
        }
    }
}