using TMPro;
using UnityEngine;

namespace Script.Skill.BlackHole
{
    public class Blackhole_HotKey_Controller : MonoBehaviour
    {
        private SpriteRenderer sr;
        private KeyCode myHotKey;
        private TextMeshProUGUI myText;

        private Transform enemy;
        private Blackhole_Skill_Controller blackholeAc;

        public void SetUpHotKey(KeyCode myHotKey, Transform enemy, Blackhole_Skill_Controller blackholeAc)
        {
            sr = GetComponent<SpriteRenderer>();

            myText = GetComponentInChildren<TextMeshProUGUI>();

            myText.text = myHotKey.ToString();

            this.myHotKey = myHotKey;
            this.enemy = enemy;
            this.blackholeAc = blackholeAc;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(myHotKey)) return;
            
            blackholeAc.AddEnemyToTarget(enemy);

            myText.color = Color.clear;

            sr.color = Color.clear;
        }
    }
}
