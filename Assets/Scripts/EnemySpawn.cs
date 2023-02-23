using UnityEngine;

namespace DefaultNamespace
{
    public enum Enemies
    {
        None,
        Slime
    }
    
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private Enemies enemies;
        [SerializeField] private GameObject slimePrefab;

        private float gizmosRange = 0.3f;

        public void GenerateEnemy()
        {
            if (enemies == Enemies.None)
            {
                return;
            }

            if (enemies == Enemies.Slime)
            {
                Instantiate(slimePrefab, transform);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, gizmosRange);
        }
    }
}