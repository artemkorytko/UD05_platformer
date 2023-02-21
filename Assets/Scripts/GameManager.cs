using System;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        private LevelManager _levelManager;
        private ISaveSystem _saveSystem;
        private GameData _gameData;

        public uint CountCoins => _gameData.Coins;
        public event Action OnRestartLevel; 
        public event Action<uint> OnCoinValueChanged;
        public event Action OnFinishEvent;
        public event Action OnFailEvent;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _saveSystem = new SaveSystem();
            _levelManager = GetComponent<LevelManager>();
            _gameData = _saveSystem.LoadData();
        }

        private void Start()
        {
            UIController.Instance.SetPanels(Panel.Menu);
        }

        private void OnDestroy()
        {
            _saveSystem.SaveData(_gameData);
        }

        public void StartGame()
        {
            OnCoinValueChanged?.Invoke(_gameData.Coins);
            _levelManager.CreateLevel(_gameData.Level);
            UIController.Instance.SetPanels(Panel.Game);
        }

        public void RestartLevel()
        {
            OnRestartLevel?.Invoke();
            UIController.Instance.SetPanels(Panel.Game);
        }

        public void OnCoinCollect()
        {
            _gameData.Coins++;
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void OnFinish()
        {
            OnFinishEvent?.Invoke();
            UIController.Instance.SetPanels(Panel.Win);
        }

        public void OnPlayerDeath()
        {
            UIController.Instance.SetPanels(Panel.Fail);
            OnFailEvent?.Invoke();
        }

        public void OnResurrectionPlayer()
        {
            _gameData.Coins -= 10;
            OnCoinValueChanged?.Invoke(_gameData.Coins);
            OnRestartLevel?.Invoke();
            UIController.Instance.SetPanels(Panel.Game);
        }

    }
}