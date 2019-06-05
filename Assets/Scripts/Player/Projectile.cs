using UnityEngine;

namespace Player
{
    public class Projectile : MonoBehaviour {

        public float Speed;
        public int Strength;
        public bool FaceWhenShot;
        public int ShotAction;

        private GameObject _stageLeft, _stageRight;

        public PlayerScript Player;

        private void Start()
        {
            FaceWhenShot = Player.facingRight;
            ShotAction = Player.executingAction;
            _stageLeft = GameObject.FindWithTag("stageLeft");
            _stageRight = GameObject.FindWithTag("stageRight");
        }

        private void Update()
        {
            transform.position += FaceWhenShot ? new Vector3(Speed, 0, 0) : new Vector3(-Speed, 0, 0);

            GetComponent<SpriteRenderer>().flipX = !FaceWhenShot;

            if (transform.position.x < _stageLeft.transform.position.x || transform.position.x > _stageRight.transform.position.x)
                Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.tag.Equals("projectile")) return;
            
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (Player == null) return;
            if (col.transform.parent.parent.tag.Equals(Player.tag)) return;
            
            Player.otherPlayer.GetComponent<PlayerScript>().health -= Strength;
            var other = Player.otherPlayer.GetComponent<PlayerScript>();
            var action = Player.behaviors.GetAction(ShotAction);
            Destroy(gameObject);
            
            if (action == null) return;
            if (other.airborn)
                other.OverrideDamage(action.ProjectileStrength, action.AStrength[Player.actionFrameCounter],
                    action.AAngle[Player.actionFrameCounter], action.Block, action.P1Scaling, action.Tier, ShotAction);
            else
                other.OverrideDamage(action.ProjectileStrength, action.GStrength[Player.actionFrameCounter],
                    action.GAngle[Player.actionFrameCounter], action.Block, action.P1Scaling, action.Tier, ShotAction);
        }
    }
}
