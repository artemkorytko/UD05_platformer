using UnityEngine;

namespace DefaultNamespace
{
    public class State : MonoBehaviour // включение/выключение скриптов?(enabled(true/false)) х.з...
    {
        private Transition _transition; // как в аниматоре.. транзиты.. т.е когда произошло что-то тогда мы переключается в другое состояние? ... 
        public void Enter() // установить в сотояние
        {
            
        }

        public void Exit() // выйти из состояния 
        {
        }

        public State GetNextState() // получить след. состояние <= тут нада проверять "транзиты"
        {
            return null;
        }
    }
}