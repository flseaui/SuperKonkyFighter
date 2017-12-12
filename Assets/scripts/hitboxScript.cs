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
        float h = 12;
        float w = 6;
        float y = script.getY();
        float x = otherScript.getX();
        float tx = (w / 2) * ((y / h) - 1);
        if (y<=h && script.getX() < tx && script.facingRight)
        {
            script.setX(tx);
        }
	}

    private void OnTriggerStay2D(Collider2D col)
    {
        if (hit)
        {
            Debug.Log("esit");
            hit = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (hit)
        {
            Debug.Log("exit");
            hit = false;
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

   /* private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "hitbox")
        {
            if (script.facingRight)
            {
                script.setX(otherScript.getX() - otherScript.hitbox.size.x / 2 - script.hitbox.size.x / 2 - script.hitbox.offset.x);
            }
            else
            {
                script.setX(otherScript.getX() + otherScript.hitbox.size.x / 2 + script.hitbox.size.x / 2 - script.hitbox.offset.x);
            }
        }
    }*/
}
// script.setX(otherScript.getX() - otherScript.hitbox.size.x / 2 - script.hitbox.size.x / 2 - script.hitbox.offset.x);