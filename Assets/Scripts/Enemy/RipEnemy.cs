using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class RipEnemy : MonoBehaviour
    {
        private Enemy enemy;
        private void Awake()
        {
            enemy = GetComponentInParent<Enemy>();
        }

        public void Rip()
        {
            enemy.gameObject.SetActive(false);
        }
    }
}