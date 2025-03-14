using UnityEngine;

namespace Script
{
    public class ParallaxBackground : MonoBehaviour
    {
        private GameObject cam;
        [SerializeField] private float parallaxEffect;

        private float xPosition;
        private float length;
        
        private void Start()
        {
            cam = GameObject.Find("Main Camera");

            xPosition = transform.position.x;

            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
        
        private void Update()
        {
            var distanceMoved = cam.transform.position.x * (1- parallaxEffect);

            var distanceToMove = cam.transform.position.x * parallaxEffect;

            transform.position = new Vector3(xPosition +  distanceToMove, transform.position.y);

            if (distanceMoved > xPosition + length)
                xPosition += length;
            else if (distanceMoved < xPosition - length)
                xPosition -= length;
        }
    }
}
