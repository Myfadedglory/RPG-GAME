using System.Collections.Generic;
using Script.Player;
using UnityEngine;

namespace Script.Skill.BlackHole
{
    public class Blackhole_Skill_Controller : MonoBehaviour
    {
        [SerializeField] private GameObject hotKeyPrefab;
        [SerializeField] private List<KeyCode> keyCodeList;

        public bool PlayerCanExitState { get; private set; }

        private float maxSize;
        private float maxDuration;
        private float growSpeed;
        private float shrinkSpeed;
        private bool canGrow;
        private bool canShrink;

        private bool canCreateHotkey;
        private int amountOfAttack;
        private float cloneAttackCoolDown;
        private float cloneAttacktimer;
        private float durationTimer;
        private bool cloneAttackReleased;
        private bool playerCanDisapear;
        private Player.Player player;

        private List<Transform> targets;
        private List<GameObject> createdHotkeys;

        private void Awake()
        {
            keyCodeList = new List<KeyCode>
            {
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.W,
                KeyCode.E
            };
            targets = new List<Transform>();
            createdHotkeys = new List<GameObject>();
            canGrow = true;
            canCreateHotkey = true;
            playerCanDisapear = true;
        }

        public void SetUpBlackHole(
            Player.Player player,
            float maxSize,
            float maxDuration, 
            float growSpeed, 
            float shrinkSpeed, 
            int amountOfAttack, 
            float cloneAttackCoolDown
        )
        {
            this.player = player;
            this.maxSize = maxSize;
            this.maxDuration = maxDuration;
            this.growSpeed = growSpeed;
            this.shrinkSpeed = shrinkSpeed;
            this.amountOfAttack = amountOfAttack;
            this.cloneAttackCoolDown = cloneAttackCoolDown;

            durationTimer = maxDuration;
        }

        private void Update()
        {

            cloneAttacktimer -= Time.deltaTime;
            durationTimer -= Time.deltaTime;

            if (durationTimer < 0)
            {
                durationTimer = Mathf.Infinity;

                if (targets.Count > 0)
                    ReleaseCloneAttack();
                else
                    FinishBlackhole();
            }

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
                {
                    Destroy(gameObject);
                }
            }
        }

        private void ReleaseCloneAttack()
        {
            if(targets.Count < 0) 
                return;

            DestroyHotkeys();

            cloneAttackReleased = true;

            canCreateHotkey = false;

            if (!playerCanDisapear) return;
            
            playerCanDisapear = false;
            
            PlayerManger.instance.player.MakeTransprent(true);
        }

        private void CloneAttackLogic()
        {
            if (cloneAttacktimer >= 0 || !cloneAttackReleased || amountOfAttack <= 0) return;

            cloneAttacktimer = cloneAttackCoolDown;

            float xoffset;

            if (Random.Range(0, 100) > 50)
                xoffset = 2;
            else
                xoffset = -2;

            if(targets.Count > 0)
            {
                SkillManger.instance.Clone.CreateClone(targets[Random.Range(0, targets.Count)], new Vector3(xoffset, 0));
            }

            amountOfAttack--;

            Invoke(nameof(FinishBlackhole), 0.5f);
        }

        private void FinishBlackhole()
        {
            PlayerCanExitState = true;
            canShrink = true;
            cloneAttackReleased = false;
            DestroyHotkeys();
        }

        private void DestroyHotkeys()
        {
            for(int i = 0; i < createdHotkeys.Count; i++)
            {
                Destroy(createdHotkeys[i]);
            }
            createdHotkeys.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.GetComponent<Enemy.Enemy>() != null)
            {
                collision.GetComponent<Enemy.Enemy>().FreezeTime(true);

                CreateHotKey(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Enemy.Enemy>() != null)
                collision.GetComponent<Enemy.Enemy>().FreezeTime(false);
        }

        private void CreateHotKey(Collider2D collision)
        {
            if(keyCodeList.Count <= 0 && !canCreateHotkey) return;

            var newHotkey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

            if (!createdHotkeys.Exists(h => h == newHotkey))
            {
                createdHotkeys.Add(newHotkey);
            }
            else
            {
                Destroy(newHotkey);
                return;
            }

            var choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];

            keyCodeList.Remove(choosenKey);

            var newHotKeyScript = newHotkey.GetComponent<Blackhole_HotKey_Controller>();

            newHotKeyScript.SetUpHotKey(choosenKey, collision.transform, this);
        }

        public void AddEnemyToTarget(Transform enemy)
        {
            if (targets.Exists(e => e == enemy)) return;
            targets.Add(enemy);
        } 

    }
}
