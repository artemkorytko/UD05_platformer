using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AttackZoneTrigger : MonoBehaviour
    {
        public event Action PlayerEnterRange;
        public event Action PlayerCancelRange;

        private void OnTriggerEnter2D(Collider2D player)
        {
            if (player.GetComponent<PlayerController>())
            {
                PlayerEnterRange?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D player)
        {
            if (player.GetComponent<PlayerController>())
            {
                PlayerCancelRange?.Invoke();
            }
        }
    }
}