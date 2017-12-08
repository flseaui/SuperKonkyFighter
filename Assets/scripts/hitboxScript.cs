using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour {

    public bool hit;
    public bool collide;
    private PlayerScript script;

    // Use this for initialization
    void Start () {
        hit = false;
        collide = false;
        script = this.GetComponentInParent<PlayerScript>();
    }
	
	// Update is called once per frame
	void Update () {
        if (collide)
        {
            
        }
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

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "hitbox")
        {
            script.hVelocity = script.otherPlayer.GetComponent<PlayerScript>().hVelocity;
            Debug.Log("u a n");
        }
    }
}
