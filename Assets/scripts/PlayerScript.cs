using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    //CONSTANT
    static float FLOOR_HEIGHT = 0;
    static float BASE_GRAVITY = -0.05f;
    static float BUFFER = 0.1f;
    static int NO_ATTACK_INDEX = -1;
    static int NO_ATTACK = -1;
    static int LIGHT_ATTACK = 0;
    static int MEDIUM_ATTACK = 1;
    static int HEAVY_ATTACK = 2;
    static int ANIM_STATE = Animator.StringToHash("state");
    static int ANIM_ATTACK_STATE = Animator.StringToHash("attack");

    public float friction;
    public float gravity;

    private int stunTimer;

    public float vVelocity;
    public float hVelocity;

    private float forwardSpeed;
    private float backwardSpeed;
    private float jumpSpeed;

    public static Sprite[] textures;

    public Animator animator;
    public Behaviors behaviors;

    public bool air;
    bool airLock = false;
    bool crouching = false;

    public int state;//player actual state, can be out of the player's control
    // 7 8 9
    // 4 5 6
    // 1 2 3
    public int iState;//player input state, doesn't always sync up with state, but is always within control

    public int attack;//attack strength (LMH)
    public int iAttack = -1;//input attack strength (LMH)

    public int attackState;//attack going on rn
    public bool attacking = false;//is there an attack
    public int attackTimer = 0; //time left in attack animation

    int forgiveness = 4; //forgiveness in number of frames to make an input
    public int forTime = 0; //the timer for frame forgiveness

    public bool up    = false;
    public bool left  = false;
    public bool down  = false;
    public bool right = false;

    //start
    void Start()
    {
        behaviors = new KonkyBehaviours();
        animator = GetComponent<Animator>();
        forwardSpeed = 0.25f;
        backwardSpeed = 0.15f;
        jumpSpeed = 1f;
        friction = 0f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;

        iState = 5;
        state = 5;
    }

    // Update is called once per frame
    private void Update () {

        bool forgive = false;

        if (Input.GetKey(KeyCode.Keypad4))
        {
            forgive = true;
            iAttack = LIGHT_ATTACK;
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            forgive = true;
            iAttack = MEDIUM_ATTACK;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            forgive = true;
            iAttack = HEAVY_ATTACK;
        }
        if (Input.GetKey(KeyCode.W))
        {
            forgive = true;
            up = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            forgive = true;
            left = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forgive = true;
            down = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            forgive = true;
            right = true;
        }

        if (forgive && forTime < 0)
        {
            forTime = forgiveness;
        }

        //find what input state the player is in
        if (up && right)
        {
            iState = 9;
        } else if (right && down)
        {
            iState = 3;
        } else if (down && left)
        {
            iState = 1;
        } else if (left && up)
        {
            iState = 7;
        }
        else if (up)
        {
            iState = 8;
        }
        else if (right)
        {
            iState = 6;
        }
        else if (down)
        {
            iState = 2;
        }
        else if (left)
        {
            iState = 4;
        }
        else
        {
            iState = 5;
        }

        if (forTime == 0)//time to actually do something
        {
            if (air)
            {
                airLock = true;
            }

            //set what is inputted to what is going to happen
            state = iState;
            attack = iAttack;

            //reset what is inputted
            up = false;
            left = false;
            down = false;
            right = false;
            iAttack = NO_ATTACK;

            //do something finally
            execute();
        }
        forTime--;

        moveX(hVelocity);
        moveY(vVelocity);

        //floor check
        if (getY() < FLOOR_HEIGHT)
        {
            air = false;
            airLock = false;
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }
        else
        {
            air = true;
        }

        //gravity
        vVelocity += gravity;
        if (!air) {
            hVelocity = 0;
        }

        //see what the state should be
        stateCheck();

        //communicate to the animaton controller for player state and attack state VV
        animInt(ANIM_STATE,state);
        animInt(ANIM_ATTACK_STATE,attackState);
    }

    private void stateCheck() //checks on the current state, resets it if need be (basically exits out of states)
    {
        //attack timer
        if (attackTimer == 0)
        {
            attack = NO_ATTACK_INDEX;
            attacking = false;
        }
        else
        {
            attackTimer--;
        }

        if (state == 6)
        {
            if (iState == 6)
            {
                hVelocity = forwardSpeed;
            }
            else
            {
                state = 5;
            }
        }
        else if (state == 4)
        {
            if (iState == 4)
            {
                hVelocity = -backwardSpeed;
            }
            else
            {
                state = 5;
            }
        }
        else if (state < 4)
        {
            if (iState > 3)
            {
                state = 5;
                crouching = false;
            }
        }
        else if (state > 6)
        {
            if (!air)
            {
                state = 5;
            }
        }
    }

    private void execute()//executes your input to do something
    {
        if (airLock)
        {

        }
        else
        {
            //set attack actually
            if (attackState != NO_ATTACK)
            {
                int check = behaviors.getAttack(attackState, state);
                if (check != NO_ATTACK_INDEX) {//don't attack for a -1 value
                    attacking = true;
                    attack = check;
                    attackTimer = behaviors.getTime(attack);
                }
            }
            //set movements for different states
            if (state==8)
            {
                airLock = true;
                vVelocity = jumpSpeed;
            }
            else if (state == 9)
            {
                airLock = true;
                vVelocity = jumpSpeed;
                hVelocity = forwardSpeed*1.2f;
            }
            else if(state == 7)
            {
                airLock = true;
                vVelocity = jumpSpeed;
                hVelocity = -backwardSpeed*1.2f;
            }
            else if (state < 4)
            {
                crouching = true;
            }
        }
    }

    private void animInt(int hash, int value)
    {
        animator.SetInteger(hash, value);
    }

    private void animBool(bool b, string s)
    {
            animator.SetBool(s, b);
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
