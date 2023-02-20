using System;
using System.Collections.Generic;
using DefaultNamespace.EmptyScript;
using UnityEngine;

namespace DefaultNamespace
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private Coin coinPrefab;

        private SpawnPointPlayer spawnPointPlayer;
        private SpawnPointEnemy[] _spawnPointsEnemy;
        private SpawnPointCoin[] _spawnPointsCoin;
        
        private List<GameObject> _coins = new List<GameObject>(10);
        private List<GameObject> _enemys = new List<GameObject>(10);

        private void Awake()
        {
            spawnPointPlayer = GetComponentInChildren<SpawnPointPlayer>();
            _spawnPointsEnemy = GetComponentsInChildren<SpawnPointEnemy>();
            _spawnPointsCoin = GetComponentsInChildren<SpawnPointCoin>();

        }

        private void Start()
        {
            GeneratePlayer();
            GenerateEnemy();
            GenerateCoins();
        }

        private void GeneratePlayer()
        {
            Instantiate(playerPrefab, spawnPointPlayer.transform);
        }

        private void GenerateEnemy()
        {
            for (int i = 0; i < _enemys.Count; i++)
            {
                var enemy = Instantiate(enemyPrefab, _spawnPointsEnemy[i].transform);
               enemy.SetPoints(_spawnPointsEnemy[i].transform, _spawnPointsEnemy[i].GetComponentInChildren<PatrolPoint>().transform);
            }
        }

        private void GenerateCoins()
        {
            _coins = Pool.Instance.GenerateObjects(coinPrefab.gameObject, _spawnPointsCoin.Length);
            for (int i = 0; i < _coins.Count; i++)
            {
                _coins[i].transform.position = _spawnPointsCoin[i].transform.position;
                _coins[i].SetActive(true);
            }
        }
    }
}