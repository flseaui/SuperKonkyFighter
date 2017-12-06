using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour {

    
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
        if (col.enabled)
        {
            Debug.Log("hit");
            hit = true;
        }
    }


}
