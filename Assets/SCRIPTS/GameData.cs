using System;

// для save system
namespace DefaultNamespace
{
    [Serializable] // чтобы сохранять при всех возиожных кейсах
    public class GameData
    {
        public uint Coins; // U - не сможем написать отрицательное! от 0 до 21474967200...
        public uint Carrots;
        public uint Berries;
        public uint Gems;
        public uint GoldenPoo;
        
        
        public uint Level;
    }
}