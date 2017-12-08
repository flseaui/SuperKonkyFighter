using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour {

    public bool hit;
    public bool collide;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hit = false;
        collide = false;
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.enabled)
        {
            Debug.Log("hit");
            hit = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "hitbox")
        {
            Debug.Log("collide");
            collide = true;
        }
    }
}
