using Character;
using Player;
using UnityEngine;

namespace Core
{
    public class BackGroundScript : MonoBehaviour
    {
        public PlayerScript P1S;
        public PlayerScript P2S;
        public PlayerScript[] Player;
        public bool HitStopped, Shake;
        public bool[] IsColliding = new bool[2];
        public int StopTimer, StopNextFrame = 0;
        public float PlayerOneLastX;
        public float PlayerTwoLastX;
        public float Buffer;

        // Use this for initialization
        public void Start() {
            Buffer = 4;
        }

        public void SetScripts(PlayerScript p1S, PlayerScript p2S)
        {
            P1S = p1S;
            P2S = p2S;

            Player = new[] { p1S, p2S, p1S };
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Player[0].hitStopped)
            {
                Player[0].DecreaseHitboxLifespan();
                Player[1].DecreaseHitboxLifespan();
                Player[0].DecreaseHurtboxLifespan();
                Player[1].DecreaseHurtboxLifespan();
            }

            switch (StopNextFrame)
            {
                case 1:
                    StopNextFrame = 0;
                    HitStop((int)P1S.Level(4));
                    break;
                case 2:
                    StopNextFrame = 0;
                    HitStop((int)P2S.Level(4));
                    break;
            }

            CheckCollisions();
            Pushing();

            for (var i = 0; i < 2; i++)
            {
                if (!Player[i].hitStopped)
                    Player[i].UpdateEnd();
            }

            if (HitStopped)
            {
                StopTimer--;
                Time.timeScale = 0;
                if (StopTimer <= 0)
                {
                    HitStopped = false;
                    P1S.hitStopped = false;
                    P2S.hitStopped = false;
                    Shake = false;
                    Time.timeScale = 1;
                }
            }

       
        }

        private void HitStop(int stopLength)
        {
            HitStopped = true;
            P1S.hitStopped = true;
            P2S.hitStopped = true;
            Shake = true;
            StopTimer = stopLength;
        }

        private void CheckCollisions()
        {
            if (P1S.GetComponentInChildren<HurtboxScript>().Hit && !P2S.damageDealt && (P2S.currentFrameType == 1 || P2S.currentFrameType == 5))
            {
                var action = P2S.behaviors.GetAction(P2S.executingAction);
                P2S.damageDealt = true;
                if(!P1S.airborn)
                    P1S.Damage(action.Damage[P2S.actionFrameCounter], action.GStrength[P2S.actionFrameCounter], action.GAngle[P2S.actionFrameCounter], action.Block, action.P1Scaling, action.Tier);
                else
                    P1S.Damage(action.Damage[P2S.actionFrameCounter], action.AStrength[P2S.actionFrameCounter], action.AAngle[P2S.actionFrameCounter], action.Block, action.P1Scaling, action.Tier);
                if (!HitStopped)
                {
                    StopNextFrame = 2;
                }
            }
            if (P2S.GetComponentInChildren<HurtboxScript>().Hit && !P1S.damageDealt && (P1S.currentFrameType == 1 || P1S.currentFrameType == 5))
            {
                var action = P1S.behaviors.GetAction(P1S.executingAction);
                P1S.damageDealt = true;
                if (!P2S.airborn) 
                    P2S.Damage(action.Damage[P1S.actionFrameCounter], action.GStrength[P1S.actionFrameCounter], action.GAngle[P1S.actionFrameCounter], action.Block, action.P1Scaling, action.Tier);
                else
                    P2S.Damage(action.Damage[P1S.actionFrameCounter], action.AStrength[P1S.actionFrameCounter], action.AAngle[P1S.actionFrameCounter], action.Block, action.P1Scaling, action.Tier);
                if (!HitStopped)
                {
                    StopNextFrame = 1;
                }
            }
        }

        private void Pushing()
        {
            // if one above other dib dab dabalab no pumsh
            if (Player[1].transform.position.y <= Player[2].hitbox.bounds.size.y / 2 + Player[2].transform.position.y && Player[2].transform.position.y <= Player[1].hitbox.bounds.size.y / 2 + Player[1].transform.position.y)
            {
                for (var i = 0; i < 2; i++)
                {
                    float xPos = Player[i].hitbox.transform.position.x,

                        xPosFuture = xPos + (Player[i].playerSide ? Player[i].hVelocity : -Player[i].hVelocity),

                        otherXPos = Player[i + 1].hitbox.transform.position.x,

                        otherXPosFuture = otherXPos + (Player[i + 1].playerSide ? Player[i + 1].hVelocity : -Player[i + 1].hVelocity);

                    if (Player[i].playerSide && xPosFuture + Buffer + 1> otherXPosFuture || !Player[i].playerSide && xPosFuture - Buffer  - 1 < otherXPosFuture)
                        IsColliding[i] = true;
                    else
                        IsColliding[i] = false;
                }

                for (var i = 0; i < 2; i++)
                {
                    if (IsColliding[i])
                        Player[i].OnPush(Player[i + 1].hVelocity);

                    if (Player[i].hPush < 0)
                        Player[i].hPush = 0;

                }

                var diff = new float[2];

                if (Mathf.Abs(Player[0].X()) > 64)
                    Player[1].hPush = -Player[1].hPush;
                if (Mathf.Abs(Player[1].X()) > 64)
                    Player[1].hPush = -Player[0].hPush;



                //antichariot mesures

                for (var i = 0; i < 2; i++)
                {
                    float xPos = Player[i].hitbox.transform.position.x,

                        xPosFuture = xPos + (Player[i].playerSide ? Player[i].hVelocity : -Player[i].hVelocity),

                        otherXPos = Player[i + 1].hitbox.transform.position.x,

                        otherXPosFuture = otherXPos + (Player[i + 1].playerSide ? Player[i + 1].hVelocity : -Player[i + 1].hVelocity);

                    if (IsColliding[i] && Player[i].transform.position.y < Player[i + 1].transform.position.y)
                    {
                        if (Player[i].playerSide && xPosFuture + Buffer - otherXPosFuture >= Buffer / 2 || !Player[i].playerSide && xPosFuture - Buffer + otherXPosFuture >= Buffer / 2)
                            Player[i].hPush += (Player[i + 1].hitbox.bounds.size.x + Buffer) / (Player[i + 1].hitbox.bounds.size.y / 2) * Mathf.Abs(Player[i + 1].vVelocity + .25f / 2);
                        else
                            Player[i].hPush += (Player[i + 1].hitbox.bounds.size.x + Buffer) / (Player[i + 1].hitbox.bounds.size.y / 2) * Mathf.Abs(Player[i + 1].vVelocity + .25f / 2);
                    }
                }
            }
        }

        private Vector2 DiffBetweenPoints(Vector2 point1, Vector2 point2)
        {
            return new Vector2(point1.x - point2.x, point1.y - point2.y);
        }

    }
}