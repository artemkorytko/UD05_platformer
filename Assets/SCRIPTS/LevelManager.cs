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

        public event Action <uint> LevelChanged;

        // логика создания, дестрой предыдущего и создание нового уровня
        public void CreateLevel(uint index) //приходит к нам уровень uint - только положительные
        {
            
            if (_currentLevel) // если существует текущий уровень, проверяет компонент
                               // надо так
            {
                Destroy(_currentLevel.gameObject); 
            }
            
            //------- РАЗКОММЕНТИТЬ ДЛЯ ТЕСТА УРОВНЯ ---------------------
            // index = 5;
            
            // высчитываем индекс - от 0 до сколько в массиве
            index %= (uint)levelsArr.Length; // тоже не может быть отрицательным
            // ??????????????????????????????????????????????
            // я математический дибил, как сдлать разные уровни?, у меня всегда 1й
            // в массиве на объекте все уровни висят 
            // ??????????????????????????????????????????????
 
            
            _currentLevel = Instantiate(levelsArr[index], transform);
            //   transform - парентом будет HOLDER
            
            LevelChanged?.Invoke(index); // переписывает текст в менюхе
        }
    }
}