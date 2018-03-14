using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxScript : MonoBehaviour {

    string tag;
    string oppositeBox;
    bool hit;

    private void Start()
    {
        oppositeBox = (tag == "hurtbox1" ? "hitbox2" : "hitbox1");
        Debug.Log(oppositeBox);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("COSMPEFPSEFMPSEMFPSEFM");
        if (col.collider.CompareTag(oppositeBox))
        {
            hit = true;
            Debug.Log("hittin with ya boy scott");
        }
    }

}
