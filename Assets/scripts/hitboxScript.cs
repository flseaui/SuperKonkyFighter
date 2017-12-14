using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour {

    public bool hit;
    public bool collide;
    private PlayerScript script;
    private PlayerScript otherScript;
	public LineRenderer line;

    // Use this for initialization
    void Start () {
        hit = false;
        collide = false;
        script = this.GetComponentInParent<PlayerScript>();
        otherScript = script.otherPlayer.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update() {
        float h = 12;
        float w = 12;
        float y = script.getY();
        float x = otherScript.getX();
        if (y <= (h + otherScript.getY()) && y >= otherScript.getY()) {
            if (script.facingRight)
            {
                float tx = x + ((w / 2) * (((y+otherScript.getY()) / h) - 1));
                if (script.getX() > tx)
                {
                    script.setX(tx);
                }
            }
            else
            {
                float tx = x - ((w / 2) * (((y + otherScript.getY()) / h) - 1));
                if (script.getX() < tx)
                {
                    script.setX(tx);
                }
            }
        }
	}

	private void OnPostRender()
	{
		line.positionCount = 2;
		/*GL.Begin(GL.LINES);
		GL.Color(Color.red);
		GL.Vertex(new Vector3(0,0,2));
		GL.Vertex(new Vector3(0, 1, 2));
		GL.End();*/
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