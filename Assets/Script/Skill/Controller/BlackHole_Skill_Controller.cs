using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    public List<Transform> targets;

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(
                transform.localScale ,
                new Vector2(maxSize , maxSize), 
                growSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            GameObject newHotkey = Instantiate(
                hotKeyPrefab,
                collision.transform.position + new Vector3(0 , 2),
                Quaternion.identity);

            KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];

            keyCodeList.Remove(choosenKey);

            BlackHole_HotKey_Controller newHotKeyScript = newHotkey.GetComponent<BlackHole_HotKey_Controller>();

            newHotKeyScript.SetUpHotKey(choosenKey);
        }
    }
}
