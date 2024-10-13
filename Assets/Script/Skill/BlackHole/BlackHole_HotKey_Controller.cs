using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemy;
    private BlackHole_Skill_Controller blackHoleAC;

    public void SetUpHotKey(KeyCode myHotKey, Transform enemy, BlackHole_Skill_Controller blackHoleAC)
    {
        sr = GetComponent<SpriteRenderer>();

        myText = GetComponentInChildren<TextMeshProUGUI>();

        myText.text = myHotKey.ToString();

        this.myHotKey = myHotKey;
        this.enemy = enemy;
        this.blackHoleAC = blackHoleAC;
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackHoleAC.AddEnemyToTarget(enemy);

            myText.color = Color.clear;

            sr.color = Color.clear;
        }
    }
}
