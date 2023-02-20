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
        //---- инвоки идут прямо в отдельные текстовые поля
        public void OnCoinCollect(bool stolen = false) // по умолчанию не украденное
        {
            if (!stolen)
            {_gameData.Coins++;} //если вещь украдена из enemy приходит bool true - то инвокает обновление UI без ++
            OnCoinValueChanged?.Invoke(_gameData.Coins);
        }

        public void OnCarrotCollect(bool stolen = false)
        {
            if (!stolen)
            {_gameData.Carrots++;}
            OnCarrotValueChanged?.Invoke(_gameData.Carrots);
        }
        
        public void OnBerryCollect(bool stolen = false)
        {
            if (!stolen)
            {_gameData.Berries++;}
            OnBerryValueChanged?.Invoke(_gameData.Berries);
        }
        
        public void OnGemCollect(bool stolen = false)
        {
            if (!stolen)
            {_gameData.Gems++;}
            OnGemValueChanged?.Invoke(_gameData.Gems);
        }
        
        public void OnGoldenPooCollect(bool stolen = false)
        {
            if (!stolen)
            {_gameData.GoldenPoo++;}
            OnGoldenPooValueChanged?.Invoke(_gameData.GoldenPoo);
        }
        
        
        //------------------------------------------------------------
        public void OnFinish()
        {
            UiController.Instance.SetPanel(Panel.Win);
            _gameData.Level++;
            
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