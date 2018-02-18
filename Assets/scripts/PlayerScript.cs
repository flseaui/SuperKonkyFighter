using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    //CONSTANT
    float FLOOR_HEIGHT = 0;
    float BASE_GRAVITY = -0.05f;
    int NO_Attack_INDEX = -1;
    int NO_Attack_STRENGTH = -1;
    int LIGHT_Attack = 0;
    int MEDIUM_Attack = 1;
    int HEAVY_Attack = 2;
	int SPECIAL_Attack = 3;
    int ANIM_STATE = Animator.StringToHash("state");

    public float[,] levelScaling = new float[,]
    { 
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
    public int ActionState;//Attack going on rn
    public bool Attack;//is there an Attack
	public int currentFrame;//current type of frame
	public int[] AttackTypes;//the list of frame types
    public int AttackFrames; //total time
	public int AttackCounter;//the timer that moves along (counting up)
	public int damageCounter;//counts up damage amounts
	public bool infiniteAttack;//INFINTIYNIYIN
    public int lvl;
	public bool AttackOverride;//for buffering
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

    public int basicState;
    public int AttackState;
    public int AdvState;

    int bufferedMove;

    bool damageDealt;

    public int[] jump;

    public int dashTimer;
    public int dashTrack;

    public int currentAttack;

    public bool flipFacing;
    public bool flip;

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

        storedAttackStrength = NO_Attack_STRENGTH;
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

        inputManager = new InputManager(1);
	}

    // Update is called once per frame
    void Update()
    {
        if (!hitStopped)
        {
            GameUpdate();
        }
        else
        {
            buffer(inputConvert(inputManager.currentInput));
        }

        if (stunned)
        {
           
        }
    }

	public void updateAnimation()
	{
		if (Attack)//wow this is actually a lot simpler than before to send anim states to controller
		{
			animInt(ANIM_STATE, AttackState + 10);
		}
		else
		{
			animInt(ANIM_STATE, state);
		}
	}

    private void GameUpdate()
	{
        inputManager.pollInput(0);

        basicState = inputConvert(inputManager.currentInput);
        setAttackInput(inputManager.currentInput);
        setAdvancedInput(inputManager.currentInput);

        if (currentAttack != 0)
            incrementFrame(frameCheck(currentAttack));
        

        stateCheck();

        // If facing right flip x-scale right, otherwise flip x-scale left
        this.transform.localScale = facingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        /*if (stunned)
        {
			--stunTimer;
			if (stunTimer == 0)
			{
				shutdown();
				stunned = false;
			}
		}
		else
		{*/
            inputManager.handleInput();

        // }

		//floor check
		if (x() < -64f)
        {
            setX(-64);
        }
        else if (x() > 64f)
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
        foreach (int Attack in behaviors.getAction(AttackState).ActionCancels)
            if (Attack == bufferedInput)
                bufferedMove = bufferedInput;

        foreach (int Attack in behaviors.getAction(AdvState).advCancels)
            if (Attack == bufferedInput)
                bufferedMove = bufferedInput;
        foreach (int Attack in behaviors.getAction(AdvState).advCancels)
            if (Attack == 40 && (bufferedInput == 7 || bufferedInput == 8 || bufferedInput == 9))
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
        if (input[8] && dashTrack == 0 && dashTimer != 0)
        {
            if (facingRight)
                AdvState = 2;
            else
                AdvState = 1;

        }
        else if (input[9] && dashTrack == 1 && dashTimer != 0)
        {
            if (facingRight)
                AdvState = 1;
            else
                AdvState = 2;

        }

        if (input[8] || input[9])
        {
            dashTimer = 15;
            if (input[2])
                dashTrack = 0;
            else
                dashTrack = 1;
        }

        if (flip)
            if (currentAttack != 0)
                waitForEnd = true;
            else if (air)
                waitForGround = true;
            else
            {
                AdvState = 7;
                dashTimer = 0;
            }

        if (waitForGround && !air)
        {
            waitForGround = false;
            AdvState = 7;
        }
            
        if ((!input[8] || !input[9]) && dashTimer != 0)
            dashTimer--;
    }

    private void setAttackInput(bool[] input)
    {
        if (input[4])
            AttackState = basicState;
        else if (input[5])
            AttackState = basicState + 10;
        else if (input[6])
            AttackState = basicState + 20;
        else if (input[7])
            AttackState = basicState + 30;
        else
            AttackState = 0;
    }

	private void incrementFrame(int[] frames)
	{	
		int previousFrame = currentFrame;
		currentFrame = frames[AttackCounter];
        AttackCounter++;

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
            damageDealt = false;
			hurtbox.size = new Vector2(0f, 0f);
		}

        if (currentFrame == 3)
        {
            if (!air && waitForEnd)
            {
                waitForEnd = false;
                AdvState = 7;
                AttackEnd();
            }
            if (bufferedMove != 0)
            {
                AttackState = bufferedMove;
                bufferedMove = 0;
                AttackEnd();
            }
            else if (AttackState != 0)
            {
                AttackEnd();
            }
        }
	}

    private void stateCheck()
    {
        if (currentAttack != 0)
        {
            if (currentFrame >= frameCheck(currentAttack).Length)
            {
                if (behaviors.getAction(currentAttack).infinite)
                    AttackCounter--;
                else
                    AttackEnd();
            }
        }
        else if (AdvState != 0 || waitForEnd)
        {
            if (waitForEnd)
            {
                waitForEnd = false;
                AdvState = 7;
            }
            currentAttack = AdvState + 40;
        }
        else if (AttackState != 0)
            currentAttack = AttackState;
        else
            basicMove();
    }

    private void basicMove()
    {
        hVelocity = (basicState == 6 ? 
                    (facingRight ? forwardSpeed : -forwardSpeed) : 
                    (basicState == 4 ? 
                    (facingRight ? -backwardSpeed : backwardSpeed) : hVelocity));
    }

    /*

        if (!Attack) {//set velocity when moving forward and backward
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
			if (AttackState == Behaviors.aBDash)
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
			else if (AttackState == Behaviors.aDash || AttackState == Behaviors.aADash)
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
			executeAttack(Behaviors.aJump, false);
	
        // TODO need to figure out way to get current Attack
		//executeAttack(, true);
	*/

	private void AttackEnd()
	{
        Debug.Log("Attack end");
        currentAttack = 0;
        currentFrame = 0;
        AttackCounter = 0;
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
        
	}

    public float level(int wanted)
    {
       return levelScaling[lvl, wanted];
    }

	public void stun(int time)
	{
       
	}

    public void knockbackDecrease()
    {
        if (hKnockback != 0)
        {
            if (!air)
            {
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
    }

    public int[] frameCheck(int calledAttack)
    {
            return behaviors.getAction(currentAttack).frames;
    }

}
