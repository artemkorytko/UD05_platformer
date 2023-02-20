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

        public void StartGame()
        {
            _levelManager.CreateLevel(_gameData.Level);
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

    }
}