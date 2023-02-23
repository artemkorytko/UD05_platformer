using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image health;
        private bool firstData = true;
        private float offset;

        private void Start()
        {
            health.fillAmount = 1f;
        }

        public void SetCurrentHealth(float currentHealth)
        {
            if (firstData)
            {
                offset = 1 / currentHealth;
                firstData = false;
            }
            else
            {
                health.fillAmount = currentHealth * offset;
            }
        }
    }
}