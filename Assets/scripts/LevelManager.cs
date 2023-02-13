using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] levels;

    private Level _currentLevel;

    public void CreateLevel(uint index)
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }

        index %= (uint)levels.Length;
        _currentLevel = Instantiate(levels[index], transform);
    }
    

}
