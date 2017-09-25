using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public static bool controllable;
    
    public static kek;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 position = this.transform.position;
            position.y += 0.04f;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 position = this.transform.position;
            position.x -= 0.04f;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 position = this.transform.position;
            position.y -= 0.04f;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x += 0.04f;
            this.transform.position = position;
        }


    }
}
