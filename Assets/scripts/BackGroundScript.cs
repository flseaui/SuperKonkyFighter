using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    public PlayerScript p1s;
    public PlayerScript p2s;
    public PlayerScript[] player;
    public bool hitStopped, shake;
    public bool[] isColliding = new bool[2];
    public int stopTimer, stopNextFrame = 0;
    public float playerOneLastX;
    public float playerTwoLastX;
    public float buffer;

    // Use this for initialization
    void Start() {
        buffer = 4;
    }

    public void setScripts(PlayerScript p1s, PlayerScript p2s)
    {
        this.p1s = p1s;
        this.p2s = p2s;

        player = new PlayerScript[] { p1s, p2s, p1s };
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraScript.BOTH_SPAWNED) return;
        if (!player[0].hitStopped)
        {
            player[0].decreaseHitboxLifespan();
            player[1].decreaseHitboxLifespan();
            player[0].decreaseHurtboxLifespan();
            player[1].decreaseHurtboxLifespan();
        }

        switch (stopNextFrame)
        {
            case 1:
                stopNextFrame = 0;
                hitStop((int)p1s.level(4));
                break;
            case 2:
                stopNextFrame = 0;
                hitStop((int)p2s.level(4));
                break;
        }

        checkCollisions();
        pushing();

        for (int i = 0; i < 2; i++)
        {
            if (!player[i].hitStopped)
                player[i].UpdateEnd();
        }

        if (hitStopped)
        {
            stopTimer--;
            Time.timeScale = 0;
            if (stopTimer <= 0)
            {
                hitStopped = false;
                p1s.hitStopped = false;
                p2s.hitStopped = false;
                shake = false;
                Time.timeScale = 1;
            }
        }

       
    }

    void FixedUpdate()
    {

       
    }

    private void hitStop(int stopLength)
    {
        hitStopped = true;
        p1s.hitStopped = true;
        p2s.hitStopped = true;
        shake = true;
        stopTimer = stopLength;
    }

    private void checkCollisions()
    {
        if (p1s.GetComponentInChildren<HurtboxScript>().hit && !p2s.damageDealt && (p2s.currentFrameType == 1 || p2s.currentFrameType == 5))
        {
            Action action = p2s.behaviors.getAction(p2s.executingAction);
            p2s.damageDealt = true;
            if(!p1s.airborn)
                p1s.damage(action.damage[p2s.actionFrameCounter], action.gStrength[p2s.actionFrameCounter], action.gAngle[p2s.actionFrameCounter], action.block, action.p1scaling, action.tier);
            else
                p1s.damage(action.damage[p2s.actionFrameCounter], action.aStrength[p2s.actionFrameCounter], action.aAngle[p2s.actionFrameCounter], action.block, action.p1scaling, action.tier);
            if (!hitStopped)
            {
                stopNextFrame = 2;
            }
        }
        if (p2s.GetComponentInChildren<HurtboxScript>().hit && !p1s.damageDealt && (p1s.currentFrameType == 1 || p1s.currentFrameType == 5))
        {
            Action action = p1s.behaviors.getAction(p1s.executingAction);
            p1s.damageDealt = true;
            if (!p2s.airborn) 
                p2s.damage(action.damage[p1s.actionFrameCounter], action.gStrength[p1s.actionFrameCounter], action.gAngle[p1s.actionFrameCounter], action.block, action.p1scaling, action.tier);
            else
                p2s.damage(action.damage[p1s.actionFrameCounter], action.aStrength[p1s.actionFrameCounter], action.aAngle[p1s.actionFrameCounter], action.block, action.p1scaling, action.tier);
            if (!hitStopped)
            {
                stopNextFrame = 1;
            }
        }
    }

    private void pushing()
    {
        // if one above other dib dab dabalab no pumsh
        if ((player[1].transform.position.y <= player[2].hitbox.bounds.size.y / 2 + player[2].transform.position.y) && (player[2].transform.position.y <= player[1].hitbox.bounds.size.y / 2 + player[1].transform.position.y))
        {
            for (int i = 0; i < 2; i++)
            {
                float xPos = player[i].hitbox.transform.position.x,

                xPosFuture = xPos + (player[i].playerSide ? player[i].hVelocity : -player[i].hVelocity),

                otherXPos = player[i + 1].hitbox.transform.position.x,

                otherXPosFuture = otherXPos + (player[i + 1].playerSide ? player[i + 1].hVelocity : -player[i + 1].hVelocity);

                if ((player[i].playerSide && (xPosFuture + buffer + 1> otherXPosFuture)) || (!player[i].playerSide && (xPosFuture - buffer  - 1 < otherXPosFuture)))
                    isColliding[i] = true;
                else
                    isColliding[i] = false;
            }

            for (int i = 0; i < 2; i++)
            {
                if (isColliding[i])
                    player[i].onPush(player[i + 1].hVelocity);

                if (player[i].hPush < 0)
                    player[i].hPush = 0;

            }

            float[] diff = new float[2];

            if (Mathf.Abs(player[0].x()) > 64)
                player[1].hPush = -player[1].hPush;
            if (Mathf.Abs(player[1].x()) > 64)
                player[1].hPush = -player[0].hPush;



            //antichariot mesures

            for (int i = 0; i < 2; i++)
            {
                float xPos = player[i].hitbox.transform.position.x,

                xPosFuture = xPos + (player[i].playerSide ? player[i].hVelocity : -player[i].hVelocity),

                otherXPos = player[i + 1].hitbox.transform.position.x,

                otherXPosFuture = otherXPos + (player[i + 1].playerSide ? player[i + 1].hVelocity : -player[i + 1].hVelocity);

                if (isColliding[i] && player[i].transform.position.y < player[i + 1].transform.position.y)
                {
                    if ((player[i].playerSide && xPosFuture + buffer - otherXPosFuture >= buffer / 2) || (!player[i].playerSide && xPosFuture - buffer + otherXPosFuture >= buffer / 2))
                        player[i].hPush += ((player[i + 1].hitbox.bounds.size.x + buffer) / (player[i + 1].hitbox.bounds.size.y / 2)) * Mathf.Abs(player[i + 1].vVelocity + .25f / 2);
                    else
                        player[i].hPush += ((player[i + 1].hitbox.bounds.size.x + buffer) / (player[i + 1].hitbox.bounds.size.y / 2)) * Mathf.Abs(player[i + 1].vVelocity + .25f / 2);
                }
            }
        }
    }

    private Vector2 diffBetweenPoints(Vector2 point1, Vector2 point2)
    {
        return new Vector2(point1.x - point2.x, point1.y - point2.y);
    }

}