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
            player.otherPlayer.GetComponent<PlayerScript>().health -= strength;
            Destroy(this.gameObject);
        }
    }
}
