using UnityEngine;

namespace DefaultNamespace.ConfigsSO
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private Coin coinPrefab;

        public Player PlayerPrefab => playerPrefab;

        public Enemy EnemyPrefab => enemyPrefab;

        public Coin CoinPrefab => coinPrefab;
    }
}