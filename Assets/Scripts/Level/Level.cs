using System;
using System.Collections.Generic;
using DefaultNamespace.ConfigsSO;
using DefaultNamespace.EmptyScript;
using UnityEngine;

namespace DefaultNamespace
{
    public class Level : Pool
    {
        [SerializeField] private LevelConfig config;

        private SpawnPointPlayer spawnPointPlayer;
        private SpawnPointEnemy[] _spawnPointsEnemy;
        private SpawnPointCoin[] _spawnPointsCoin;

        private void Awake()
        {
            spawnPointPlayer = GetComponentInChildren<SpawnPointPlayer>();
            _spawnPointsEnemy = GetComponentsInChildren<SpawnPointEnemy>();
            _spawnPointsCoin = GetComponentsInChildren<SpawnPointCoin>();
        }
        
        public void StartLevel()
        {
            GeneratePlayer();
            GenerateEnemy();
            GenerateCoins();
        }
        

        private void GeneratePlayer()
        {
            Instantiate(config.PlayerPrefab, spawnPointPlayer.transform);
        }

        private void GenerateEnemy()
        {
            for (int i = 0; i < _spawnPointsEnemy.Length; i++)
            {
                var enemy = Instantiate(config.EnemyPrefab, _spawnPointsEnemy[i].transform);
                enemy.SetPoints(_spawnPointsEnemy[i].transform, _spawnPointsEnemy[i].GetComponentInChildren<PatrolPoint>().transform);
            }
        }

        private void GenerateCoins()
        {
            GeneratePool(config.CoinPrefab.gameObject, _spawnPointsCoin.Length);
            
            for (int i = 0; i < _spawnPointsCoin.Length; i++)
            {
                if(TryGetItem(out GameObject coin))
                    SetItem(coin, _spawnPointsCoin[i].transform.position);
            }
        }
    }
}