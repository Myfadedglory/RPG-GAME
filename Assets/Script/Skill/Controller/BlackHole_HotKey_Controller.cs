using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    public void SetUpHotKey(KeyCode _myNewHotKey)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myText.text = _myNewHotKey.ToString();

        myHotKey = _myNewHotKey;
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            Debug.Log(myHotKey);
        }
    }
}
