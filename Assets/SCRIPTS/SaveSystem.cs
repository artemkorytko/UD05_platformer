using UnityEngine;

namespace DefaultNamespace
{
    public class SaveSystem : ISaveSystem //наследуется от интерфейссссса
    {
        private const string SAVE_KEY = "GameData";


        public GameData LoadData()
        {
            return PlayerPrefs.HasKey(SAVE_KEY)
                ? JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(SAVE_KEY)) : new GameData();
            // возвращаем    КЛЮЧ ЕСТЬ ? возвращаем вот это : иначе если нету вот это
            
            // ------- тернарка, то же самое что: --------------------
            
            // if (PlayerPrefs.HasKey(SAVE_KEY))
            // {
            //     return JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(SAVE_KEY));
            // }
            // return new GameData();
        }

        
        public void SaveData(GameData gameData)
        {
            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(gameData));
        }
    }
}