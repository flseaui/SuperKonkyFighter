using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public int strength;

    public PlayerScript player { get; set; } = default(PlayerScript);

    void Update()
    {
        transform.position += new Vector3(speed, 0, 0);
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
                    other.damage(action.projectileStrength, action.gStrength[player.actionFrameCounter], action.gAngle[player.actionFrameCounter], action.block, action.p1scaling);
                else
                    other.damage(action.projectileStrength, action.aStrength[player.actionFrameCounter], action.aAngle[player.actionFrameCounter], action.block, action.p1scaling);
            }
        }
    }
}
