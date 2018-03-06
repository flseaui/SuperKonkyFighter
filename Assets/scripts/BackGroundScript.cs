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

        pushing();

        player[0].decreaseHitboxLifespan();
        player[1].decreaseHitboxLifespan();
    }

    private void pushing()
    {
        float[] diff = new float[2];

        if (p1s.coll)
        {
            Debug.Log("Colliding");

            if (player[0].hPush < 0)
                player[0].hPush = 0;
            if (player[1].hPush < 0)
                player[1].hPush = 0;

            if (Mathf.Abs(player[0].x()) > 64)
                player[1].hPush = -player[1].hPush;
            if (Mathf.Abs(player[1].x()) > 64)
                player[1].hPush = -player[0].hPush;

            for (int i = 0; i < 2; i++)
            {
                float xPos = player[i].hitbox.transform.position.x,
                xPosFuture = xPos + (player[i].facingRight ? player[i].hVelocity - player[i].hPush : -player[i].hVelocity + player[i].hPush),
                otherXPos = player[i + 1].hitbox.transform.position.x,
                otherXPosFuture = otherXPos + (player[i + 1].facingRight ? player[i + 1].hVelocity - player[i + 1].hPush : -player[i + 1].hVelocity + player[i + 1].hPush),
                hitboxWidth = player[i].hitbox.GetComponent<PolygonCollider2D>().bounds.size.x,
                otherHitboxWidth = player[i + 1].hitbox.GetComponent<PolygonCollider2D>().bounds.size.x;

                if (Mathf.Abs((xPosFuture) - (otherXPosFuture)) <= (hitboxWidth / 2 + otherHitboxWidth / 2))
                {
                    diff[i] = ((hitboxWidth / 2 + otherHitboxWidth / 2) - Mathf.Abs((xPosFuture) - (otherXPosFuture)));
                    player[i].vPush = 0;
                }
            }
        }
        else
        {
            diff[0] = 0;
            diff[1] = 0;
        }

        for (int i = 0; i < 2; i++)
        {
            if (Mathf.Abs(player[i].hVelocity) > Mathf.Abs(player[i + 1].hVelocity))
            {
                if (Mathf.Abs(player[i + 1].x()) < 64)
                    diff[i] = 0;
            }
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
