using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    //CONSTANT
    float FLOOR_HEIGHT = 0;
    float BASE_GRAVITY = -0.05f;
    int NO_ATTACK_INDEX = -1;
    int NO_ATTACK_STRENGTH = -1;
    int LIGHT_ATTACK = 0;
    int MEDIUM_ATTACK = 1;
    int HEAVY_ATTACK = 2;
	int SPECIAL_ATTACK = 3;
    int ANIM_STATE = Animator.StringToHash("state");

    public float[,] levelScaling = new float[,] { 
        { 8, 12, 23, 9, .75f }, 
        { 10, 14, 26, 11, .8f }, 
        { 12, 16, 28, 13, .85f }, 
        { 14, 19, 33, 16, .89f }, 
        { 16, 21, 36, 18, .92f }, 
        { 18, 24, 40, 20, .94f }
    };

    public bool juggle;
    public bool dashing;

    public KeyCode[] dashKey;

    public int dashDirectKey;
    public bool dashDirect;
    public bool previousDirect;
    public bool groundDash;

	bool dashed;
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

    public bool hitStopped;
    public bool stunned;

    public int maxHealth;
    public int health;

    public int state;

    public int storedAttackStrength;
    public int bufferFrames;

	private int STARTUP = 0;
	private int ACTIVE = 1;
	private int BREAK = 2;
	private int RECOVERY = 3;
    public int actionState;//attack going on rn
    public bool action;//is there an attack
	public int currentFrame;//current type of frame
	public int[] attackTypes;//the list of frame types
    public int actionFrames; //total time
	public int actionCounter;//the timer that moves along (counting up)
	public int damageCounter;//counts up damage amounts
	public bool infiniteAction;//INFINTIYNIYIN
    public int lvl;
	public bool actionOverride;//for buffering
    public int gAnglePass;
    public float gKnockpass;
    public int aAnglePass;
    public float aKnockpass;
    public int classPass;

	public bool up1;
	public bool left1;
	public bool down1;
	public bool right1;

	public bool facingRight;
	public int playerID;

    public GameObject otherPlayer;
    public JoyScript JoyScript;

	public int[] damages;
	public int damagePass;
    List<int> cancel;

    public float hKnockback;
    public float vKnockback;

    public int meter;

    InputHandler inputHandler;

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
		this.tag = playerID.ToString();
		hitbox.tag = playerID.ToString();
		hurtbox.tag = playerID.ToString();

        storedAttackStrength = NO_ATTACK_STRENGTH;
        forwardSpeed = 0.25f;
        backwardSpeed = 0.15f;
        jumpSpeed = 1.25f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;

		cancel = new List<int>();
		dashed = false;

		//konky specific things...
		maxHealth = 11000;
		health = maxHealth;
		behaviors = new KonkyBehaviours();
		baseHeight = 8;
		width = 4;

        inputHandler = new InputHandler(0);
	}

    // Update is called once per frame
    private void Update()
    {
        if (!hitStopped)
        {
            SubUpdate();
        }
        if (stunned)
        {
            executeAction(Behaviors.aStun, false);
        }
    }

	public void updateAnimation()
	{
		if (action)//wow this is actually a lot simpler than before to send anim states to controller
		{
			animInt(ANIM_STATE, actionState + 10);
		}
		else
		{
			animInt(ANIM_STATE, state);
		}
	}

    private void SubUpdate()
	{
        inputHandler.pollInput(0);

        // If facing right flip x-scale right, otherwise flip x-scale left
        this.transform.localScale = facingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        if (stunned)
        {
			--stunTimer;
			if (stunTimer == 0)
			{
				shutdown();
				stunned = false;
			}
		}
		else
		{
            inputHandler.handleInput();
		}

		//floor check
		if (x() < -64f)
        {
            setX(-64);
        }else if (x() > 64f)
        {
            setX(64);
        }

        if (!air && !GetComponentInChildren<HitboxScript>().colliding)
        {
            hVelocity = 0;
		}

		vVelocity += gravity;

		if (vVelocity < -1)
		{
			vVelocity = -1;
		}

		stateCheck();

		historyCheck();

		moveX(hVelocity + hKnockback);
		moveY(vVelocity + vKnockback);

		if (y() < FLOOR_HEIGHT)//ground snap
		{
			if (air)
			{
				state = 5;
				shutdown();
				if (waitForGround)
				{
					waitForGround = false;
					executeAction(Behaviors.aTurn, false);
				}
			}
			dashed = false;
			air = false;
			vVelocity = 0;
			setY(FLOOR_HEIGHT);
		}
		else
		{
			air = true;
		}

		updateAnimation();
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
			{16,-1},
			{16,-1}
		};
		int[] index = new int[]
		{
			Behaviors.aBDash,
			Behaviors.aDash,
		};
		for (int m = 0; m < moves.GetLength(0); ++m)//m i sthe variable that counts thorugh the moves array starting from 0 and going to the end
		{
			//the counter counts up every time through the i loop
			int counter = 0;
			//the i loop goes through history, starting at the last position and going down by a number of moves required by the move trying to be activated (2)
			for (int i = 5; i > 3; --i)
			{
				if (history[i] == moves[m,counter])
				{
					if (counter == moves.GetLength(1) - 1)
					{
						if (!dashed) {
							dashed = true;
							if (m==1) {
								if (air)
								{
									executeAction(Behaviors.aADash, false);
								}
								else
								{
									executeAction(index[m], false);
								}
							}
							else
							{
								executeAction(index[m], false);
							}
						}
						for (int j = history.Count - 1; j > history.Count - 1 - moves.GetLength(1); --j)
						{
							history[j] = 5;
						}
						break;
						
					}
				}
				else
				{
					break;
				}
				if (delays[i] > times[m, counter])
				{
					break;
				}
				++counter;
			}
		}
	}

	private void incrementFrame()
	{	
		int previousFrame = currentFrame;
		currentFrame = attackTypes[actionCounter];
        actionCounter++;

        if (previousFrame != 1 && currentFrame == 1)
		{
			otherPlayer.GetComponentInChildren<HitboxScript>().already = false;
			damagePass = damages[damageCounter];
            ++damageCounter;
        }

		if (currentFrame == 1)
			hurtbox.enabled = true;

		else
		{
			hurtbox.enabled = false;
			hurtbox.size = new Vector2(0f, 0f);
		}
	}

    private void stateCheck() //casual loop
    {
		if (action) {
			if (!infiniteAction) {
				if (actionCounter == actionFrames - 1)//end an action by counting down the action timer
				{
					actionEnd();
				}
				else
				{
					incrementFrame();
				}
			}
		}
		else
		{
			hurtbox.enabled = false;
			hurtbox.size = new Vector2(0f, 0f);
		}

		if (actionState == Behaviors.aDash)
		{
			if (heldState != 6)
			{
				shutdown();
			}
		}

        if (hKnockback != 0)
        {
			if (!air) {
				hKnockback *= .75f;
				if (Mathf.Abs(hKnockback) < 0.005f)
				{
					hKnockback = 0;
				}
			}
			else
			{
				hKnockback *= .5f;
				if (Mathf.Abs(hKnockback) < 0.005f)
				{
					hKnockback = 0;
				}
			}
        }

        if (vKnockback != 0)
        {
            vKnockback *= .85f;
            if (Mathf.Abs(vKnockback) < 0.005f)
            {
                vKnockback = 0;
            }
        }

        if (!action) {//set velocity when moving forward and backward
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

		else{//set velocity when dashing
			if (actionState == Behaviors.aBDash)
			{
				if (facingRight)
				{
					hVelocity = -forwardSpeed * 1.5f;
					vVelocity = 0;
				}
				else
				{
					hVelocity = forwardSpeed * 1.5f;
					vVelocity = 0;
				}
			}
			else if (actionState == Behaviors.aDash || actionState == Behaviors.aADash)
			{
				if (facingRight)
				{
					hVelocity = forwardSpeed * 3f;
					vVelocity = 0;
				}
				else
				{
					hVelocity = -forwardSpeed * 3f;
					vVelocity = 0;
				}
			}
		}

		if (state < 4)//set the height for crouching
		{
			height = baseHeight / 2;
		}
		else
		{
			height = baseHeight;
		}

        // If airborn and state > 6: jump
		if (!air && state > 6)
			executeAction(Behaviors.aJump, false);
	
        // TODO need to figure out way to get current attack
		//executeAction(, true);
	}

    private void executeAction(int strength, bool attacking)//new and improved! lots of canceling!
    {
		int place;//derive which place in the list of actions the requested action is

		if (attacking)
		{
			place = (state - 1) * 3 + strength;
		}
		else
		{
			place = strength;
		}

		bool executeAction_pass = true;//make sure the attack in cancelable basically
		if (action)
		{
			executeAction_pass = (cancel.Contains(place) && currentFrame == RECOVERY) || actionOverride;
		}

		if (executeAction_pass)
		{

			Action act = behaviors.getAction(place);
	
			if (act != null)
			{
				action = true;
				actionState = place;

                lvl = act.level;
				attackTypes = act.frames;
				actionCounter = 0;
				damageCounter = 0;
                gAnglePass = act.gAngle;
                gKnockpass = act.gStrength;
                aAnglePass = act.aAngle;
                aKnockpass = act.aStrength;
                classPass = act.attackClass;
                actionFrames = attackTypes.Length;
				infiniteAction = act.infinite;
				incrementFrame();
				otherPlayer.GetComponentInChildren<HitboxScript>().already = false;

				cancel = new List<int>(act.cancels);

				if (attacking)
				{
					damages = act.damage;
					damagePass = damages[0];
				}
			}
		}
		else
		{
			if (currentFrame == RECOVERY)//check if you can buffer then do that
			{
				storedAttackStrength = iAttack;
			}
		}
    }

    private void actionEnd()
    {
		if (waitForEnd)
		{
			waitForEnd = false;
			if (state < 4)
			{
				executeAction(Behaviors.aCTurn, false);
			}
			else
			{
				executeAction(Behaviors.aTurn, false);
			}
		}
		else if (actionState == Behaviors.aTurn || actionState == Behaviors.aCTurn)
		{
			shutdown();
			facingRight = passDir;
		}
		else if (actionState == Behaviors.aJump)
		{
			shutdown();
			state = jumpPass;
			if (jumpPass == 8)
			{
				vVelocity = jumpSpeed;
			}
			else if (jumpPass == 9)
			{
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
        else if (storedAttackStrength != NO_ATTACK_STRENGTH)
        {
			actionOverride = true;
            executeAction(storedAttackStrength, true);
			actionOverride = false;
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
		storedAttackStrength = NO_ATTACK_STRENGTH;
    }

	private void shutdown()
	{
		actionState = NO_ATTACK_INDEX;
		action = false;
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
		else if (action)
		{
			waitForEnd = true;
		}
		else
		{
			waitForEnd = false;
			waitForGround = false;
			if (state < 4) {
				executeAction(Behaviors.aCTurn, false);
			}
			else
			{
				executeAction(Behaviors.aTurn, false);
			}
		}
		passDir = dir;
    }

    public int damage(int ammount, float k, int angle, int ac, bool bl)
    {
        Debug.Log(ammount);
        health -= ammount;
        hKnockback = k * Mathf.Cos(((float)angle / 180f) * Mathf.PI) * (facingRight ? -1 : 1);
        vKnockback = k * Mathf.Sin(((float)angle / 180f) * Mathf.PI);
        if(vKnockback > 0)
        {
            air = true;
            airLock = true;
        }
        vVelocity = 0;
        if (!bl)
        {
            switch (ac)
            {
                case 0:
                    return 4;
                case 1:
                    return 8;
                case 2:
                    return 12;
                case 3:
                    return 8;
                default:
                    return 2;
            }
        }
        else
        {
            return 2;
        }
    }

	public void block(int amm)
	{
        stunned = true;
        stunTimer = amm;
        executeAction(Behaviors.aBlock, false);
	}

    public float level(int wanted)
    {
       return levelScaling[lvl,wanted];
    }

	public void stun(int time)
	{
        shutdown();
		stunned = true;
		stunTimer = time;
		actionOverride = true;
		executeAction(Behaviors.aStun, false);
		actionOverride = false;
        Debug.Log("stunend");
	}

}
