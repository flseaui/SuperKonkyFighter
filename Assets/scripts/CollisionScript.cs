using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private PlayerScript s;
    private PlayerScript os;

    int selfID;

    public bool hit;

    public bool initialFrame;
    public bool colliding;

    // Use this for initialization
    void Start()
    {
        s = GetComponentInParent<PlayerScript>();
        os = s.otherPlayer.GetComponent<PlayerScript>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
    }

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

            if (Mathf.Abs(this.transform.position.y - os.hitbox.transform.position.y) >= 3)
            {
                s.inPushCollision = false;
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

                s.inPushCollision = true;
                //s.onPush(os.hVelocity, os.vVelocity);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        os.hPush = 0;
        s.inPushCollision = false;
    }
}