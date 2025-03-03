using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class SkillToolTipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI skillDescription;

        public void ShowToolTip(string description, string name)
        {
            skillName.text = name;
            skillDescription.text = description;
            gameObject.SetActive(true);
        }
        
        public void HideToolTip() => gameObject.SetActive(false);
    }
}