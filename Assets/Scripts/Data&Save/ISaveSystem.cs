namespace DefaultNamespace
{
    public interface ISaveSystem
    {
        public GameData LoadData();
        public void SaveData(GameData gameData);
    }
}