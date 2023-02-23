using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BackgroundBehaviour : MonoBehaviour
    {
        private Transform followingTarget;
        [SerializeField, Range(0f, 1f)] private float parallaxStrength;
        private Vector3 targetPrevPos;

        private Transform[] layers;
        [SerializeField] private float bgSize;
        private float childOffsetY;
        private int leftIndex;
        private int rightIndex;
        [SerializeField] private float viewOffset = 5;

        private void Awake()
        {
            if (!followingTarget)
            {
                if (Camera.main != null) followingTarget = Camera.main.transform;
                
                layers = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    layers[i] = transform.GetChild(i);
                }

                childOffsetY = layers[1].position.y;

                leftIndex = 0;
                rightIndex = layers.Length - 1;

                targetPrevPos = followingTarget.position;
            }
        }

        private void Update()
        {
            var offset = followingTarget.position - targetPrevPos;
            offset.y = 0;

            targetPrevPos = followingTarget.position;
            transform.position += offset * parallaxStrength;

            if (followingTarget.position.x < (layers[leftIndex].position.x + viewOffset))
            {
                ScrollLeft();
            }
            else if (followingTarget.position.x > (layers[rightIndex].position.x - viewOffset))
            {
                ScrollRight();
            }
        }

        private void ScrollLeft()
        {
            layers[rightIndex].position = new Vector3((layers[leftIndex].position.x - bgSize), childOffsetY, 0);
            leftIndex = rightIndex;
            rightIndex--;
            if (rightIndex < 0)
            {
                rightIndex = layers.Length - 1;
            }
        }
        
        private void ScrollRight()
        {
            layers[leftIndex].position = new Vector3((layers[rightIndex].position.x + bgSize), childOffsetY, 0);
            rightIndex = leftIndex;
            leftIndex++;
            if (leftIndex == layers.Length)
            {
                leftIndex = 0;
            }
        }
    }
}