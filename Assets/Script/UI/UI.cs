using System;
using UnityEngine;

namespace Script.UI
{
    public class UI : MonoBehaviour
    {
        
        public Tooltip tooltip;

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