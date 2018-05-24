using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    #region VARIABLES

    private float FLOOR_HEIGHT = 0;
    private float BASE_GRAVITY = -0.05f;
    public float MAP_WITDH = 46f;

    private float[,] levelScaling = new float[,]
    {
        //Hitstun | CH Hitstun | Untech Time | CH Untech Time | Hitstop	| CH Hitstop | Blockstun | P2 Scaling
        { 10,       14,          12,           23,              8,        5,           9,          .84f },
        { 12,       16,          12,           23,              9,        6,           11,         .87f },
        { 14,       18,          14,           26,              10,       7,          13,         .90f },
        { 17,       22,          17,           31,              11,       9,          16,         .93f },
        { 19,       24,          19,           34,              12,       10,          18,         .96f },
        { 21,       27,          21,           36,              13,       11,          20,         .99f }
    };

    /*
Level Hitstun CH Hitstun Untech Time CH Untech Time	Hitstop	CH Hitstop Blockstun P2 Scaling
0	  10	  14         12	         23	            8	    8	       9	     75
1	  12	  16         12	         23	            9	    9	       11	     80
2	  14	  18         14	         26	            10	    11	       13	     85
3	  17	  22         17	         31	            11	    13	       16	     89
4	  19	  24         19	         34	            12	    17	       18	     92
5	  21	  27         21	         36	            13	    21	       20	     94 
    */

    public bool passedPlayerInAir;     // true if pass other player while airborn DEAD!!!!!!!!!!!!!!!!
    public bool passedPlayerInAction;  // true if pass other player while they're in an action DEAD!!!!!!!!!!!!!!!!!!!!
    public bool airborn;               // true if in the air
    public bool hitStopped;            // true if in hitstop
    public bool hitStunned;            // true if in hitstun
    public bool shouldFlip;            // true when pass other player in air, signifying flipping upon landing DEAD!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public bool shouldBlock;           // true when the player should block this frame
    public bool facingRight;           // true if the player is facing right
    public bool playerSide;            // true if left of the other player, false if right
    public bool dashingForwards;       // true if dashing forward, false if dashing back
    public bool airbornActionUsed;     // true if player has spent airial action
    public bool damageDealt;           // true if damage was dealt this frame
    public bool inPushCollision;       // true if player push boxes are currently colliding
    public bool alreadyExecutedAttackMove;
    public bool shouldWallbounce;
    public bool shouldGroundbounce;
    public bool firstStun;
    public bool intialBound;
    public bool jumpSquated;

    public int bufferedMove;           // the move currently buffered
    public int maxHealth;              // starting health of the player
    public int maxMeter;
    public int health;                 // current health of the player
    public int healthStore;
    public int currentFrameType;       // frame type of the currently executing action
    public int actionFrameCounter;     // current frame number of the executing action
    public int activeFrameCounter;     // current active frame number of the executing action
    public int playerID;               // the players id, either 1 or 0
    public int meterCharge;            // current meter charge
    public int meterStore;
    public int dashTimer;              // counts number of frames player has been dashing for
    public int executingAction;        // currently executing action
    public int stunTimer;              // timer counting down the time player has been in hitstun
    public int previousBasicState;     // basic state last frame
    public int basicAnimFrame;         // current frame number of playing animation
    public int blockTimer;             // Timer for counting down block stun
    public int throwType;
    public int comboTimer;
    public int comboCounter;
    public int cancelLimit = 0;

    /* MOVEMENT NUMPAD NOTATION
     * [ jump back ][ jump ][ jump forwards ]
     * [ walk back ][      ][ walk forwards ]
     * [crouch back][crouch][crouch forwards]
     */
    public int basicState; // current movement state

    /* ATTACK NUMPAD NOTATION
     * [       ][        ][       ]
     * [ light ][ medium ][ heavy ]
     * [       ][        ][       ]
     */
    public int attackState; // current attack state

    /* ADVANCED STATES
     * 1 - forward dash
     * 2 - back dash
     * 3 - forward air dash
     * 4 - backward air dash
     * 5 - stun
     */
    public int advancedState; // current advanced state

    /* JUMP STATES
     * 0 - no jump
     * 7 - back jump
     * 8 - up jump
     * 9 - forward jump
     */
    public int jumpDirection;   // current jump direction

    /* UPDATE STATES
     * 0 - ganeUpdate
     * 1 - hitstop / no update
     * 2 - updateEnd
     */
    public int updateState;            // keeps track of what type of update the player should execute
    public int overrideAction;          // highest level state that overrides every other state

    public float hKnockback;          // horizontal knockback
    public float vKnockback;          // vertical knockback
    public float gravity;             // gravity constant, default to BASE_GRAVITY
    public float vVelocity;           // vertical velocity
    public float hVelocity;           // horizontal velocity
    public float hPush;               // amount the player will be horizontally pushed
    public float forwardSpeed;        // forward movement speed constant
    public float backwardSpeed;       // backwards movement speed constant
    public float jumpDirectionSpeed;  // jump speed constant
    public float pushBuffer;          // the buffer before pushing takes place
    public float wallBuffer;
    public float p1Scale;
    public float p2Scale;
    public float stored;
    public float camRight;
    public float camLeft;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public RuntimeAnimatorController konkyAnimationController;
    public RuntimeAnimatorController greyshirtAnimationController;
    public Behaviors behaviors;
    public BoxCollider2D hitbox;
    public GameObject otherPlayer;
    public JoyScript JoyScript;
    public InputManager inputManager;
    public Transform cameraLeft, cameraRight;
    public IntVariable p1ComboCounterText, p2ComboCounterText;
    private IntVariable comboCounterText;
    public GameObject projectile;

    public List<float> livingHitboxesIds;        // the ids of all living hitboxes
    public List<float> livingHitboxesLifespans;  // the lifespans of all living hitboxes
    public List<float> livingHurtboxesIds;       // the ids of all living hurtboxes
    public List<float> livingHurtboxesLifespans; // the lifespans of all living hurtboxes

    //temp variables
    AIController testAI;
    bool ai = false;

    #endregion

    /*
     * AND IF YOU WIN YOU CAN FUCK GREYSHIRT IN THE SPECIAL FEATURES
     */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(transform.position);
    }

    // initialize variables based on current players character
    void Start()
    {

        this.tag = playerID.ToString();
        this.transform.GetChild(0).tag = "collisionHitbox" + playerID.ToString();
        hitbox = GetComponentInChildren<BoxCollider2D>();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic((AudioManager.Music) UnityEngine.Random.Range(2, 8));
        

        if (CompareTag("1"))
        {
            comboCounterText = p1ComboCounterText;
            inputManager = new InputManager(1);
            switch (PlayerPrefs.GetInt("player1c"))
            {
                case 0:
                    behaviors = new KonkyBehaviours();
                    animator.runtimeAnimatorController = konkyAnimationController;
                    break;
                case 1:
                    behaviors = new GreyshirtBehaviours();
                    animator.runtimeAnimatorController = greyshirtAnimationController;
                    break;
            }
        }
        else if (CompareTag("2"))
        {
            comboCounterText = p2ComboCounterText;
            inputManager = new InputManager(2);
            switch (PlayerPrefs.GetInt("player2c"))
            {
                case 0:
                    behaviors = new KonkyBehaviours();
                    animator.runtimeAnimatorController = konkyAnimationController;
                    break;
                case 1:
                    behaviors = new GreyshirtBehaviours();
                    animator.runtimeAnimatorController = greyshirtAnimationController;
                    break;
            }
        }

        vVelocity = 0;
        hVelocity = 0;
        behaviors.setStats();
        forwardSpeed = behaviors.getForwardSpeed();
        backwardSpeed = behaviors.getBackwardSpeed();
        jumpDirectionSpeed = behaviors.getJumpDirectionSpeed();
        maxHealth = behaviors.getMaxHealth();
        maxMeter = behaviors.getMaxMeter();
        gravity = behaviors.getGravity();
        health = maxHealth;
        pushBuffer = 4;
        wallBuffer = 3.5f;

        livingHitboxesIds = new List<float>();
        livingHitboxesLifespans = new List<float>();

        testAI = new KonkyAI();
    }

    // called 60 times per second
    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Y)) { ai = !ai; }

        // get currently held keys or pressed buttons
        if (InputManager.isInputEnabled)
            inputManager.pollInput(1, playerID);


        testAI.observe(0, otherPlayer.GetComponent<PlayerScript>().position(), otherPlayer.GetComponent<PlayerScript>().executingAction, position(), facingRight);
        if (!hitStopped)
        {

        //set basic and attack states    
        setStates();

        preAction();

        // check whether to continue or end action
        stateCheck();

        }   

        // zero horizontal and vertical push
        hPush = 0;

        if (!hitStopped)
        {
            // if executing an action
            if (executingAction != 0)
            {
                if (executingAction < 40)
                {
                    if (!airborn)
                        hVelocity = 0;
                    actionMove(executingAction);

                    if (executingAction < 40 && damageDealt && !alreadyExecutedAttackMove)
                        attackMove(executingAction);
                }                
                else if (executingAction >= 40)
                    advancedMove();

                if (executingAction != 0)
                {
                    // progress the current action
                    incrementFrame(behaviors.getAction(executingAction).frames);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void preAction()
    {
        switch (currentFrameType)
        {
            case 3:
                checkForFlip(false);

                bufferAction();

                // cancel into buffered move
                if (bufferedMove != 0)
                    swapBuffers();
                break;
            case 4:
                bufferAction();
                break;

            case 1:
                bufferAction();
                break;              
            case 5:
                bufferAction();
                break;
        }

        if (executingAction == 0)
            checkForFlip(false);
    }

    public void checkForFlip( bool specialCase)
    {
        if(playerSide != facingRight && executingAction != 41 && executingAction != 52)
        {
            if (!airborn || specialCase)
                facingRight = !facingRight;
            if (!airborn && !specialCase)
                if (basicState < 4)
                    overrideAction = 50;
                else
                    overrideAction = 49;
        }
    }

    public void cleanup()
    {
        hPush = 0;
        vKnockback *= .5f;
        overrideAction = 0;
        if (comboTimer > 0 && (executingAction != 45 && executingAction != 56 && executingAction != 54))
            comboTimer--;
        if (comboTimer == 0)
        {
            if (otherPlayer.GetComponent<PlayerScript>().healthStore > 0)
            {
                otherPlayer.GetComponent<PlayerScript>().healthStore -= (otherPlayer.GetComponent<PlayerScript>().healthStore >= 25 ? 25 : otherPlayer.GetComponent<PlayerScript>().healthStore);
            }

            otherPlayer.GetComponent<PlayerScript>().comboCounter = 0;
            p1Scale = 0;
            p2Scale = 1;
            if (health <= 0)
            {
                if (airborn)
                    executingAction = 54;
                else
                    executingAction = 59;
            }
        }
        if (airborn && basicState < 7)
            basicState = 8;
        if (!airborn && executingAction == 54)
        {
            ActionEnd();
            if (health <= 0)
                executingAction = 59;
            else
                executingAction = 53;
        }

        if (otherPlayer.GetComponent<PlayerScript>().executingAction == 59 && !airborn)
        {
            ActionEnd();

            hVelocity = 0;
            vVelocity = 0;
            hKnockback = 0;
            vKnockback = 0;

            executingAction = 58;
        }
        
        if (meterStore > 0)
        {
            meterCharge += (meterStore >= 25? 25 : meterStore);
            meterStore -= (meterStore >= 25 ? 25 : meterStore);
        }
    }

    private void setStates()
    {
        // updates basic state accordingly 
        if (playerID == 1 && ai)
        {
            basicState = inputConvert(testAI.getInput());
            setAttackInput(testAI.getInput());

            // if jumping reset attackState
            if (attackState % 10 >= 7 && !airborn)
                attackState = 0;
            setAdvancedInput(testAI.getInput());
            setSpecialInput(testAI.getInput());
        }
        else
        {
            basicState = inputConvert(inputManager.currentInput);
            setAttackInput(inputManager.currentInput);

            // if jumping reset attackState
            if (attackState % 10 >= 7 && !airborn)
                attackState = 0;
            setAdvancedInput(inputManager.currentInput);
            setSpecialInput(inputManager.currentInput);
        }
    }

    // update attackState from current input
    private void setAttackInput(bool[] input)
    {
        if (input[4])
            attackState = basicState;
        else if (input[5])
            attackState = basicState + 10;
        else if (input[6])
            attackState = basicState + 20;
        else if (input[7])
        {
            attackState = basicState + 30;
            if (attackState == 35) {
                if (meterCharge < maxMeter)
                    attackState = 0;
            }
        }
        else
            attackState = 0;
    }

    private void setSpecialInput(bool[] input)
    {
        if (airborn)
        {
            if (input[7])
            {
                if (playerSide)
                {
                    if (input[1])
                    {
                        if (input[2])
                        {
                            if (behaviors.getAction(31).airOK)
                                attackState = 31;
                        }
                        else if (input[3])
                        {
                            if (behaviors.getAction(33).airOK)
                                attackState = 33;
                        }
                    }
                    else
                    {
                        if (input[2])
                        {
                        if (behaviors.getAction(34).airOK)
                            attackState = 34;
                        }
                        else if (input[3])
                        {
                            if (behaviors.getAction(35).airOK)
                                attackState = 36;
                        }
                    }
                }
                else
                {
                    if (input[1])
                    {
                        if (input[3])
                        {
                        if (behaviors.getAction(31).airOK)
                            attackState = 31;
                        }
                        else if (input[2])
                        {
                        if (behaviors.getAction(33).airOK)
                            attackState = 33;
                        }
                    }
                    else
                    {
                        if (input[3])
                        {
                        if (behaviors.getAction(34).airOK)
                            attackState = 34;
                        }
                        else if (input[2])
                        {
                        if (behaviors.getAction(36).airOK)
                            attackState = 36;
                        }
                    }
                }
            }
        }
    }

    // updates advanced state and deals with advanced action stopping
    private void setAdvancedInput(bool[] input)
    {
        // if not dashing forwards
        if (executingAction != 41)
        {
            // if left held dashing and not facing forward
            if (input[8] && !dashingForwards && dashTimer != 0)
            {
                if (playerSide)
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            //backward air dash
                            advancedState = 4;
                            airbornActionUsed = true;
                            checkForFlip(true);
                        }
                    }
                    else
                        // back dash
                        advancedState = 2;
                }
                else
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            //forward air dash
                            advancedState = 3;
                            airbornActionUsed = true;
                            checkForFlip(true);
                        }
                    }
                    else
                        // forward dash
                        advancedState = 1;
                }
                dashTimer = 0;
            }
            // if right held dashing and not facing forward
            else if (input[9] && dashingForwards && dashTimer != 0)
            {
                if (playerSide)
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            // forward air dash
                            advancedState = 3;
                            airbornActionUsed = true;
                            checkForFlip(true);
                        }
                    }
                    else
                        // forward dash
                        advancedState = 1;
                }
                else
                {
                    if (airborn)
                    {
                        if (!airbornActionUsed)
                        {
                            // backward air dash
                            advancedState = 4;
                            airbornActionUsed = true;
                            checkForFlip(true);
                        }
                    }
                    else
                        // backward dash
                        advancedState = 2;
                }
                dashTimer = 0;
            }

            // if not left or right held and dashing decrement timer
            if ((!input[8] || !input[9]) && dashTimer != 0)
                dashTimer--;

            // if left or right held set dash timer
            if (input[8] || input[9])
            {
                dashTimer = 15;
                if (input[8])
                    dashingForwards = false;
                else
                    dashingForwards = true;
            }

            if (input[2] && facingRight && executingAction == 0)
                shouldBlock = true;
            else if (input[3] && !facingRight && executingAction == 0)
                shouldBlock = true;
            else if ((executingAction >= 46 && executingAction <= 48))
                shouldBlock = true;
            else
                shouldBlock = false;

            if (!airborn && input[6] && (input[2] || input[3]) && !input[1])
            {
                if (input[2])
                    throwType = 0;
                else if (input[3])
                    throwType = 1;
                advancedState = 15;
            }
        }
    }

    // if the inputted action can be cancelled into from the current action buffer it
    private void buffer(int bufferedInput)
    {
        foreach (int action in behaviors.getAction(executingAction).actionCancels)
            if (action == bufferedInput)
                bufferedMove = bufferedInput;
    }

    // executes a frame of the current action
    private void incrementFrame(int[] frames)
    {
        if (actionFrameCounter == 0)
            killAllBoxes();

        if ((currentFrameType == 1 || currentFrameType == 5) && (frames[actionFrameCounter + 1] != 1 && frames[actionFrameCounter + 1] != 5) && !damageDealt)
            if (behaviors.getAction(executingAction).whiffSound != null)
                AudioManager.Instance.PlaySound(behaviors.getAction(executingAction).whiffSound);

         placeHurtboxes(actionFrameCounter);

        int previousFrame = currentFrameType;
        currentFrameType = frames[actionFrameCounter];
        actionFrameCounter++;

        switch (currentFrameType)
        {
            // if active frame
            case 1:
                // if first active frame in action
                if (previousFrame != 1)
                {
                    otherPlayer.GetComponentInChildren<CollisionScript>().initialFrame = false;
                    if (behaviors.getAction(executingAction).projectileLocation.HasValue)
                    {
                        var projectileX = behaviors.getAction(executingAction).projectileLocation.Value.x;
                        var shotProjectile = Instantiate(projectile, new Vector3(x() + (facingRight ? projectileX : -projectileX), y() + behaviors.getAction(executingAction).projectileLocation.Value.y, 69), Quaternion.identity);
                        shotProjectile.GetComponent<SpriteRenderer>().flipX = facingRight ? false : true;
                        shotProjectile.GetComponent<SpriteRenderer>().flipX = facingRight ? false : true;
                        Projectile shotProjectileScript = shotProjectile.GetComponent<Projectile>();
                        //shotProjectile.transform.position.Set(behaviors.getAction(executingAction).projectileLocation.Value.x, behaviors.getAction(executingAction).projectileLocation.Value.y, 0);
                        shotProjectile.transform.localPosition.Set(behaviors.getAction(executingAction).projectileLocation.Value.x, behaviors.getAction(executingAction).projectileLocation.Value.y, 0);
                        shotProjectileScript.speed = behaviors.getAction(executingAction).projectileSpeed;
                        shotProjectileScript.strength = behaviors.getAction(executingAction).projectileStrength;
                        shotProjectileScript.player = this;
                    }
                }

                if (executingAction < 40 || executingAction == 55 || executingAction == 52)
                    placeHitboxes();

                activeFrameCounter++;
                break;

            // recovery frame
            case 5:
                // if first active frame in action
                if (previousFrame != 1)
                {
                    otherPlayer.GetComponentInChildren<CollisionScript>().initialFrame = false;
                    if (behaviors.getAction(executingAction).projectileLocation.HasValue)
                    {
                        var projectileX = behaviors.getAction(executingAction).projectileLocation.Value.x;
                        var shotProjectile = Instantiate(projectile, new Vector3(x() + (facingRight ? projectileX : -projectileX), y() + behaviors.getAction(executingAction).projectileLocation.Value.y, 69), Quaternion.identity);
                        shotProjectile.GetComponent<SpriteRenderer>().flipX = facingRight ? false : true;
                        Projectile shotProjectileScript = shotProjectile.GetComponent<Projectile>();
                        //shotProjectile.transform.position.Set(behaviors.getAction(executingAction).projectileLocation.Value.x, behaviors.getAction(executingAction).projectileLocation.Value.y, 0);
                        shotProjectile.transform.localPosition.Set(behaviors.getAction(executingAction).projectileLocation.Value.x, behaviors.getAction(executingAction).projectileLocation.Value.y, 0);
                        shotProjectileScript.speed = behaviors.getAction(executingAction).projectileSpeed;
                        shotProjectileScript.strength = behaviors.getAction(executingAction).projectileStrength;
                        shotProjectileScript.player = this;
                    }
                }

                if (executingAction < 40 || executingAction == 55)
                    placeHitboxes();

                activeFrameCounter++;
                break;

            // recovery frame
            case 3:
                // if grounded from a non-infinite action that passed the other player
                if (!airborn && passedPlayerInAction && !passedPlayerInAir && !behaviors.getAction(executingAction).infinite)
                {
                    ActionEnd();
                }

                damageDealt = false;
                alreadyExecutedAttackMove = false;
                break;
            // buffer frames
            case 4:
                damageDealt = false;
                alreadyExecutedAttackMove = false;
                break;
            default:
                damageDealt = false;
                alreadyExecutedAttackMove = false;
                break;
        }
        advancedState = 0;
        attackState = 0;
        if (actionFrameCounter >= behaviors.getAction(executingAction).frames.Length)
        {
            if (behaviors.getAction(executingAction).infinite)
                actionFrameCounter--;
            else
            {
                ActionEnd();
                basicBoxes();
            }
        }
    }

    // buffer jump, advancedState, and attackState
    private void bufferAction()
    {
        foreach (int action in behaviors.getAction(executingAction).actionCancels)
            //buffer jumps
            if (action == 40 && inputManager.currentInput[12] && !airbornActionUsed)
            {
                bufferedMove = 51;
                if (basicState == 7)
                    jumpDirection = 7;
                else if (basicState == 8)
                    jumpDirection = 8;
                else
                    jumpDirection = 9;
            }
        //buffer attacks
        if (advancedState != 0)
            buffer(advancedState + 40);
        else if (attackState != 0)
            buffer(attackState);
    }

    // swap current state with buffered action
    private void swapBuffers()
    {
        if (bufferedMove > 40)
            overrideAction = bufferedMove;
        else if (bufferedMove == 40)
        {
            ActionEnd();
            basicState = jumpDirection;
            if (!jumpSquated)
                overrideAction = 51;
        }
        else
            overrideAction = bufferedMove;
        bufferedMove = 0;

       // if (GetComponent<Animation>())
         //   GetComponent<Animation>().Stop(this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //GetComponent<Animator>().enabled = false;
    }

    private void stateCheck()
    {
        if (executingAction != 0)
        {
            if (overrideAction != 0)
            {
                ActionEnd();
                executingAction = overrideAction;
            }
        }
        else if (overrideAction != 0)
        {
            executingAction = overrideAction;
        }
        else if (advancedState != 0)
        {
            executingAction = advancedState + 40;
        }
        else if (attackState != 0)
        {
            executingAction = attackState;

            if (attackState == 35)
            {
                meterCharge = 0;
                meterStore = 0;
            }
            
        }
        else
        {
            if (basicState >= 7 && !airborn && !jumpSquated)
                executingAction = 51;
            else
                basicMove();
        }

    }

    private int inputConvert(bool[] input)
    {
        if (!airborn)
        {
            if (input[0])
            {
                if (input[2])
                    return (playerSide ? 7 : 9);
                else if (input[3])
                    return (playerSide ? 9 : 7);
                else
                    return 8;
            }
            else if (input[1])
            {
                if (input[2])
                    return (playerSide ? 1 : 3);
                else if (input[3])
                    return (playerSide ? 3 : 1);
                else
                    return 2;
            }
            else if (input[2])
                return (playerSide ? 4 : 6);
            else if (input[3])
                return (playerSide ? 6 : 4);
            else
                return 5;
        }
        else
        {
            return basicState;
        }
    }

    private void basicMove()
    {
        if (!airborn)
        {
            jumpDirection = 0;

            if (basicState == 8)
            {
                hVelocity = 0;
                vVelocity = jumpDirectionSpeed;
            }
            else if (basicState == 7)
            {
                vVelocity = jumpDirectionSpeed;
                hVelocity = backwardSpeed;
            }
            else if (basicState == 9)
            {
                vVelocity = jumpDirectionSpeed;
                hVelocity = forwardSpeed;
            }
            else if (basicState == 5)
            {
                vVelocity = 0;
                hVelocity = 0;
            }
            else if (basicState == 6 || basicState == 4)
            {
                hVelocity = (basicState == 6 ? forwardSpeed : backwardSpeed);
            }

            if (basicState < 4)
            {
                hVelocity = 0;
            }
        }

        if (airborn)
        {
            if ((inputManager.currentInput[12] && inputManager.currentInput[2] && playerSide) || (inputManager.currentInput[12] && inputManager.currentInput[3] && !playerSide))
            {
                jumpDirection = 7;

                if (!airbornActionUsed)
                    basicState = 7;
            }
            else if ((inputManager.currentInput[12] && inputManager.currentInput[3] && playerSide) || (inputManager.currentInput[12] && inputManager.currentInput[2] && !playerSide))
            {
                jumpDirection = 9;

                if (!airbornActionUsed)
                    basicState = 9;
            }
            else if (inputManager.currentInput[12])
            {
                jumpDirection = 8;
                if (!airbornActionUsed)
                    basicState = 8;
            }

            if (!airbornActionUsed && jumpDirection >= 7)
            {
                airbornActionUsed = true;
                checkForFlip(true);

                if (jumpDirection == 7)
                {
                    vVelocity = jumpDirectionSpeed;
                    hVelocity = backwardSpeed;
                }
                else if (jumpDirection == 9)
                {
                    vVelocity = jumpDirectionSpeed;
                    hVelocity = forwardSpeed;
                }
                else
                {
                    vVelocity = jumpDirectionSpeed;
                    hVelocity = 0;
                }
            }
        }

        basicBoxes();

    }

    private void basicBoxes()
    {
        if (previousBasicState == basicState)
        {
            if (basicAnimFrame >= behaviors.getAction(basicState + 100).hurtboxData.GetLength(0))
            {
                //killAllBoxes();
                basicAnimFrame = 0;
            }

            placeHurtboxes(basicAnimFrame);
            basicAnimFrame++;
        }
        else
        {
            killAllBoxes();
            placeHurtboxes(0);
            basicAnimFrame = 1;
            previousBasicState = basicState;
        }
    }

    private void actionMove(int action)
    {
        if (!intialBound && (behaviors.getAction(action).hMovement[actionFrameCounter] > 0 || behaviors.getAction(action).vMovement[actionFrameCounter] > 0))
        {
            intialBound = true;

            hVelocity = behaviors.getAction(action).hMovement[actionFrameCounter];
            vVelocity = behaviors.getAction(action).vMovement[actionFrameCounter];
        }
        else
        {
            hVelocity += behaviors.getAction(action).hMovement[actionFrameCounter];
            vVelocity += behaviors.getAction(action).vMovement[actionFrameCounter];
        }
    }

    private void attackMove(int action)
    {
        if (((playerSide && (x() + behaviors.getAttackMovementHorizontal(action) + pushBuffer < otherPlayer.GetComponent<PlayerScript>().x() + otherPlayer.GetComponent<PlayerScript>().hVelocity)) || (!playerSide && (x() - pushBuffer - behaviors.getAttackMovementHorizontal(action) > otherPlayer.GetComponent<PlayerScript>().x() + otherPlayer.GetComponent<PlayerScript>().hVelocity))))
            hKnockback += (playerSide ? behaviors.getAttackMovementHorizontal(action) : -behaviors.getAttackMovementHorizontal(action));
        vKnockback += behaviors.getAttackMovementVertical(action);
        alreadyExecutedAttackMove = true;
    }

    private void advancedMove()
    {
        int advancedAction = executingAction - 40;
        //Debug.LogFormat("index {0} in {1} length array", advancedAction, behaviors.onAdvancedActionCallbacks.Length);
        if (behaviors.onAdvancedActionCallbacks[advancedAction] != null)
        {
            behaviors.onAdvancedActionCallbacks[advancedAction].Invoke(this);
            //Debug.Log("advanced move: " + (behaviors.onAdvancedActionCallbacks[advancedAction].Method.Name.ToString()));
        }
    }

    public void throwThatMfOtherPlayer()
    {
        hVelocity = 0;
        switch (throwType)
        {
            case 0:
                otherPlayer.GetComponent<PlayerScript>().hKnockback = -6;
                facingRight = false;
                break;
            case 1:
                otherPlayer.GetComponent<PlayerScript>().hKnockback = 6;
                facingRight = true;
                break;
        }
    }

    public void checkDashEnd()
    {
        if ((!inputManager.currentInput[2] && !dashingForwards) || (!inputManager.currentInput[3] && dashingForwards))
        {
            hVelocity = 0;
            dashTimer = 0;
            ActionEnd();
        }
    }

    public void checkBlockEnd()
    {
        blockTimer--;
        if(blockTimer == 0)
        {
            ActionEnd();
        }
    }

    public void UpdateEnd()
    {
        GetComponent<SpriteRenderer>().flipX = facingRight ? false : true;

        knockbackDecrease();

        movePlayer();

        updateAnimation();

        cleanup();
    }

    public void knockbackDecrease()
    {
        if (hKnockback != 0)
        {
            if (!airborn)
            {
                hKnockback *= .75f;
                if (Mathf.Abs(hKnockback) < 0.005f)
                    hKnockback = 0;
            }
            else
            {
                hKnockback *= .75f;
                if (Mathf.Abs(hKnockback) < 0.005f)
                    hKnockback = 0;
            }
        }
    }

    private void movePlayer()
    {
        if (executingAction != 43 && executingAction != 44)
            vVelocity += gravity;

        if (vVelocity < -1)
        {
            vVelocity = -1;
        }

        moveX((facingRight ? hVelocity : -hVelocity) + (playerSide ? -hPush :hPush) + hKnockback);
        moveY(vVelocity + vKnockback);

        if (x() < -MAP_WITDH + pushBuffer + wallBuffer && !playerSide && (y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().y() && otherPlayer.GetComponent<PlayerScript>().y() <= hitbox.bounds.size.y + y()))
        {
            setX(-MAP_WITDH + pushBuffer + wallBuffer);
        }
        if (x() < -MAP_WITDH)
        {
            setX(-MAP_WITDH);
        }
        else if (x() > MAP_WITDH - pushBuffer - wallBuffer && playerSide && (y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().y() && otherPlayer.GetComponent<PlayerScript>().y() <= hitbox.bounds.size.y + y()))
        {
            setX(MAP_WITDH - pushBuffer - wallBuffer);
        }
        else if (x() > MAP_WITDH)
        {
            setX(MAP_WITDH);
        }

        setX(Mathf.Clamp(x(), cameraLeft.position.x - (!playerSide && ((y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().y() && otherPlayer.GetComponent<PlayerScript>().y() <= hitbox.bounds.size.y + y()))? 5 - pushBuffer : 5 ), cameraRight.position.x + 
            (playerSide && ((y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().y() && otherPlayer.GetComponent<PlayerScript>().y() <= hitbox.bounds.size.y + y())) ? 5 - pushBuffer : 5 )));

        if (y() <= FLOOR_HEIGHT) //ground snappity
        {
            if (airborn)
            {
                airborn = false;
                jumpSquated = false;
                vKnockback = 0;
                airbornActionUsed = false;

                if (executingAction == 54)
                {
                    ActionEnd();
                    if (health <= 0)
                        executingAction = 59;
                    else
                        executingAction = 53;
                }
                else if (executingAction > 0)
                    ActionEnd();
            }
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }
        else
        {
            airborn = true;
        }
    }

    public void updateAnimation()
    {
        PlayerScript other = otherPlayer.GetComponent<PlayerScript>();
        if (executingAction != 0)
        {
            if (executingAction == 45)
            {
                if (basicState <= 3)
                    animInt(Animator.StringToHash("StunType"), 3);
                else if (basicState >= 7)
                    animInt(Animator.StringToHash("StunType"), 4);
                else if (other.behaviors.getAction(other.executingAction).block == 1)
                    animInt(Animator.StringToHash("StunType"), 0);
                else if (other.behaviors.getAction(other.executingAction).block == 3)
                    animInt(Animator.StringToHash("StunType"), 2);
                else
                    animInt(Animator.StringToHash("StunType"), 1);
            }


            animInt(Animator.StringToHash("Action"), behaviors.getAnimAction(behaviors.getAction(executingAction)));
            animInt(Animator.StringToHash("Basic"), 0);
        }
        else
        {
            animInt(Animator.StringToHash("Basic"), basicState);
            animInt(Animator.StringToHash("Action"), 0);
        }
    }

    private void placeHitboxes()
    {
        Action.rect[,] hitboxData = behaviors.getAction(executingAction).hitboxData;

        for (int i = 0; i < hitboxData.GetLength(1); i++)
        {
            Action.rect hitbox = hitboxData[activeFrameCounter, i];

            if (!livingHitboxesIds.Contains(hitbox.id) && hitbox.id != -1)
            {
                livingHitboxesIds.Add(hitbox.id);
                livingHitboxesLifespans.Add(hitbox.timeActive);
                addBoxCollider2D(hitbox.id.ToString(), new Vector2(hitbox.width, hitbox.height), new Vector2((facingRight ? hitbox.x : -hitbox.x), hitbox.y), true);
            }
        }
    }

    private void placeHurtboxes(int frame)
    {
        Action.rect[,] hurtboxData;

        if (executingAction != 0)
        {
            hurtboxData = behaviors.getAction(executingAction).hurtboxData;
        }
        else
        {
            hurtboxData = behaviors.getAction(basicState + 100).hurtboxData;
        }
        for (int i = 0; i < hurtboxData.GetLength(1); i++)
        {
            Action.rect hurtbox = hurtboxData[frame, i];

            if (!livingHurtboxesIds.Contains(hurtbox.id) && hurtbox.id != -1)
            {
                livingHurtboxesIds.Add(hurtbox.id);
                livingHurtboxesLifespans.Add(hurtbox.timeActive);
                addBoxCollider2D((hurtbox.id + 100).ToString(), new Vector2(hurtbox.width, hurtbox.height), new Vector2((facingRight ? hurtbox.x : -hurtbox.x), hurtbox.y), false);
            }
            else if (livingHurtboxesIds.Contains(hurtbox.id))
            {
                /*
                Debug.Log(livingHurtboxesIds.Count);
                Debug.Log("repeat call");
                */
}
        }
    }

    public void decreaseHitboxLifespan()
    {
        for (int j = livingHitboxesLifespans.Count; j > 0; j--)
            if (livingHitboxesLifespans[j - 1] > 0)
            {
                livingHitboxesLifespans[j - 1]--;
            }
            else
            {
                removeBoxCollider2D(livingHitboxesIds[j - 1].ToString(), hitbox);
                livingHitboxesIds.RemoveAt(j - 1);
                livingHitboxesLifespans.RemoveAt(j - 1);
            }
    }

    public void decreaseHurtboxLifespan()
    {
        for (int j = livingHurtboxesLifespans.Count; j > 0; j--)
        {
            if (livingHurtboxesLifespans[j - 1] > 1)
            {
                livingHurtboxesLifespans[j - 1]--;
            }
            else
            {
                removeBoxCollider2D((livingHurtboxesIds[j - 1] + 100).ToString(), false);
                livingHurtboxesIds.RemoveAt(j - 1);
                livingHurtboxesLifespans.RemoveAt(j - 1);
            }
        }
    }

    private void addBoxCollider2D(String name, Vector2 size, Vector2 offset, bool boxType)
    {
        GameObject childbox = new GameObject(name);

        childbox.transform.position = transform.position;
        childbox.transform.SetParent(boxType ? transform.GetChild(1) : transform.GetChild(2));

        childbox.tag = (boxType ? "hitbox" + playerID : "hurtbox" + playerID);

        /*if (!boxType)
            childbox.AddComponent<HurtboxScript>();
        else
            childbox.AddComponent<RealHitboxScript>();
        */
        childbox.AddComponent<BoxCollider2D>();
        childbox.GetComponent<BoxCollider2D>().size = size;
        childbox.GetComponent<BoxCollider2D>().offset = offset;
        if (!boxType)
            childbox.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void removeBoxCollider2D(String name, bool boxType)
    {
        //boxType true = hitbox, false = hurtbox
        foreach (Transform child in boxType ? transform.GetChild(1) : transform.GetChild(2))
        {
            if (child.gameObject.name.Equals(name))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void killAllHurtboxes()
    {
        livingHurtboxesIds.Clear();
        livingHurtboxesLifespans.Clear();

        foreach (Transform child in transform.GetChild(2))
        {
            if (child.gameObject.tag.Equals("hurtbox1") || child.gameObject.tag.Equals("hurtbox2"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void killAllHitboxes()
    {
        livingHitboxesIds.Clear();
        livingHitboxesLifespans.Clear();

        foreach (Transform child in transform.GetChild(1))
        {
            if (child.gameObject.tag.Equals("hitbox1") || child.gameObject.tag.Equals("hitbox2"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void killAllBoxes()
    {
        killAllHitboxes();
        killAllHurtboxes();
    }

    public void onPush(float otherHVel)
    {
        if (executingAction == 53)
            otherPlayer.GetComponent<PlayerScript>().hPush += facingRight ? (hVelocity + otherHVel) / 2 : (otherHVel + hVelocity) / 2;
        else
            hPush += facingRight ? (hVelocity + otherHVel) / 2 : (otherHVel + hVelocity) / 2;
    }

    public void ActionEnd()
    {
        if (executingAction != 51)
            jumpSquated = false;

        executingAction = 0;
        currentFrameType = 0;
        actionFrameCounter = 0;
        activeFrameCounter = 0;
        basicAnimFrame = 0;
        previousBasicState = 0;
        damageDealt = false;
        shouldWallbounce = false;
        shouldGroundbounce = false;
        intialBound = false;
        killAllBoxes();
    }

    public void overrideDamage(int dmg, float knockback, int angle, int blck, float p1, int tier, int newAction)
    {
        int action = otherPlayer.GetComponent<PlayerScript>().executingAction;
        otherPlayer.GetComponent<PlayerScript>().executingAction = newAction;
        damage(dmg, knockback, angle, blck, p1, tier);
        otherPlayer.GetComponent<PlayerScript>().executingAction = action;
    }

    public void damage(int damage, float knockback, int angle, int blck, float p1, int tier)
    {
        ActionEnd();

        hKnockback = knockback * Mathf.Cos(((float)angle / 180f) * Mathf.PI) * (playerSide ? -1 : 1);
        vKnockback = knockback * Mathf.Sin(((float)angle / 180f) * Mathf.PI);
        
        if (comboTimer > 0)
        {
            p2Scale *= otherPlayer.GetComponent<PlayerScript>().level(7);
            damage = (int)(p1Scale * p2Scale * damage * .6);

            if (damage <= 50)
            {
                damage = 50;
            }
        }

        if (shouldBlock && ((blck == 3 && basicState > 3) || (blck == 1 && basicState < 4 && basicState < 7) || (blck == 2) || (basicState > 6)))
        {
            hKnockback /= 4;
            vKnockback = 0;
            health -= damage / 10;
            healthStore += damage / 10;
            meterStore += (int)(damage * .4f);
            if (!otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).super)
                otherPlayer.GetComponent<PlayerScript>().meterStore += (int)(damage * .2f);
            block();
            AudioManager.Instance.PlaySound(AudioManager.Sound.BLOCK);
        }
        else
        {
            if (otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).hitSound != null)
            {
                AudioManager.Instance.PlaySound(otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).hitSound);
            }
            health -= damage;
            healthStore += damage;
            meterStore += (int)(damage * .8f);
            if (!otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).super)
                otherPlayer.GetComponent<PlayerScript>().meterStore += (int)(damage * 1.4f);
            stun();
        }

        if (vKnockback > 0)
        {
            airborn = true;
        }

        vVelocity = 0;
    }

    public void stun()
    {
        ActionEnd();
        firstStun = true;

        if (otherPlayer.GetComponent<PlayerScript>().currentFrameType == 5 && otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).knockdown > 0 && otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).knockdown <= 2)
        {
            executingAction = 54;
        }
        else if (otherPlayer.GetComponent<PlayerScript>().currentFrameType == 5 && otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).knockdown <= 4)
        {
            executingAction = 45;
            shouldGroundbounce = true;
            stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().level(1);

        }
        else if (otherPlayer.GetComponent<PlayerScript>().currentFrameType == 5 && otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).knockdown >= 5)
        {
            executingAction = 45;
            shouldWallbounce = true;
            stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().level(1);
        }
        else
        {
            executingAction = 45;
            stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().level(1);
        }

        otherPlayer.GetComponent<PlayerScript>().comboCounter++;

        comboCounterText.value = otherPlayer.GetComponent<PlayerScript>().comboCounter;

        if (comboTimer == 0)
            p1Scale = otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(otherPlayer.GetComponent<PlayerScript>().executingAction).p1scaling;

        comboTimer = 1;
    }

    public void block()
    {
        ActionEnd();

        if (basicState >= 7)
            executingAction = 48;
        else if (basicState <= 3)
            executingAction = 47;
        else
            executingAction = 46;

        blockTimer = (int)otherPlayer.GetComponent<PlayerScript>().level(6);
    }

    public float level(int wanted)
    {
        return levelScaling[behaviors.getAction(executingAction).level, wanted];
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
        transform.Translate(amm, 0, 0);
    }

    public void moveY(float amm)
    {
        transform.Translate(0, amm, 0);
    }

    public void setY(float amm)
    {
        moveY(-transform.position.y + amm);
    }

    public void setX(float amm)
    {
        moveX(-transform.position.x + amm);
    }

    public float y()
    {
        return this.transform.position.y;
    }

    public float x()
    {
        return this.transform.position.x;
    }

    public Vector2 position()
    {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }
}