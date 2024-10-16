using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemy;
    private Blackhole_Skill_Controller blackholeAC;

    public void SetUpHotKey(KeyCode myHotKey, Transform enemy, Blackhole_Skill_Controller blackholeAC)
    {
        sr = GetComponent<SpriteRenderer>();

        myText = GetComponentInChildren<TextMeshProUGUI>();

        myText.text = myHotKey.ToString();

        this.myHotKey = myHotKey;
        this.enemy = enemy;
        this.blackholeAC = blackholeAC;
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackholeAC.AddEnemyToTarget(enemy);

            myText.color = Color.clear;

            sr.color = Color.clear;
        }
    }
}
