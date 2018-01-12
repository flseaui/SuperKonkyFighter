using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    //CONSTANT
    float FLOOR_HEIGHT = 0;
    float BASE_GRAVITY = -0.05f;
    int NO_ATTACK_INDEX = -1;
    int NO_ATTACK = -1;
    int LIGHT_ATTACK = 0;
    int MEDIUM_ATTACK = 1;
    int HEAVY_ATTACK = 2;
	int SPECIAL_ATTACK = 3;
    int ANIM_STATE = Animator.StringToHash("state");
    int STATUS_NORMAL = 0;
	int CROUCHTIME = 8;
    //int STATUS_BROKEN = 1;

    public bool juggle;
    public bool dashing;

    public KeyCode[] dashKey;

    public int dashDirectKey;
    public bool dashDirect;

    public bool flipping;//flipping variables
	public int flipTimer;
	private bool passDir;
	private bool waitForground;
	
	public bool jumpCrouch;//jump crouch variables
	public int crouchTimer;
	public int jumpPass;

	public float gravity;

    private int stunTimer;

    public float vVelocity;
    public float hVelocity;

    private float forwardSpeed;
    private float backwardSpeed;
    private float jumpSpeed;

	public float baseHeight;
	public float width;
	public float height;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Behaviors behaviors;
    public BoxCollider2D hitbox;
    public BoxCollider2D hurtbox;

    public bool air;
    public bool airLock;

    private int maxHealth;
    private int health;

	public bool pushing;

    public int state;//player actual state, can be out of the player's control
    // 7 8 9
    // 4 5 6
    // 1 2 3
    public int iState;//player input state, doesn't always sync up with state, but is always within control

    public int storedAttackStrength;
    public int bufferFrames;
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
    public bool speLock = false;

    public bool lite = false;
    public bool medium = false;
    public bool heavy = false;
    public bool special = false;

    public bool facingRight;

	public int playerID;

    private KeyCode upKey;
    private KeyCode rightKey;
    private KeyCode downKey;
    private KeyCode leftKey;
    private KeyCode lightKey;
    private KeyCode mediumKey;
    private KeyCode heavyKey;
    private KeyCode specialKey;

    public GameObject otherPlayer;

    public int moveTimer = 0;

    void OnDrawGizmos()
    {
        if (hurtbox.enabled)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5F);
            Gizmos.DrawCube(new Vector2(transform.position.x + hurtbox.offset.x * this.transform.localScale.x, transform.position.y + hurtbox.offset.y), new Vector2(hurtbox.size.x, hurtbox.size.y));
        }

        Gizmos.color = new Color(0, 1, 0, 0.5F);
        Gizmos.DrawCube(new Vector2(transform.position.x + hitbox.offset.x * this.transform.localScale.x, transform.position.y + hitbox.offset.y), new Vector2(hitbox.size.x, hitbox.size.y));
    }

    void Start()
    {
        if (playerID == 1)
        {
            upKey = KeyCode.W;
            rightKey = KeyCode.D;
            downKey = KeyCode.S;
            leftKey = KeyCode.A;
            lightKey = KeyCode.J;
            mediumKey = KeyCode.K;
            heavyKey = KeyCode.L;
			specialKey = KeyCode.Slash;
        }
        else if (playerID == 2)
        {
            upKey = KeyCode.UpArrow;
            rightKey = KeyCode.RightArrow;
            downKey = KeyCode.DownArrow;
            leftKey = KeyCode.LeftArrow;
            lightKey = KeyCode.Keypad4;
            mediumKey = KeyCode.Keypad5;
            heavyKey = KeyCode.Keypad6;
			specialKey = KeyCode.KeypadEnter;
		}

        storedAttackStrength = NO_ATTACK;
        forwardSpeed = 0.25f;
        backwardSpeed = 0.15f;
        jumpSpeed = 1.25f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;
        iState = 5;
        state = 5;

        dashKey = new KeyCode[] { leftKey, rightKey };

		//konky specific things...
		behaviors = new KonkyBehaviours();
		baseHeight = 8;
		width = 8;
	}

    // Update is called once per frame
    private void Update()
    {
        if (!facingRight)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        up = false;
        right = false;
        down = false;
        left = false;
        lite = false;
        medium = false;
        heavy = false;
		special = false;

        if (Input.GetKey(lightKey))
        {
            if (!litLock)
            {
                litLock = true;
                lite = true;
            }
        }
        else
        {
            litLock = false;
        }
        if (Input.GetKey(mediumKey))
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
        if (Input.GetKey(heavyKey))
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
		if (Input.GetKey(specialKey))
		{
			if (!speLock)
			{
				speLock = true;
				special = true;
			}
		}
		else
		{
			speLock = false;
		}

		if (Input.GetKey(upKey))
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

        if (Input.GetKeyUp(dashKey[dashDirectKey])) {
            dashing = false;
        }
        if (Input.GetKey(leftKey))
        {
            if (Input.GetKeyDown(leftKey) && moveTimer > 0 && !dashing)
            {
                dashing = true;
                dashDirectKey = 0;
                if (GetComponent<CameraScript>().history)
                {
                    dashDirect = true;
                }else
                {
                    dashDirect = false;
                }
            }
            else if (!dashing)
            {
                moveTimer = 10;
                left = true;
            }
        }
        if (Input.GetKey(downKey))
        {
            down = true;
        }
        if (Input.GetKey(rightKey))
        {
            right = true;
        }

        if (up && right)
        {
            if (facingRight)
            {
                iState = 9;
				jumpPass = 9;
			}
            else
            {
                iState = 7;
				jumpPass = 7;
			}
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
            if (facingRight)
            {
                iState = 7;
				jumpPass = 7;
            }
            else
            {
                iState = 9;
				jumpPass = 9;
            }
        }
        else if (up)
        {
            iState = 8;
			jumpPass = 8;
        }
        else if (right)
        {
            if (facingRight)
            {
                iState = 6;
            }
            else
            {
                iState = 4;
            }
        }
        else if (down)
        {
            iState = 2;
        }
        else if (left)
        {
            if (facingRight)
            {
                iState = 4;
            }
            else
            {
                iState = 6;
            }
        }
        else
        {
            iState = 5;
        }

        if (lite)
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
        }else if (special)
		{
			iAttack = SPECIAL_ATTACK;
		}
        else
        {
            iAttack = NO_ATTACK;
        }

        if (air)
        {
            airLock = true;
        }

        execute();

        vVelocity += gravity;

        moveX(hVelocity);
        moveY(vVelocity);

        //floor check
        if (y() < FLOOR_HEIGHT)
        {
			if (air)
			{
				attackTimer = 0;
				if (waitForground) {
					waitForground = false;
					flipping = true;
				}
			}
            air = false;
            airLock = false;
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }
        else
        {
            air = true;
        }

        if (x() < -64f)
        {
            setX(-64);
        }else if (x() > 64f)
        {
            setX(64);
        }

        if (!air)
        {
            hVelocity = 0;
        }

        //see what the state should be
        stateCheck();

		//communicate to the animaton controller for player state and attack state
		if (jumpCrouch)
		{
			animInt(ANIM_STATE, -2);
		}
		else if (flipping)
        {
            if (state < 4)
            {
                animInt(ANIM_STATE, -3);
            }
            else
            {
                animInt(ANIM_STATE, -1);
            }
            
        }
        else if (dashing)
        {
            if (dashDirect)
            {
                animInt(ANIM_STATE, -5);
            }
            else
            {
                animInt(ANIM_STATE, -4);
            }
        }
        else if (juggle)
        {
            animInt(ANIM_STATE, 0);
        }
        else if (attacking)
        {
            animInt(ANIM_STATE, 10 + attackState);
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
            attackEnd(STATUS_NORMAL);

            if (state == 6)
            {
                if (iState == 6)
                {
                    if (facingRight)
                    {
                        hVelocity = forwardSpeed;
                    }
                    else
                    {
                        hVelocity = -forwardSpeed;
                    }
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
                    if (facingRight)
                    {
                        hVelocity = -backwardSpeed;
                    }
                    else
                    {
                        hVelocity = backwardSpeed;
                    }
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

		if (state < 4)
		{
			height = baseHeight / 2;
		}
		else
		{
			height = baseHeight;
		}

        if (flipping)
        {
            flipTimer--;
            
            if(flipTimer == 0)
            {
				facingRight = passDir;
				flipping = false;
            }
        }
        if (moveTimer != 0)
        {
            moveTimer--;
        }

		if (jumpCrouch)
		{
			--crouchTimer;

			if (crouchTimer == 0)
			{
				jumpCrouch = false;

				//jump now
				state = jumpPass;
				if (jumpPass == 8)
				{
					airLock = true;
					vVelocity = jumpSpeed;
				}
				else if (jumpPass == 9)
				{
					airLock = true;
					vVelocity = jumpSpeed;
					if (facingRight)
					{
						hVelocity = forwardSpeed * 1.2f;
					}
					else
					{
						hVelocity = -forwardSpeed * 1.2f;
					}
				}
				else if (jumpPass == 7)
				{
					airLock = true;
					vVelocity = jumpSpeed;
					if (facingRight)
					{
						hVelocity = -backwardSpeed * 1.2f;
					}
					else
					{
						hVelocity = backwardSpeed * 1.2f;
					}
				}
			}
		}
    }

    private void execute()//executes your input to do something
    {
        if (!attacking && !jumpCrouch)
        {

            attackStrengh = iAttack;
            executeAttack(attackStrengh);

            if (!airLock)
            {
                state = iState;
				if(state > 6 && !jumpCrouch)
				{
					jumpCrouch = true;
					crouchTimer = CROUCHTIME;
				}
            }
        }
        else
        {
            if (iAttack != NO_ATTACK)
            {
                if (attackTimer <= bufferFrames)
                {
                    storedAttackStrength = iAttack;
                }
                else
                {
                    storedAttackStrength = NO_ATTACK;
                }
            }
        }
    }

    private void executeAttack(int strength)
    {
        if (strength != NO_ATTACK)
        {
            int check = behaviors.getAttack(strength, state);
            if (check != NO_ATTACK_INDEX)
            {
                attacking = true;
                attackState = check;
                attackTimer = behaviors.getTotalTime(attackState);
                bufferFrames = behaviors.getRecoveryTime(attackState);
            }
        }
    }

    private void attackEnd(int status)
    {
        if (attacking)
        {
            attackStrengh = NO_ATTACK;
            attackState = NO_ATTACK_INDEX;
            attacking = false;
            if (status == STATUS_NORMAL)
            {
                if (storedAttackStrength != NO_ATTACK)
                {
					if (!air) {
						state = iState;
					}
					//animBool(true,"restart");
                    executeAttack(storedAttackStrength);
                }
                storedAttackStrength = NO_ATTACK;
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

    public void moveX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x += amm;
        this.transform.position = position;
    }

    public void moveY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y += amm;
        this.transform.position = position;
    }

    public void setY(float amm)
    {
        Vector3 position = this.transform.position;
        position.y = amm;
        this.transform.position = position;
    }

    public void setX(float amm)
    {
        Vector3 position = this.transform.position;
        position.x = amm;
        this.transform.position = position;
    }

    public float y()
    {
        return this.transform.position.y;
    }

    public float x()
    {
        return this.transform.position.x;
    }

    public int getMaxHealth()
    {
        return 0;
    }

    public void flip(bool dir)
    {
		if (air) {
			waitForground = true;
			flipping = false;
		}
		else
		{
			waitForground = false;
			flipping = true;
		}
		passDir = dir;
        flipTimer = 6;
    }
}
