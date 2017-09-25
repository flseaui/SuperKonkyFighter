using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public static bool controllable;
    
    public static int FLOOR_HEIGHT = -2;

    public static float BASE_GRAVITY = -0.163f;

    public static float gravity; 

    public float yvelocity;

	// Use this for initialization
	void Start () {
        yvelocity = 0;
        gravity = BASE_GRAVITY;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("kek'd my b");
            yvelocity = 2;
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

        //PHYSICS

        yvelocity += gravity;

        moveY(yvelocity);
        
        if (this.transform.position.y < FLOOR_HEIGHT)
        {
            setY(FLOOR_HEIGHT);
        }
    }

    private void moveX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x += amm;
        this.transform.position = position;
    }

    private void moveY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y += amm;
        this.transform.position = position;
    }

    private void setY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y = amm;
        this.transform.position = position;
    }

    private void setX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x = amm;
        this.transform.position = position;
    }
}
