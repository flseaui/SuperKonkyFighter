using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;

	void Start () {
		
	}
	
	void Update () {
        transform.position += new Vector3(speed, 0, 0);
	}

    void 
}
