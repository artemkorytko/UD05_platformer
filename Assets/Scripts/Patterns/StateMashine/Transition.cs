using UnityEngine;

namespace DefaultNamespace
{
    public class Transition : MonoBehaviour
    {
        private State _nextState; // это будет когда мы что-то сделали тогда получили это стостояние... 
        private bool needTransit; // если мы что-то сделали, то будет true;
    }
}