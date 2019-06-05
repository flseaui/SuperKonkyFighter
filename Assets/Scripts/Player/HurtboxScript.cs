using UnityEngine;

namespace Player
{
    public class HurtboxScript : MonoBehaviour {
        private int _tag;
        private string _oppositeBox;
        public bool Hit;

        private void Start()
        {
            _tag = GetComponentInParent<PlayerScript>().playerId;
            _oppositeBox = _tag == 1 ? "hitbox2" : "hitbox1";
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(_oppositeBox))
            {
                Hit = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Hit = false;
        }

    }
}
