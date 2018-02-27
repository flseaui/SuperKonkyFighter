    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
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

	// Update is called once per frame
	void Update()
	{
        if (s.y() <= (os.height + os.y()) && s.y() >= os.y())
        {
            if (s.x() < os.x())
            {
                float tx = -os.width * Mathf.Sqrt(1 - Mathf.Pow(((s.y() - os.y()) / os.height), 2)) + os.x();
                if (s.x() > tx)
                {
                    if (s.hVelocity > 0)
                    {
                        if (!s.air || os.air)
                        {
                            os.hVelocity += s.hVelocity / 2;
                            s.hVelocity -= s.hVelocity / 2;
                            colliding = true;
                        }
                    }
                    s.setX(tx);
                }
                else
                {
                    colliding = false;
                }
            }
            else
            {
                float tx = os.width * Mathf.Sqrt(1 - Mathf.Pow(((s.y() - os.y()) / os.height), 2)) + os.x();
                if (s.x() < tx)
                {
                    if (s.hVelocity < 0)
                    {
                        if (!s.air || os.air)
                        {
                            os.hVelocity += s.hVelocity / 2;
                            s.hVelocity -= s.hVelocity / 2;
                            colliding = true;
                        }
                    }
                    s.setX(tx);
                }
                else
                {
                    colliding = false;
                }
            }
        }
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

        if (this.transform.position.y - os.hitbox.transform.position.y >= 3)
        {
            Debug.Log("yes");

            if ((Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) * (Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) - (this.transform.position.y) * (this.transform.position.y) <= (os.hitbox.size.x) * (os.hitbox.size.x))
            {

                Debug.Log((Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) * (Mathf.Abs(this.transform.position.x) - Mathf.Abs(os.hitbox.transform.position.x)) - (this.transform.position.y) * (this.transform.position.y) <= (os.hitbox.size.x) * (os.hitbox.size.x));


                if (!col.collider.CompareTag(tag) && (col.collider.CompareTag("collisionHitbox1") || col.collider.CompareTag("collisionHitbox2")))
                    s.onPush(os.hVelocity);
            }
        }
        else
        {
            if (!col.collider.CompareTag(tag) && (col.collider.CompareTag("collisionHitbox1") || col.collider.CompareTag("collisionHitbox2")))
                s.onPush(os.hVelocity);
        }

	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        os.hPush = 0;
    }

}