using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private ISaveSystem _saveSystem;
        private LevelManager _levelManager;

        private GameData _gameData;

        private void Awake()
        {
            _saveSystem = new SaveSystem();
            _levelManager = GetComponent<LevelManager>();
            _gameData = _saveSystem.LoadData();
        }

        public void StartGame()
        {
            _levelManager.CreateLevel(_gameData.Level);
        }
    }
}