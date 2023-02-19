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
        public GameData _gameData; // для загрузки гейм-даты
        
        public event Action<uint> OnCoinValueChanged;
        public event Action<uint> OnCarrotValueChanged;
        public event Action<uint> OnBerryValueChanged;
        public event Action<uint> OnGemValueChanged;
        public event Action<uint> OnGoldenPooValueChanged;
        
        public event Action OnFinishEvent;
        public event Action OnFailEvent;
        
        private void Awake()
        {
            //------singleton-----------
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            //---------------------------

            _saveSystem = new SaveSystem(); // создаем систему
            _levelManager = GetComponent<LevelManager>(); // висит на HOLDER
            _gameData = _saveSystem.LoadData(); // получаем сюда гейм-дату
        }


        private void Start()
        {
            UiController.Instance.SetPanel(Panel.Menu);
        }

        public void StartGame()
        {
            UiController.Instance.SetPanel(Panel.Game);
            _levelManager.CreateLevel(_gameData.Level); 
            
            //--------- обновляет менюху с собранным -----------
            OnCoinValueChanged?.Invoke(_gameData.Coins);
            OnCarrotValueChanged?.Invoke(_gameData.Carrots);
            OnBerryValueChanged?.Invoke(_gameData.Berries);
            OnGemValueChanged?.Invoke(_gameData.Gems);
            OnGoldenPooValueChanged?.Invoke(_gameData.GoldenPoo);
        }

        //--------------- СОБИРАЛКИ шесть одинаковых зато наглядно:/ ---------------------------------
        //---- инвоки идут прямо в текстовые поля
        public void OnCoinCollect()
        {
            _gameData.Coins++;
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void OnCarrotCollect()
        {
            _gameData.Carrots++;
            OnCarrotValueChanged?.Invoke(_gameData.Carrots);
        }
        
        public void OnBerryCollect()
        {
            _gameData.Berries++;
            OnBerryValueChanged?.Invoke(_gameData.Berries);
        }
        
        public void OnGemCollect()
        {
            _gameData.Gems++;
            OnGemValueChanged?.Invoke(_gameData.Gems);
        }
        
        public void OnGoldenPooCollect()
        {
            _gameData.GoldenPoo++;
            OnGoldenPooValueChanged?.Invoke(_gameData.GoldenPoo);
        }
        
        
        //------------------------------------------------------------
        public void OnFinish()
        {
            UiController.Instance.SetPanel(Panel.Win);
            OnFinishEvent?.Invoke(); 
            // ---------- дорисовать анимацию радости --------!!!
            
            
        }

        public void OnEnemySteal() // было OnPlayerDeath()
        {
            // UiController.Instance.SetPanel(Panel.Fail);
            OnFailEvent?.Invoke(); 
        }

        private void OnApplicationQuit()
        {
            _saveSystem.SaveData(_gameData);
        }
    }
}