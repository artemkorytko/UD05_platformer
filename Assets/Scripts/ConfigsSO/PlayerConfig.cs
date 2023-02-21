using UnityEngine;

namespace DefaultNamespace.ConfigsSO
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float horizontalSpeed = 5f;
        [SerializeField] private float jumpImpulse = 7f;
        [SerializeField] private int health = 3;

        public float HorizontalSpeed => horizontalSpeed;

        public float JumpImpulse => jumpImpulse;

        public int Health => health;
    }
}