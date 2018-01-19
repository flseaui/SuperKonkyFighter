using System;
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
    //int STATUS_BROKEN = 1;

    public bool juggle;
    public bool dashing;

    public KeyCode[] dashKey;

    public int dashDirectKey;
    public bool dashDirect;
    public bool previousDirect;
    public bool groundDash;

    public int DashTimer;
    bool DashCount;

	public bool passDir;
	public bool waitForGround;
	public bool waitForEnd;
	
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

    public int maxHealth;
    public int health;

	public List<int> history;
	public List<int> delays;
	public int inputTimer;

    public int state;
    public int iState;
	public int heldState;

    public int storedAttackStrength;
    public int bufferFrames;
    public int attackStrengh;//attack strength (LMH)
    public int iAttack = -1;//input attack strength (LMH)

    public int attackState = -1;//attack going on rn
    public bool attacking = false;//is there an attack
    public int attackTimer = 0; //time left in attack animation

	public bool upLock = false;
	public bool leftLock = false;
	public bool downLock = false;
	public bool rightLock = false;

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

	public bool up1 = false;
	public bool left1 = false;
	public bool down1 = false;
	public bool right1 = false;

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

	public int damagePass;

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

		history = new List<int>(new int[] { 5, 5, 5, 5, 5, 5 });
		delays = new List<int>(new int[] { 0, 0, 0, 0, 0, 0 });
		inputTimer = 0;

		dashKey = new KeyCode[] { leftKey, rightKey };

		//konky specific things...
		maxHealth = 11000;
		health = maxHealth;
		behaviors = new KonkyBehaviours();
		baseHeight = 8;
		width = 8;

		//Time.timeScale = 0.1F;
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

		up1 = false;
		right1 = false;
		down1 = false;
		left1 = false;
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
			up1 = true;
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

		if (Input.GetKey(leftKey))
		{
			left1 = true;
			if (!leftLock)
			{
				leftLock = true;
				left = true;
			}
		}
		else
		{
			leftLock = false;
		}

		if (Input.GetKey(downKey))
		{
			down1 = true;
			if (!downLock)
			{
				downLock = true;
				down = true;
			}
		}
		else
		{
			downLock = false;
		}

		if (Input.GetKey(rightKey))
		{
			right1 = true;
			if (!rightLock)
			{
				rightLock = true;
				right = true;
			}
		}
		else
		{
			rightLock = false;
		}

		/*if (Input.GetKeyUp(dashKey[dashDirectKey])) {
            dashing = false;
        }
        if (Input.GetKey(leftKey))
        {
            if(previousDirect)
            {
                moveTimer = 0;
            }
            if (Input.GetKeyDown(leftKey) && moveTimer > 0 && !dashing && DashTimer == 0 && state > 3)
            {
                dashing = true;
                if (!air)
                {
                    groundDash = true;
                }
                dashDirectKey = 0;
                if (facingRight)
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
                previousDirect = false;
            }
        }*/

		/*if (Input.GetKey(rightKey))
        {
            if (!previousDirect)
            {
                moveTimer = 0;
            }
            if (Input.GetKeyDown(rightKey) && moveTimer > 0 && !dashing && DashTimer == 0 && state > 3)
            {
                dashing = true;
                if (!air)
                {
                    groundDash = true;
                }
                dashDirectKey = 1;
                if (!facingRight)
                {
                    dashDirect = true;
                }
                else
                {
                    dashDirect = false;
                }
            }
            else if (!dashing)
            {
                moveTimer = 10;
                right = true;
                previousDirect = true;
            }
        }*/

		if (up1 && right1)
		{
			if (facingRight)
			{
				heldState = 9;
				jumpPass = 9;
			}
			else
			{
				heldState = 7;
				jumpPass = 7;
			}
		}
		else if (right1 && down1)
		{
			heldState = 3;
		}
		else if (down1 && left1)
		{
			heldState = 1;
		}
		else if (left1 && up1)
		{
			if (facingRight)
			{
				heldState = 7;
				jumpPass = 7;
			}
			else
			{
				heldState = 9;
				jumpPass = 9;
			}
		}
		else if (up1)
		{
			heldState = 8;
			jumpPass = 8;
		}
		else if (right1)
		{
			if (facingRight)
			{
				heldState = 6;
				jumpPass = 9;
			}
			else
			{
				heldState = 4;
				jumpPass = 7;
			}
		}
		else if (down1)
		{
			heldState = 2;
		}
		else if (left1)
		{
			if (facingRight)
			{
				heldState = 4;
				jumpPass = 7;
			}
			else
			{
				heldState = 6;
				jumpPass = 9;
			}
		}
		else
		{
			heldState = 5;
		}

		if (up && right)
        {
            if (facingRight)
            {
                iState = 9;
			}
            else
            {
                iState = 7;
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
            }
            else
            {
                iState = 9;
            }
        }
        else if (up)
        {
            iState = 8;
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

		//ADD INPUTS TO HISTORYYYYYYYYY
		//
		if (iState != 5)
		{
			history.Add(iState);
			delays.Add(inputTimer);
			inputTimer = 0;
			history.RemoveAt(0);
			delays.RemoveAt(0);
		}

		if (iAttack != NO_ATTACK)
		{
			history.Add(iAttack);
			delays.Add(inputTimer);
			inputTimer = 0;
			history.RemoveAt(0);
			delays.RemoveAt(0);
		}

		if (iAttack == NO_ATTACK && iState == 5)
		{
			++inputTimer;
		}
		//

        if (air)
        {
            airLock = true;
        }

        if (DashCount)
        {
            state = 999;
        }

        if(DashTimer > 0)
        {
            state = 999;
            dashing = true;
        }

        vVelocity += gravity;

        moveX(hVelocity);
        moveY(vVelocity);

        //floor check
        if (y() < FLOOR_HEIGHT)
        {
			if (air)
			{
				state = 5;
				attackTimer = 0;
				if (waitForGround) {
					waitForGround = false;
					executeAction(32, false);
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
		historyCheck();

        stateCheck();

        if (attacking)
        {
            animInt(ANIM_STATE, attackState);
        }
        else
        {
            animInt(ANIM_STATE, state);
        }
    }

	private void historyCheck()
	{
		int[,] moves = new int[,]
		{
			{4,4},
			{6,6}
		};
		int[,] times = new int[,]
		{
			{16,0},
			{16,0}
		};
		int[] index = new int[]
		{
			34,
			35,
		};
		for (int m = 0; m < moves.GetLength(0); ++m)
		{
			int counter = 0;
			for (int i = history.Count - 1; i > history.Count - 1 - moves.GetLength(1); --i)
			{
				if (history[i] == moves[m,counter] && delays[i] <= times[m,counter])
				{
					if (counter == moves.GetLength(1) - 1)
					{
						executeAction(index[m], false);
						for (int j = history.Count - 1; j > history.Count-1-moves.GetLength(1); --j)
						{
							history[j] = 5;
						}
					}
				}
				else
				{
					break;
				}
				++counter;
			}
		}
	}

    private void stateCheck() //casual loop
    {
        //attack timer
        if (attackTimer == 0)
        {
            attackEnd(STATUS_NORMAL);
        }
        else
        {
            attackTimer--;
        }

		if (!attacking) {
			if (state == 6)
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
			else if (state == 4)
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
		}

		if (state < 4)
		{
			height = baseHeight / 2;
		}
		else
		{
			height = baseHeight;
		}

        if (state == 999)
        {
            dashing = true;
        }

        if (moveTimer != 0)
        {
            moveTimer--;
        }

        if (DashTimer != 0)
        {
            DashTimer--;
        }
        else if(DashCount)
        {
            dashing = false;
            DashCount = false;
        }
        
        if(air && groundDash)
        {
            dashing = false;
            groundDash = false;
        }

        if (dashing)
        {
            if (!dashDirect && !air)
            {
                if (facingRight)
                {
                    hVelocity = forwardSpeed * 2;
                }
                else
                {
                    hVelocity = forwardSpeed * -2;
                }
            }
            else if(dashDirect && !air)
            {
                if (facingRight)
                {
                    hVelocity = forwardSpeed * -1.6f;
                    if (!DashCount)
                    {
                        DashTimer = 20;
                        DashCount = true;
                    }
                }
                else
                {
                    hVelocity = forwardSpeed * 1.6f;
                    if (!DashCount)
                    {
                        DashTimer = 20;
                        DashCount = true;
                    }
                }
            }
            else if (!dashDirect)
            {

            }
            else
            {

            }
        }

		if (!attacking)
		{

			attackStrengh = iAttack;
			executeAction(attackStrengh, true);

			if (!airLock)
			{
				state = heldState;
				if (state > 6)
				{
					executeAction(33, false);
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
			}
		}
	}

    private void executeAction(int strength, bool actual)
    {
		if (actual) {
			if (strength != NO_ATTACK)
			{
				int check = behaviors.getAttack(strength, state);
				if (check != NO_ATTACK_INDEX)
				{
					attacking = true;
					attackState = check + 10;
					attackTimer = behaviors.getTotalTime(check);
					bufferFrames = behaviors.getRecoveryTime(check);
					damagePass = behaviors.getDamage(check);
				}
			}
		}
		else
		{
			attacking = true;
			attackState = strength;
			attackTimer = behaviors.getTotalTime(attackState);
		}
    }

    private void attackEnd(int status)
    {
        if (status == STATUS_NORMAL)
        {
			if (waitForEnd)
			{
				waitForEnd = false;
				if (state < 4)
				{
					executeAction(36, false);
				}
				else
				{
					executeAction(32, false);
				}
			}
			else if (attackState == 32 || attackState == 36)
			{
				shutdown();
				facingRight = passDir;
			}
			else if (attackState == 33)
			{
				shutdown();
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
            else if (storedAttackStrength != NO_ATTACK)
            {
				if (!air) {
					state = heldState;
				}
                executeAction(storedAttackStrength, true);
			}
			else
			{
				shutdown();
				if (state == 6)
				{
					if (heldState != 6)
					{
						state = 5;
					}
				}
				else if (state == 4)
				{
					if (heldState != 4)
					{
						state = 5;
					}
				}
				else if (state < 4)
				{
					if (heldState > 3)
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
			storedAttackStrength = NO_ATTACK;
		}
    }

	private void shutdown()
	{
		attackStrengh = NO_ATTACK;
		attackState = NO_ATTACK_INDEX;
		attacking = false;
		damagePass = 0;
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

    public void flip(bool dir)
    {
		if (air) {
			waitForGround = true;
		}
		else if (attacking)
		{
			waitForEnd = true;
		}
		else
		{
			waitForEnd = false;
			waitForGround = false;
			if (state < 4) {
				executeAction(36, false);
			}
			else
			{
				executeAction(32, false);
			}
		}
		passDir = dir;
    }

	public void damage(int ammount)
	{
		health -= ammount;
	}
}
