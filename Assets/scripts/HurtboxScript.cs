using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxScript : MonoBehaviour {

    int tag;
    string oppositeBox;
    public bool hit;

    void Start()
    {
        tag = GetComponentInParent<PlayerScript>().playerID;
        oppositeBox = (tag == 1 ? "hitbox2" : "hitbox1");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(oppositeBox))
        {
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hit = false;
    }

}
