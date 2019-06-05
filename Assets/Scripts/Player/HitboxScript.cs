using UnityEngine;

namespace Player
{
    public class HitboxScript : MonoBehaviour
    {
        private string _tag;
        public bool Hurt;

        private void OnTriggerEnter2D(Collider2D col)
        {
            Hurt = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Hurt = false;
        }
    }
}
