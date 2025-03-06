using System.Collections.Generic;
using Script.Entity.Player;
using UnityEngine;

namespace Script.Skill.BlackHole
{
    public class BlackholeSkillController : MonoBehaviour
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
        private float cloneAttackTimer;
        private float durationTimer;
        private bool cloneAttackReleased;
        private bool playerCanDisappear;
        private Player player;

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
            playerCanDisappear = true;
        }

        public void SetUpBlackHole(
            Player player,
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

            cloneAttackTimer -= Time.deltaTime;
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

            if (!canShrink) return;
            
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

        private void ReleaseCloneAttack()
        {
            if(targets.Count < 0) 
                return;

            DestroyHotkeys();

            cloneAttackReleased = true;

            canCreateHotkey = false;

            if (!playerCanDisappear) return;
            
            playerCanDisappear = false;
            
            PlayerManager.instance.player.MakeTransparent(true);
        }

        private void CloneAttackLogic()
        {
            if (cloneAttackTimer >= 0 || !cloneAttackReleased || amountOfAttack <= 0) return;

            cloneAttackTimer = cloneAttackCoolDown;

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if(targets.Count > 0)
            {
                SkillManager.instance.Clone.CreateClone(targets[Random.Range(0, targets.Count)], new Vector3(xOffset, 0));
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
            foreach (var hotkey in createdHotkeys)
            {
                Destroy(hotkey);
            }

            createdHotkeys.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Entity.Enemy.Enemy>() == null) return;
            
            collision.GetComponent<Entity.Enemy.Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Entity.Enemy.Enemy>() != null)
                collision.GetComponent<Entity.Enemy.Enemy>().FreezeTime(false);
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

            var chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];

            keyCodeList.Remove(chosenKey);

            var newHotKeyScript = newHotkey.GetComponent<BlackholeHotKeyController>();

            newHotKeyScript.SetUpHotKey(chosenKey, collision.transform, this);
        }

        public void AddEnemyToTarget(Transform enemy)
        {
            if (targets.Exists(e => e == enemy)) return;
            targets.Add(enemy);
        } 
    }
}
