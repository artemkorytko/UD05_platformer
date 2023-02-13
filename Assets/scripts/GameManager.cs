using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private ISaveSystem _saveSysrtem;
        private LevelManager _levelManager;

        private GameData _gameData;
        private void Awake()
        {
            _saveSysrtem = new SaveSystem();
            _levelManager = GetComponent<LevelManager>();
            _gameData = _saveSysrtem.LoadData();
        }

        public void StartGame()
        {
            _levelManager.CreateLevel(_gameData.Level);
        }
    }
}