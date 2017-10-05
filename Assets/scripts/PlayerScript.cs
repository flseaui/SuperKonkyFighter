using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    public float FLOOR_HEIGHT = -2;
    public float BASE_GRAVITY = -0.05f;
    public float BUFFER = 0.1f;

    public float friction;
    public float gravity;

    public int stunTimer;

    public float vVelocity;
    public float hVelocity;

    public float forwardSpeed;
    public float backwardSpeed;

    public static Sprite[] textures;

    public Animator animator;

    public bool air;
    public bool forward;
    public bool backward;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        forwardSpeed = 0.25f;
        backwardSpeed = 0.15f;
        friction = 0f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;
	}
	
	// Update is called once per frame
	void Update () {

        backward = false;
        forward = false;

        if (Input.GetKey(KeyCode.W) && !air)
        {  
            vVelocity = 1f;
        }
        if (Input.GetKey(KeyCode.A) && !air)
        {
            backward = true;
            hVelocity = -backwardSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //CROUCHING
        }
        if (Input.GetKey(KeyCode.D) && !air)
        {
            forward = true;
            hVelocity = forwardSpeed;
        }

        moveX(hVelocity);
        moveY(vVelocity);

        if (getY() < FLOOR_HEIGHT)
        {
            air = false;
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }
        else
        {
            air = true;
        }

        vVelocity += gravity;
        if (!air) {
            hVelocity = 0;
        }

        boolSet(air, "air");
        boolSet(forward, "forward");
        boolSet(backward, "backward");
    }

    private void boolSet(bool b, string s)
    {
            animator.SetBool(s, b);
    }

    private void aEnable(string index)
    {
        animator.SetBool(index, true);
    }

    private void aDisable(string index)
    {
        animator.SetBool(index,false);
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

    private float getY()
    {
        return this.transform.position.y;
    }

    private float getX()
    {
        return this.transform.position.x;
    }
}
