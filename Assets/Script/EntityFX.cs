using System.Collections;
using System.Collections.Generic;
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
        
        [Header("AlimentFX")]
        public List<Color> igniteColor;
        public List<Color> chillColor;
        public List<Color> lightningColor;
        
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
        
        public void AlimentsFxFor(List<Color> colors, float seconds)
        {
            StartCoroutine(AlimentsFx(colors, seconds));
        }

        private IEnumerator AlimentsFx(List<Color> colors, float seconds)
        {
            var coroutine = StartCoroutine(AlimentColorFX(colors));
            
            yield return new WaitForSeconds(seconds);
            
            StopCoroutine(coroutine);
            
            sr.color = Color.white;
        }

        private IEnumerator AlimentColorFX(List<Color> colors)
        {
            while (true)
            {
                sr.color = sr.color != colors[0] ? colors[0] : colors[1];

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
