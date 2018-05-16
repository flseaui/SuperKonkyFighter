using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public int strength;

    public PlayerScript player { get; set; } = default(PlayerScript);

    void Update()
    {
        transform.position += player.facingRight ? new Vector3(speed, 0, 0) : new Vector3(-speed, 0, 0);

        GetComponent<SpriteRenderer>().flipX = player.facingRight ? false : true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("faggot____");
        if (collision.gameObject.tag.Equals("projectile"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (this?.player)
        {
            if (!col.transform.parent.parent.tag.Equals(player.tag))
            {
                player.otherPlayer.GetComponent<PlayerScript>().health -= strength;
                Destroy(this.gameObject);
                PlayerScript other = player.otherPlayer.GetComponent<PlayerScript>();
                Action action = player.behaviors.getAction(player.executingAction);
                if (!other.airborn)
                {
                    if (action != null)
                        other.damage(action.projectileStrength, action.gStrength[player.actionFrameCounter], action.gAngle[player.actionFrameCounter], action.block, action.p1scaling);
                }
                else
                {
                    if (action != null)
                        other.damage(action.projectileStrength, action.aStrength[player.actionFrameCounter], action.aAngle[player.actionFrameCounter], action.block, action.p1scaling);
                }
            }
        }
    }
}
