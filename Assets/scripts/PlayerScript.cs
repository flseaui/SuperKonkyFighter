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
        { 8,  12, 23, 9,  .75f }, 
        { 10, 14, 26, 11, .8f  }, 
        { 12, 16, 28, 13, .85f }, 
        { 14, 19, 33, 16, .89f }, 
        { 16, 21, 36, 18, .92f }, 
        { 18, 24, 40, 20, .94f }
    };

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

    InputManager inputManager;

    int BasicState;
    int AttackState;
    int AdvState;

    int bufferedMove;

    bool damageDealt;

    public int inputAttack;
    public int inputAdv;

    public int[] jump;

    public int dashTimer;
    public int dashTrack;

    public int currentAction;

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

        inputManager = new InputManager(0);
	}

    // Update is called once per frame
    private void Update()
    {
        if (!hitStopped)
        {
            GameUpdate();
        }
        else
        {
            buffer();
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

    private void GameUpdate()
	{
        inputManager.pollInput(0);

        BasicState = inputConvert(inputManager.currentInput);
        setAttackInput(inputManager.currentInput);
        setAdvancedInput(inputManager.currentInput);

        if (currentAction >= 40)
            incrementFrame(behaviors.getAdvanced(currentAction).frames);
        else
            incrementFrame(behaviors.getAttack(currentAction).frames);
        

        stateCheck();

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
            inputManager.handleInput();
            
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

		moveX(hVelocity + hKnockback);
		moveY(vVelocity + vKnockback);

		if (y() < FLOOR_HEIGHT) //ground snap
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

	private void buffer(int bufferedInput)
	{
        foreach (int action in behaviors.getAttack(inputAttack).attackCancels)
            if (action == bufferedInput)
                bufferedMove = bufferedInput;
        foreach (int action in behaviors.getAttack(inputAdv).advCancels)
            if (action == bufferedInput)
                bufferedMove = bufferedInput;
        foreach (int action in behaviors.getAttack(inputAdv).advCancels)
            if (action == 40 && (bufferedInput == 7 || bufferedInput == 8 || bufferedInput == 9))
                bufferedMove = bufferedInput;
    }
    
    private int inputConvert(bool[] input)
    {
        if (air)
        {
            if (input[2])
                return 7;
            else if (input[3])
                return 8;
            else
                return 9;
        }
        else
        {
            if (input[0])
            {
                if (input[2])
                    return 7;
                else if (input[3])
                    return 9;
                else
                    return 8;
            }
            else if (input[1])
            {
                if (input[2])
                    return 1;
                else if (input[3])
                    return 3;
                else
                    return 2;
            }
            else if (input[2])
                return 4;
            else if (input[3])
                return 6;
            else
                return 5;
        }
    }

    private void setAdvancedInput(bool[] input)
    {
        if (input[2] && dashTrack == 0 && dashTimer != 0)
        {
            if (facingRight)
                AdvState = 2;
            else
                AdvState = 1;

        }else if (input[3] && dashTrack == 1 && dashTimer != 0)
        {
            if (facingRight)
                AdvState = 1;
            else
                AdvState = 2;

        }

        if (input[2] || input[3])
        {
            dashTimer = 15;
            if (input[2])
                dashTrack = 0;
            else
                dashTrack = 1;
        }

        //if (flip)
        //  dashTimer = 0;
            
        if (!(input[2] || input[3]) && dashTimer != 0)
            dashTimer--;
    }

    private void setAttackInput(bool[] input)
    {
        if (input[4])
            inputAttack = BasicState;
        else if (input[5])
            inputAttack = BasicState + 10;
        else if (input[6])
            inputAttack = BasicState + 20;
        else if (input[7])
            inputAttack = BasicState + 30;
        else
            inputAttack = 0;
    }

	private void incrementFrame(int[] frames)
	{	
		int previousFrame = currentFrame;
		currentFrame = frames[actionCounter];
        actionCounter++;

        if (!damageDealt && currentFrame == 1)
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
            damageDealt = false;
			hurtbox.size = new Vector2(0f, 0f);
		}

        if (currentFrame == 3)
        {
            if (bufferedMove != 0)
            {
                AttackState = bufferedMove;
                bufferedMove = 0;
                ActionEnd();
            }
            else if (inputAttack != 0)
            {
                AttackState = inputAttack;
                ActionEnd();
            }
        }
	}

    private void stateCheck()
    {
		if (action) {
			if (!infiniteAction)
				if (actionCounter == actionFrames - 1) //end an action by counting down the action timer
					actionEnd();
				else
					incrementFrame();
		}
		else
		{
			hurtbox.enabled = false;
			hurtbox.size = new Vector2(0f, 0f);
		}

		if (actionState == Behaviors.aDash)
			if (heldState != 6)
                shutdown();

        if (hKnockback != 0)
        {
			if (!air) {
				hKnockback *= .75f;
				if (Mathf.Abs(hKnockback) < 0.005f)
					hKnockback = 0;
			}
			else
			{
				hKnockback *= .5f;
				if (Mathf.Abs(hKnockback) < 0.005f)
					hKnockback = 0;
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

	private void ActionEnd()
	{
        currentAction = 0;
        currentFrame = 0;
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
        health -= ammount;
        damageDealt = true;
        hKnockback = k * Mathf.Cos(((float)angle / 180f) * Mathf.PI) * (facingRight ? -1 : 1);
        vKnockback = k * Mathf.Sin(((float)angle / 180f) * Mathf.PI);

        if(vKnockback > 0)
        {
            air = true;
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
