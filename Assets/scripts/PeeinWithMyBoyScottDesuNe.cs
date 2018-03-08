using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeeinWithMyBoyScottDesuNe : MonoBehaviour {
    public bool hitting;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((this.CompareTag("hitbox1") && col.collider.CompareTag("hurtbox2")) || (this.CompareTag("hitbox2") && col.collider.CompareTag("hurtbox1")))
        {
            hitting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hitting = false;
    }
}
