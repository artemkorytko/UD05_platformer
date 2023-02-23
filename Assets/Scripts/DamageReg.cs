using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class DamageReg : MonoBehaviour
    {
        public event Action<float> TakeDamage;
        
        public void TakingDamage(float damage)
        {
            TakeDamage?.Invoke(damage);
        }
    }
}