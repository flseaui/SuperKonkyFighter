using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour {

    public PlayerScript p1s;
    public PlayerScript p2s;
    public PlayerScript[] player;

    // Use this for initialization
    void Start () { }
	
    public void setScripts(PlayerScript p1s, PlayerScript p2s)
    {
        this.p1s = p1s;
        this.p2s = p2s;

        player = new PlayerScript[]{ p1s, p2s, p1s};  
    }

	// Update is called once per frame
	void Update () {

        float[] diff = new float[2];

        if (p1s.coll)
        {
            for (int i = 0; i < 2; i++)
            {
                float xPos = player[i].hitbox.transform.position.x,
                otherXPos = player[i + 1].hitbox.transform.position.x,
                hitboxWidth = player[i].hitbox.size.x,
                otherHitboxWidth = player[i + 1].hitbox.size.x;

                if (player[i].facingRight)
                {
                  // Debug.Log(i + "player and " + (xPos + (hitboxWidth / 2)));
                    // Debug.Log(xPos);
                    //Debug.Log(otherXPos);
                    if ((xPos + (player[i].facingRight ? player[i].hVelocity - player[i].hPush : -player[i].hVelocity + player[i].hPush) + (hitboxWidth / 2)) > ((otherXPos + (player[i + 1].facingRight ? player[i + 1].hVelocity - player[i + 1].hPush : -player[i + 1].hVelocity + player[i + 1].hPush)) - (otherHitboxWidth / 2)))
                    {
                        //diff[i] = (xPos + (hitboxWidth / 2)) - (otherXPos - (otherHitboxWidth / 2));
                        diff[i] = (player[i].hitbox.size.x) - Mathf.Abs((xPos + (player[i].facingRight ? player[i].hVelocity - player[i].hPush : -player[i].hVelocity + player[i].hPush)) - (otherXPos + (player[i + 1].facingRight ? player[i + 1].hVelocity - player[i + 1].hPush : -player[i + 1].hVelocity + player[i + 1].hPush)));
                    }
                    else
                    {
                        diff[i] = 0;
                      //  Debug.Log(i + "no diff");
                    }

                }
                else
                {
                   // Debug.Log(i + "player and " + (xPos - (hitboxWidth / 2)));

                    if ((xPos + (player[i].facingRight ? player[i].hVelocity - player[i].hPush : -player[i].hVelocity + player[i].hPush) - (hitboxWidth / 2)) < ((otherXPos + (player[i + 1].facingRight ? player[i + 1].hVelocity - player[i + 1].hPush : -player[i + 1].hVelocity + player[i + 1].hPush)) + (otherHitboxWidth / 2)))
                    {
                        //diff[i] = ((xPos - (hitboxWidth / 2)) - (otherXPos + (otherHitboxWidth / 2)));
                        diff[i] = (player[i].hitbox.size.x) - Mathf.Abs((xPos + (player[i].facingRight ? player[i].hVelocity - player[i].hPush : -player[i].hVelocity + player[i].hPush)) - (otherXPos + (player[i + 1].facingRight ? player[i + 1].hVelocity - player[i + 1].hPush : -player[i + 1].hVelocity + player[i + 1].hPush)));

                    }
                    else
                    {
                        diff[i] = 0;
                       // Debug.Log(i + "no diff");
                    }
                }
            }

            if (p1s.hVelocity == 0 && p2s.hVelocity == 0)
            {
                diff[0] = (p1s.hitbox.size.x + 1) - Vector3.Distance(p1s.transform.position, p2s.transform.position);
                diff[1] = (p2s.hitbox.size.x + 1) - Vector3.Distance(p2s.transform.position, p1s.transform.position);
            }
        }

        for (int i = 0; i < 2; i++)
        {

            if (player[i].hVelocity == player[i + 1].hVelocity)
                diff[i] = diff[i] / 2;

        }

        for (int i = 0; i < 2; i++)
        {
            if (player[i].updateEnd != 0)
            {
                player[i].hPush += diff[i];

                if (player[i].updateEnd == 2)
                {
                    player[i].updateEnd = 0;
                    player[i].UpdateEnd();
                }

            }

            if (Mathf.Abs(player[i].transform.position.x - player[i+1].transform.position.x) < player[i].width)
            {
                Debug.Log("Clipping");
            }
            else
            {
                //Debug.Log("Clip1 " + player[i].transform.position.x);
               // Debug.Log("Clip2 " + player[i + 1].transform.position.x);
                Debug.Log("Clipper" + Mathf.Abs(player[i].transform.position.x - player[i + 1].transform.position.x));
                Debug.Log(diff[i]);
            }

        }
    }
}
