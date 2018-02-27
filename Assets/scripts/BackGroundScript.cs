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
                   Debug.Log(i + "player and " + (xPos + (hitboxWidth / 2)));
                    // Debug.Log(xPos);
                    //Debug.Log(otherXPos);
                    if ((xPos + (hitboxWidth / 2)) < (otherXPos - (otherHitboxWidth / 2)))
                        diff[i] = (xPos + (hitboxWidth / 2)) - (otherXPos - (otherHitboxWidth / 2));
                    else
                        diff[i] = 0;

                }
                else
                {
                    Debug.Log(i + "player and " + (xPos - (hitboxWidth / 2)));

                    if ((xPos - (hitboxWidth / 2)) > (otherXPos + (otherHitboxWidth / 2)))
                        diff[i] = ((xPos - (hitboxWidth / 2)) - (otherXPos + (otherHitboxWidth / 2)));
                    else
                        diff[i] = 0;
                }
            }

            if (p1s.hVelocity == 0 && p2s.hVelocity == 0)
            {
                diff[0] = 0.01f;
                diff[1] = 0.01f;
            }
        }

        for (int i = 0; i < 2; i++)
        {

            if (player[i].hVelocity < player[i].hVelocity)
                diff[i] = 0;
            else if (player[i].hVelocity == player[i + 1].hVelocity)
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

        }
    }
}
