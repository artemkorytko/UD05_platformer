using System;
using System.Collections.Generic;
using DefaultNamespace.ConfigsSO;
using DefaultNamespace.EmptyScript;
using UnityEngine;

namespace DefaultNamespace
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelConfig config;

        private SpawnPointPlayer spawnPointPlayer;
        private SpawnPointEnemy[] _spawnPointsEnemy;
        private SpawnPointCoin[] _spawnPointsCoin;

        private Pool<Coin> _poolCoin;
        private Pool<Enemy> _poolEnemy;
        
        private void Awake()
        {
            spawnPointPlayer = GetComponentInChildren<SpawnPointPlayer>();
            _spawnPointsEnemy = GetComponentsInChildren<SpawnPointEnemy>();
            _spawnPointsCoin = GetComponentsInChildren<SpawnPointCoin>();
            
            _poolCoin = new Pool<Coin>(config.CoinPrefab, _spawnPointsCoin.Length);
            _poolEnemy = new Pool<Enemy>(config.EnemyPrefab, _spawnPointsEnemy.Length);
        }

        
        public void StartLevel()
        {
            GenerateEnemy();
            GenerateCoins();
        }
        

        public void SetPlayer(Player player)
        {
            player.gameObject.SetActive(true);
            player.transform.position = spawnPointPlayer.transform.position;
        }

        private void GenerateEnemy()
        {
            for (int i = 0; i < _spawnPointsEnemy.Length; i++)
            {
                if (_poolEnemy.TryGetObject(out GameObject enemy))
                {
                    _poolEnemy.SetObjectInPosition(enemy, _spawnPointsEnemy[i].transform.position);
                    enemy.GetComponent<Enemy>().SetPoints(_spawnPointsEnemy[i].transform, _spawnPointsEnemy[i].GetComponentInChildren<PatrolPoint>().transform);
                }
            }
        }

        private void GenerateCoins()
        {
            for (int i = 0; i < _spawnPointsCoin.Length; i++)
            {
                if (_poolCoin.TryGetObject(out GameObject coin))
                    _poolCoin.SetObjectInPosition(coin, _spawnPointsCoin[i].transform.position);
            }
        }
    }
}