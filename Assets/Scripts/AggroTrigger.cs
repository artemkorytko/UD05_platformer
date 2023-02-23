using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AggroTrigger : MonoBehaviour
    {
        public event Action<GameObject> FindTarget;
        public event Action LostTarget;

        private void OnTriggerEnter2D(Collider2D target)
        {
            if (target.GetComponent<PlayerController>())
            {
                FindTarget?.Invoke(target.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D target)
        {
            if (target.GetComponent<PlayerController>())
            {
                LostTarget?.Invoke();
            }
        }
    }
}