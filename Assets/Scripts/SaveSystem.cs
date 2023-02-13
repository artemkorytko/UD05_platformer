using UnityEngine;

namespace DefaultNamespace
{
    public class SaveSystem : ISaveSystem
    {
        private const string SAVE_KEY = "GameData";


        public GameData LoadData()
        {
            return PlayerPrefs.HasKey(SAVE_KEY) ? JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(SAVE_KEY)) : new GameData();
        }

        public void SaveData(GameData gameData)
        {
            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(gameData));
        }
    }
}