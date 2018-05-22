using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public int strength;
    public bool faceWhenShot = default(bool);
    public int shotAction;

    private GameObject stageLeft, stageRight;

    public PlayerScript player { get; set; } = default(PlayerScript);

    private void Start()
    {
        faceWhenShot = player.facingRight;
        shotAction = player.executingAction;
        stageLeft = GameObject.FindWithTag("stageLeft");
        stageRight = GameObject.FindWithTag("stageRight");
    }

    void Update()
    {
        transform.position += faceWhenShot ? new Vector3(speed, 0, 0) : new Vector3(-speed, 0, 0);

        GetComponent<SpriteRenderer>().flipX = faceWhenShot ? false : true;

        if (transform.position.x < stageLeft.transform.position.x || transform.position.x > stageRight.transform.position.x)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("projectile"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (this.player != null)
        {
            if (!col.transform.parent.parent.tag.Equals(player.tag))
            {
                player.otherPlayer.GetComponent<PlayerScript>().health -= strength;
                PlayerScript other = player.otherPlayer.GetComponent<PlayerScript>();
                Action action = player.behaviors.getAction(shotAction);
                Destroy(this.gameObject);
                if (action != null)
                {
                    if (other.airborn)
                    {
                        other.overrideDamage(action.projectileStrength, action.aStrength[player.actionFrameCounter], action.aAngle[player.actionFrameCounter], action.block, action.p1scaling, action.tier, shotAction);
                    }
                    else
                    {
                        other.overrideDamage(action.projectileStrength, action.gStrength[player.actionFrameCounter], action.gAngle[player.actionFrameCounter], action.block, action.p1scaling, action.tier, shotAction);
                    }
                }
            }
        }
    }
}
