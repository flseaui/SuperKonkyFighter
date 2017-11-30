using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    //CONSTANT
    static float FLOOR_HEIGHT = 0;
    static float BASE_GRAVITY = -0.05f;
    static int NO_ATTACK_INDEX = -1;
    static int NO_ATTACK = -1;
    static int LIGHT_ATTACK = 0;
    static int MEDIUM_ATTACK = 1;
    static int HEAVY_ATTACK = 2;
    static int ANIM_STATE = Animator.StringToHash("state");

    public bool juggle;

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
    public PolygonCollider2D hitbox;

    public bool air;
    public bool airLock;
    public bool jumpcrouch;

    private int maxHealth;
    private int health;

    public int state;//player actual state, can be out of the player's control
    // 7 8 9
    // 4 5 6
    // 1 2 3
    public int iState;//player input state, doesn't always sync up with state, but is always within control

    public int attackStrengh;//attack strength (LMH)
    public int iAttack = -1;//input attack strength (LMH)

    public int attackState = -1;//attack going on rn
    public bool attacking = false;//is there an attack
    public int attackTimer = 0; //time left in attack animation

    public bool upLock = false;
    public bool up = false;
    public bool left = false;
    public bool down = false;
    public bool right = false;

    public bool litLock = false;
    public bool medLock = false;
    public bool hevLock = false;
    public bool light = false;
    public bool medium = false;
    public bool heavy = false;

    //start
    void Start()
    {
        behaviors = new KonkyBehaviours();
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
    private void Update()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();

        up = false;
        right = false;
        down = false;
        left = false;
        light = false;
        medium = false;
        heavy = false;

        if (Input.GetKey(KeyCode.Keypad4))
        {
            if (!litLock)
            {
                litLock = true;
                light = true;
            }
        }
        else
        {
            litLock = false;
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            if (!medLock)
            {
                medLock = true;
                medium = true;
            }
        }
        else
        {
            medLock = false;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            if (!hevLock)
            {
                hevLock = true;
                heavy = true;
            }
        }
        else
        {
            hevLock = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (!upLock)
            {
                upLock = true;
                up = true;
            }
        }
        else
        {
            upLock = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            down = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }

        if (up && right)
        {
            iState = 9;
        }
        else if (right && down)
        {
            iState = 3;
        }
        else if (down && left)
        {
            iState = 1;
        }
        else if (left && up)
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

        if (light)
        {
            iAttack = LIGHT_ATTACK;
        }
        else if (medium)
        {
            iAttack = MEDIUM_ATTACK;
        }
        else if (heavy)
        {
            iAttack = HEAVY_ATTACK;
        }
        else
        {
            iAttack = NO_ATTACK;
        }

        if (air)
        {
            airLock = true;
        }

        //do something finally
        execute();

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
        if (!air)
        {
            hVelocity = 0;
        }

        //see what the state should be
        stateCheck();

        //communicate to the animaton controller for player state and attack state VV
        if (juggle)
        {
            animInt(ANIM_STATE, 0);
        }
        else if (attacking)
        {
            animInt(ANIM_STATE, 10+attackState);
        }
        else
        {
            animInt(ANIM_STATE, state);
        }
    }

    private void stateCheck() //checks on the current state, resets it if need be (basically exits out of states)
    {
        //attack timer
        if (attackTimer == 0)
        {
            attackStrengh = NO_ATTACK;
            attackState = NO_ATTACK_INDEX;
            attacking = false;

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
        else
        {
            attackTimer--;

        }

        
    }


    private void execute()//executes your input to do something
    {
        if (!attacking)
        {

            attackStrengh = iAttack;
            
            //set attack actually
            if (attackStrengh != NO_ATTACK && !attacking)
            {
                int check = behaviors.getAttack(attackStrengh, state);
                if (check != NO_ATTACK_INDEX)
                {//don't attack for a -1 value
                    attacking = true;
                    attackState = check;
                    attackTimer = behaviors.getTime(attackState);
                }
            }

            if (!airLock)
            {

                state = iState;

                //set movements for different states
                if (state == 8)
                {
                    airLock = true;
                    vVelocity = jumpSpeed;
                }
                else if (state == 9)
                {
                    airLock = true;
                    vVelocity = jumpSpeed;
                    hVelocity = forwardSpeed * 1.2f;
                }
                else if (state == 7)
                {
                    airLock = true;
                    vVelocity = jumpSpeed;
                    hVelocity = -backwardSpeed * 1.2f;
                }
                else if (state < 4)
                {
                    //of
                }
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

    private int getMaxHealth()
    {
        return 0;
    }
}
