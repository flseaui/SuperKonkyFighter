using UnityEngine;

namespace Player
{
    public class CollisionScript : MonoBehaviour
    {
        private PlayerScript _s;
        private PlayerScript _os;

        private int _selfId;

        public bool Hit;

        public bool InitialFrame;
        public bool Colliding;

        private void Start()
        {
            _s = GetComponentInParent<PlayerScript>();
            _os = _s.otherPlayer.GetComponent<PlayerScript>();
        }

        private void OnDrawGizmos() => Gizmos.color = Color.red;

        private void OnCollisionStay2D(Collision2D col)
        {
            /*
         If ( player.y - otherplayer.y >= “triangle height) {
            If (player.x^2 + player.y^2 <= hitbox.width^2)
                player.onPush
         }
         else 
            player.onPush

    */
            if (!col.collider.CompareTag(tag) && (col.collider.CompareTag("collisionHitbox1") || col.collider.CompareTag("collisionHitbox2")))
            {

                //air to air collision

                //air to ground collision

                if (Mathf.Abs(this.transform.position.y - _os.hitbox.transform.position.y) >= 3)
                {
                    _s.inPushCollision = false;
                    /*
                if ((Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) * (Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) - (this.transform.position.y) * (this.transform.position.y) <= (os.hitbox.size.x) * (os.hitbox.size.x))
                {

                    Debug.Log((Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) * (Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) - (this.transform.position.y) * (this.transform.position.y) <= (os.hitbox.size.x) * (os.hitbox.size.x));
                    s.coll = true;
                    s.onPush(os.hVelocity);
                }
                else
                {
                    s.coll = false;
                }
                */
                }
                else
                {
                    //ground to ground collision

                    _s.inPushCollision = true;
                    //s.onPush(os.hVelocity, os.vVelocity);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _os.hPush = 0;
            _s.inPushCollision = false;
        }
    }
}