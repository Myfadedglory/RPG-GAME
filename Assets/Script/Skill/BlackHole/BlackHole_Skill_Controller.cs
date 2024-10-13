using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;

    private bool canCreateHotkey = true;
    private int amountOfAttack;
    private float cloneAttackCoolDown;
    private float cloneAttacktimer;
    private bool cloneAttackReleased;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    public void SetUpBlackHole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttack, float cloneAttackCoolDown)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttack = amountOfAttack;
        this.cloneAttackCoolDown = cloneAttackCoolDown;
    }

    private void Update()
    {

        cloneAttacktimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(
                transform.localScale,
                new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime
            );
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(
                transform.localScale,
                new Vector2(-1, -1),
                shrinkSpeed * Time.deltaTime
            );

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotkeys();

        cloneAttackReleased = true;

        canCreateHotkey = false;
    }

    private void CloneAttackLogic()
    {
        if (cloneAttacktimer < 0 && cloneAttackReleased)
        {
            cloneAttacktimer = cloneAttackCoolDown;

            float xoffset;

            if (Random.Range(0, 100) > 50)
                xoffset = 2;
            else
                xoffset = -2;


            SkillManger.instance.clone.CreateClone(targets[Random.Range(0, targets.Count)], new Vector3(xoffset, 0));

            amountOfAttack--;

            if (amountOfAttack <= 0)
            {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }
    }

    private void DestroyHotkeys()
    {
        for(int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            Debug.Log("Not Enough Hot Key !");
            return;
        }

        if(!canCreateHotkey) 
            return;

        GameObject newHotkey = Instantiate(
                        hotKeyPrefab,
                        collision.transform.position + new Vector3(0, 2),
                        Quaternion.identity);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];

        keyCodeList.Remove(choosenKey);

        BlackHole_HotKey_Controller newHotKeyScript = newHotkey.GetComponent<BlackHole_HotKey_Controller>();

        newHotKeyScript.SetUpHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToTarget(Transform enemyTransform) => targets.Add(enemyTransform);

}
