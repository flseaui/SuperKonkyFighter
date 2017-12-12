using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour {

    public bool hit;
    public bool collide;
    private PlayerScript script;
    private PlayerScript otherScript;

    // Use this for initialization
    void Start () {
        hit = false;
        collide = false;
        script = this.GetComponentInParent<PlayerScript>();
        otherScript = script.otherPlayer.GetComponent<PlayerScript>();
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

    /*private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "hitbox")
        {
            Debug.Log("collide");
            collide = true;
            if (script.facingRight)
            {
                script.hVelocity = -.25f;
            }
            else
            {
                script.hVelocity = .25f;
            }
        }
    }*/

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "hitbox")
        {
            if (script.facingRight)
            {
                script.setX(otherScript.getX() - otherScript.hitbox.size.x / 2 - script.hitbox.size.x / 2);
            }
            else
            {
                script.setX(otherScript.getX() + otherScript.hitbox.size.x / 2 + script.hitbox.size.x / 2);
            }
        }
    }
}
