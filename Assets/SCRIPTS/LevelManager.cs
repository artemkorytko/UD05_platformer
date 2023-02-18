using System;
using UnityEngine;
using UnityEngine.UIElements;
// хранит массив уровней и создает их по мере запроаса
// висит на HOLDER


namespace DefaultNamespace
{
    public class LevelManager : MonoBehaviour
    {
        // массив уровней
        [SerializeField] private Level[] levelsArr;
        // масив ждет на вход того, у кого еcть компонент level 

        //ссылка на текущий уровень
        private Level _currentLevel;

        // логика создания, дестрой предыдущего и создание нового уровня
        public void CreateLevel(uint index) //приходит к нам уровень uint - только положительные
        {
            if (_currentLevel) // если существует текущий уровень, проверяет компонент
                               // надо так
            {
                Destroy(_currentLevel.gameObject); 
            }

            // высчитываем индекс - от 0 до сколько в массиве
            index %= (uint)levelsArr.Length; // тоже не может быть отрицательным
            _currentLevel = Instantiate(levelsArr[index], transform);
            // transform - парентом будет HOLDER

        }
    }
}