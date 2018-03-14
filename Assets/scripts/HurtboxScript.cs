using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxScript : MonoBehaviour {

    string tag;
    string oppositeBox;
    public bool hit;

    void Start()
    {
        oppositeBox = (tag == "hurtbox1" ? "hitbox2" : "hitbox1");
        Debug.Log(oppositeBox);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(oppositeBox))
        {
            hit = true;
            Debug.Log("gettin hit with ya boy scott " + oppositeBox);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hit = false;
    }

}
