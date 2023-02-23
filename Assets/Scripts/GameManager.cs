using System;
using DefaultNamespace.Ui;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private ISaveSystem _saveSystem;
        private LevelManager _levelManager;

        private GameData _gameData;

        private uint _levelCoins;

        [SerializeField] private uint _resurrectionCost = 10;

        public GameData Data => _gameData;

        public uint ResurrectionCost => _resurrectionCost;

        private Transform _respawnPoint;
        private GameObject _player;
        [SerializeField] private GameObject _playerPrefab;

        public event Action<float> RefreshPlayerHealth;
        public event Action<uint> OnCoinValueChanged;

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
            _respawnPoint = FindObjectOfType<RespawnPointComponent>().transform;
            _player = Instantiate(_playerPrefab, _respawnPoint);
            _player.transform.SetParent(this.transform);

            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void NextLevel()
        {
            _levelManager.CreateLevel(_gameData.Level);
            _respawnPoint = FindObjectOfType<RespawnPointComponent>().transform;
            ReactivatePlayer();
            UiController.Instance.SetPanel(Panel.Game);
            
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void RestartLevel()
        {
            if (_gameData.Coins < _resurrectionCost)
                return;

            if (_levelCoins < _resurrectionCost)
            {
                _levelCoins = 0;
            }
            else
            {
                _levelCoins -= _resurrectionCost;
            }
            _gameData.Coins -= _resurrectionCost;
            ReactivatePlayer();
            UiController.Instance.SetPanel(Panel.Game);
            
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        private void ReactivatePlayer()
        {
            _player.SetActive(false);
            _player.transform.position = _respawnPoint.transform.position;
            _player.SetActive(true);
        }

        public void ReturnToMenu()
        {
            _gameData.Coins -= _levelCoins;
            UiController.Instance.SetPanel(Panel.Menu);
            if (_player != null)
            {
                Destroy(_player);
            }
        }

        public void ResetGameData()
        {
            _gameData.Coins = 0;
            _gameData.Level = 0;
            _saveSystem.SaveData(_gameData);
        }

        public void RefreshHealthBar(float health)
        {
            RefreshPlayerHealth?.Invoke(health);
        }

        public void OnCoinCollect()
        {
            _levelCoins++;
            _gameData.Coins++;
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void OnFinish()
        {
            _gameData.Level++;
            _levelCoins = 0;
            UiController.Instance.SetPanel(Panel.Win);
        }

        public void OnPlayerDeath()
        {
            UiController.Instance.SetPanel(Panel.Fail);
        }

        private void OnApplicationQuit()
        {
            _gameData.Coins -= _levelCoins;
            _saveSystem.SaveData(_gameData);
        }
    }
}