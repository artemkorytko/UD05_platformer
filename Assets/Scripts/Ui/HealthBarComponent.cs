using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class HealthBarComponent : MonoBehaviour
    {
        [SerializeField] private Image health;
        private bool firstData = true;
        private float offset;

        private void Start()
        {
            GameManager.Instance.RefreshPlayerHealth += SetCurrentHealth;
            health.fillAmount = 1f;
        }

        private void OnDestroy()
        {
            GameManager.Instance.RefreshPlayerHealth -= SetCurrentHealth;
        }

        private void SetCurrentHealth(float currentHealth)
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