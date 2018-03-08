using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeeinWithMyBoyScottDesuNe : MonoBehaviour {
    public bool hitting;

    private void OnCollisionEnter2D(Collision2D col)
    {
        // this broke pushing for SOME reason I have no idea
        // was in attempt to fix collision, realized the issue was diamond blocking all hurtbox collision, messed with layers and changed them back
        // now things are dead
        Debug.Log("enterer:");
        Debug.Log("this tag: " + tag);
        Debug.Log("that tag: " + col.collider.tag);
        if ((this.CompareTag("hitbox1") && col.collider.CompareTag("hurtbox2")) || ((this.CompareTag("hitbox2") && col.collider.CompareTag("hurtbox1"))))
        {
            hitting = true;
            Debug.Log("hittin with ya boy scott");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hitting = false;
    }
}
//you know I had to pee with my boy scott