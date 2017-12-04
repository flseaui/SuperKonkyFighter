using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour {

    public BoxCollider2D hitbox;
    public bool hit;
    public bool entering;

	// Use this for initialization
	void Start () {
        hit = false;
        entering = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("hit");
        hit = true;
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "player")
        {
            entering = true;
            Debug.Log("reeeeeeeeeeeeeee");
        }
        else if (col.gameObject.tag != "player" && entering == true)
        {
            entering = false;
            Debug.Log("hit");
            hit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "p1HurtBox")
        {
            Debug.Log("reeeeeeeeeeeeeee");
        }
    }

}
