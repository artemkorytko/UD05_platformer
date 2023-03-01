using UnityEngine;

namespace TRIGGERSand_CO
{
    public class Collectable : InteractableThis
    {
        public void ColliderOff()
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}