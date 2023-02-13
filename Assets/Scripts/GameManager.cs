using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private LevelManager _levelManager;
        private ISaveSystem _saveSystem;

        private GameData _gameData;

        private void Awake()
        {
            _saveSystem = new SaveSystem();
            _levelManager = GetComponent<LevelManager>();
            _gameData = _saveSystem.LoadData();
        }

        private void StartGame()
        {
            _levelManager.CreateLevel(_gameData.Level);
        }

    }
}