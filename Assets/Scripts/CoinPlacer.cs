using UnityEngine;

namespace DefaultNamespace
{
    public class CoinPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;

        private float gizmosRange = 0.3f;

        public void GenerateCoin()
        {
            Instantiate(coinPrefab, transform);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, gizmosRange);
        }
    }
}