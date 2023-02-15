using System;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private ISaveSystem _saveSystem;
        private LevelManager _levelManager;

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
            UiController.Instance.SetPanel(Panel.Menu);
        }

        public void StartGame()
        {
            UiController.Instance.SetPanel(Panel.Game);
            _levelManager.CreateLevel(_gameData.Level);
        }

        public void OnCoinCollect()
        {
            _gameData.Coins++;
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void OnFinish()
        {
            UiController.Instance.SetPanel(Panel.Win);
            OnFinishEvent?.Invoke();
            
        }
        
        public void OnPlayerDeath()
        {
            UiController.Instance.SetPanel(Panel.Fail);
            OnFailEvent?.Invoke();
        }

        private void OnApplicationQuit()
        {
            _saveSystem.SaveData(_gameData);
        }
    }
    
}