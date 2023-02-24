using System;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace DefaultNamespace
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private AssetReference[] levels;
        [SerializeField] private Player playerPrefab;
        
        private Player _player;
        private Level _currentLevel;

        private void Start()
        {
            _player = Instantiate(playerPrefab, Vector3.zero, quaternion.identity);
            _player.gameObject.SetActive(false);
        }

        public async void CreateLevel(uint index)
        {
            if (_currentLevel)
                Destroy(_currentLevel.gameObject);

            index %= (uint) levels.Length;
            
            var level = await Addressables.InstantiateAsync(levels[index], transform);
            _currentLevel = level.GetComponent<Level>();
            
            _currentLevel.SetPlayer(_player);
            _currentLevel.StartLevel();
        }
        
    }
}