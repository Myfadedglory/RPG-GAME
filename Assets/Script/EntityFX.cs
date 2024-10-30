using System.Collections;
using UnityEngine;

namespace Script
{
    public class EntityFX : MonoBehaviour
    {
        private SpriteRenderer sr;

        [Header("FlashFX")]
        [SerializeField] private Material hitMat;
        [SerializeField] private float flashDuration = 0.2f;
        private Material originalMat;
        
        private void Start()
        {
            sr = GetComponentInChildren<SpriteRenderer>();

            originalMat = sr.material;
        }

        private IEnumerator FlashFX()
        {
            sr.material = hitMat;

            yield return new WaitForSeconds(flashDuration);

            sr.material = originalMat;
        }

        private void RedColorBlink()
        {
            sr.color = sr.color != Color.white ? Color.white : Color.red;
        }

        private void CancelRedColorBlink()
        {
            CancelInvoke();

            sr.color = Color.white;
        }
    }
}
