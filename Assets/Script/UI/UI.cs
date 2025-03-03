using Script.Item.Craft;
using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject statsUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        
        public Tooltip tooltip;
        public CraftTooltip craftTooltip;
        public SkillToolTipUI skillTooltip;

        private void Start()
        {
            SwitchTo(characterUI);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchTo(characterUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchTo(statsUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchTo(skillTreeUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchTo(craftUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SwitchTo(optionsUI);
            }
        }

        public void ShowOrHideUI()
        {
            transform.gameObject.SetActive(!transform.gameObject.activeSelf);
        }

        public void SwitchTo(GameObject menu)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                
            }
            menu?.SetActive(true);
        }
    }
}