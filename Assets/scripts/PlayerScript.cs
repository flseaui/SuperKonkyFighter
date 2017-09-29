using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    public float FLOOR_HEIGHT = -2;
    public float BASE_GRAVITY = -0.05f;
    public float BUFFER = 1;

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

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        forwardSpeed = 1f;
        backwardSpeed = 0.5f;
        friction = 0.5f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;
	}
	
	// Update is called once per frame
	void Update () {

        disable(0);

        if (Input.GetKey(KeyCode.W) && !air)
        {
            air = true;
            vVelocity = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            enable(0);
            hVelocity = -backwardSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //CROUCHING
        }
        if (Input.GetKey(KeyCode.D))
        {
            hVelocity = forwardSpeed;
        }

        //PHYSICS

        hVelocity *= friction;
        moveX(hVelocity);

        vVelocity += gravity;
        moveY(vVelocity);
        
        if (this.transform.position.y < FLOOR_HEIGHT + BUFFER)
        {
            air = false;
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }

    }

    private void enable(int index)
    {
        bool b = animator.GetBool("forward");
        b = true;
    }

    private void disable(int index)
    {
        bool b = animator.GetBool(index);
        b = false;
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
