using System.Collections.Generic;
using Character;
using Character.Dark_Konky;
using Character.Greyshirt;
using Character.Konky;
using Character.Shrulk;
using Core;
using Misc;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerScript : MonoBehaviour
    {

        #region VARIABLES

        private float _floorHeight = 0;
        private float _baseGravity = -0.05f;
        [FormerlySerializedAs("MAP_WITDH")] public float mapWitdh = 46f;

        private readonly float[,] _levelScaling = 
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
        [FormerlySerializedAs("playerID")] public int playerId;               // the players id, either 1 or 0
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
        public int cancelLimit;

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
        public RuntimeAnimatorController dkAnimationController;
        public RuntimeAnimatorController shrulkAnimationController;
        public Behaviors behaviors;
        public BoxCollider2D hitbox;
        public GameObject otherPlayer;
        [FormerlySerializedAs("JoyScript")] public JoyScript joyScript;
        public InputManager inputManager;
        public Transform cameraLeft, cameraRight;
        public IntVariable p1ComboCounterText, p2ComboCounterText;
        private IntVariable _comboCounterText;
        public GameObject projectile;

        public List<float> livingHitboxesIds;        // the ids of all living hitboxes
        public List<float> livingHitboxesLifespans;  // the lifespans of all living hitboxes
        public List<float> livingHurtboxesIds;       // the ids of all living hurtboxes
        public List<float> livingHurtboxesLifespans; // the lifespans of all living hurtboxes

        //temp variables
        AIController _testAi;
        bool _ai = false;

        #endregion

        /*
     * AND IF YOU WIN YOU CAN FUCK GREYSHIRT IN THE SPECIAL FEATURES
     */

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawWireCube(transform.position);
        }

        // initialize variables based on current players character
        private void Start()
        {

            tag = playerId.ToString();
            transform.GetChild(0).tag = "collisionHitbox" + playerId;
            hitbox = GetComponentInChildren<BoxCollider2D>();

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMusic((AudioManager.Music) Random.Range(2, 8));
        

            if (CompareTag("1"))
            {
                _comboCounterText = p1ComboCounterText;
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
                    case 2:
                        behaviors = new DkBehaviours();
                        animator.runtimeAnimatorController = dkAnimationController;
                        break;
                    case 3:
                        behaviors = new ShrulkBehaviours();
                        animator.runtimeAnimatorController = shrulkAnimationController;
                        break;
                }
            }
            else if (CompareTag("2"))
            {
                _comboCounterText = p2ComboCounterText;
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
                    case 2:
                        behaviors = new DkBehaviours();
                        animator.runtimeAnimatorController = dkAnimationController;
                        break;
                    case 3:
                        behaviors = new ShrulkBehaviours();
                        animator.runtimeAnimatorController = shrulkAnimationController;
                        break;
                }
            }

            vVelocity = 0;
            hVelocity = 0;
            behaviors.SetStats();
            forwardSpeed = behaviors.GetForwardSpeed();
            backwardSpeed = behaviors.GetBackwardSpeed();
            jumpDirectionSpeed = behaviors.GetJumpDirectionSpeed();
            maxHealth = behaviors.GetMaxHealth();
            maxMeter = behaviors.GetMaxMeter();
            gravity = behaviors.GetGravity();
            health = maxHealth;
            pushBuffer = 4;
            wallBuffer = 3.5f;

            livingHitboxesIds = new List<float>();
            livingHitboxesLifespans = new List<float>();

            _testAi = new KonkyAI();
        }

        // called 60 times per second
        private void Update()
        {
            //if (Input.GetKeyUp(KeyCode.Y)) { ai = !ai; }

            // get currently held keys or pressed buttons
            if (InputManager.IsInputEnabled)
                inputManager.PollInput(0, playerId);
        
            //testAI.observe(0, otherPlayer.GetComponent<PlayerScript>().position(), otherPlayer.GetComponent<PlayerScript>().executingAction, position(), facingRight);
            if (!hitStopped)
            {
                //set basic and attack states    
                SetStates();

                PreAction();

                // check whether to continue or end action
                StateCheck();

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
                        ActionMove(executingAction);

                        if (executingAction < 40 && damageDealt && !alreadyExecutedAttackMove)
                            AttackMove(executingAction);
                    }                
                    else if (executingAction >= 40)
                        AdvancedMove();

                    if (executingAction != 0)
                    {
                        // progress the current action
                        IncrementFrame(behaviors.GetAction(executingAction).Frames);
                    }
                }
            }
        }

        private void PreAction()
        {
            switch (currentFrameType)
            {
                case 3:
                    CheckForFlip(false);

                    BufferAction();

                    // cancel into buffered move
                    if (bufferedMove != 0)
                        SwapBuffers();
                    break;
                case 4:
                    BufferAction();
                    break;

                case 1:
                    BufferAction();
                    break;              
                case 5:
                    BufferAction();
                    break;
            }

            if (executingAction == 0)
                CheckForFlip(false);
        }

        private void CheckForFlip( bool specialCase)
        {
            if (playerSide == facingRight || executingAction == 41 || executingAction == 52) return;
            if (!airborn || specialCase)
                facingRight = !facingRight;
            if (airborn || specialCase) return;
            overrideAction = basicState < 4 ? 50 : 49;
        }

        private void Cleanup()
        {
            hPush = 0;
            vKnockback *= .5f;
            overrideAction = 0;
            if (comboTimer > 0 && executingAction != 45 && executingAction != 56 && executingAction != 54)
                comboTimer--;
            if (comboTimer == 0)
            {
                if (otherPlayer.GetComponent<PlayerScript>().healthStore > 0)
                {
                    otherPlayer.GetComponent<PlayerScript>().healthStore -= otherPlayer.GetComponent<PlayerScript>().healthStore >= 25 ? 25 : otherPlayer.GetComponent<PlayerScript>().healthStore;
                }

                otherPlayer.GetComponent<PlayerScript>().comboCounter = 0;
                p1Scale = 0;
                p2Scale = 1;
                if (health <= 0)
                {
                    executingAction = airborn ? 54 : 59;
                }
            }
            if (airborn && basicState < 7)
                basicState = 8;
            if (!airborn && executingAction == 54)
            {
                ActionEnd();
                executingAction = health <= 0 ? 59 : 53;
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
                meterCharge += meterStore >= 25? 25 : meterStore;
                meterStore -= meterStore >= 25 ? 25 : meterStore;
            }
        }

        private void SetStates()
        {
            // updates basic state accordingly 
            if (playerId == 1 && _ai)
            {
                basicState = InputConvert(_testAi.getInput());
                SetAttackInput(_testAi.getInput());

                // if jumping reset attackState
                if (attackState % 10 >= 7 && !airborn)
                    attackState = 0;
                SetAdvancedInput(_testAi.getInput());
                SetSpecialInput(_testAi.getInput());
            }
            else
            {
                basicState = InputConvert(inputManager.CurrentInput);
                SetAttackInput(inputManager.CurrentInput);

                // if jumping reset attackState
                if (attackState % 10 >= 7 && !airborn)
                    attackState = 0;
                SetAdvancedInput(inputManager.CurrentInput);
                SetSpecialInput(inputManager.CurrentInput);
            }
        }

        // update attackState from current input
        private void SetAttackInput(IReadOnlyList<bool> input)
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

        private void SetSpecialInput(IReadOnlyList<bool> input)
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
                                if (behaviors.GetAction(31).AirOk)
                                    attackState = 31;
                            }
                            else if (input[3])
                            {
                                if (behaviors.GetAction(33).AirOk)
                                    attackState = 33;
                            }
                        }
                        else
                        {
                            if (input[2])
                            {
                                if (behaviors.GetAction(34).AirOk)
                                    attackState = 34;
                            }
                            else if (input[3])
                            {
                                if (behaviors.GetAction(35).AirOk)
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
                                if (behaviors.GetAction(31).AirOk)
                                    attackState = 31;
                            }
                            else if (input[2])
                            {
                                if (behaviors.GetAction(33).AirOk)
                                    attackState = 33;
                            }
                        }
                        else
                        {
                            if (input[3])
                            {
                                if (behaviors.GetAction(34).AirOk)
                                    attackState = 34;
                            }
                            else if (input[2])
                            {
                                if (behaviors.GetAction(36).AirOk)
                                    attackState = 36;
                            }
                        }
                    }
                }
            }
        }

        // updates advanced state and deals with advanced action stopping
        private void SetAdvancedInput(IReadOnlyList<bool> input)
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
                                CheckForFlip(true);
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
                                CheckForFlip(true);
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
                                CheckForFlip(true);
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
                                CheckForFlip(true);
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
                    dashingForwards = !input[8];
                }

                if (input[2] && facingRight && executingAction == 0)
                    shouldBlock = true;
                else if (input[3] && !facingRight && executingAction == 0)
                    shouldBlock = true;
                else if (executingAction >= 46 && executingAction <= 48)
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
        private void Buffer(int bufferedInput)
        {
            foreach (var action in behaviors.GetAction(executingAction).ActionCancels)
                if (action == bufferedInput)
                    bufferedMove = bufferedInput;
        }

        // executes a frame of the current action
        private void IncrementFrame(IReadOnlyList<int> frames)
        {
            if (actionFrameCounter == 0)
                KillAllBoxes();

            if ((currentFrameType == 1 || currentFrameType == 5) && frames[actionFrameCounter + 1] != 1 && frames[actionFrameCounter + 1] != 5 && !damageDealt)
                if (behaviors.GetAction(executingAction).WhiffSound != null)
                    AudioManager.Instance.PlaySound(behaviors.GetAction(executingAction).WhiffSound);

            PlaceHurtboxes(actionFrameCounter);

            var previousFrame = currentFrameType;
            currentFrameType = frames[actionFrameCounter];
            actionFrameCounter++;

            switch (currentFrameType)
            {
                // if active frame
                case 1:
                    // if first active frame in action
                    if (previousFrame != 1)
                    {
                        otherPlayer.GetComponentInChildren<CollisionScript>().InitialFrame = false;
                        if (behaviors.GetAction(executingAction).ProjectileLocation.HasValue)
                        {
                            var projectileLocation = behaviors.GetAction(executingAction).ProjectileLocation;
                            if (projectileLocation != null)
                            {
                                var projectileX = projectileLocation.Value.x;
                                var shotProjectile = Instantiate(projectile, new Vector3(X() + (facingRight ? projectileX : -projectileX), Y() + projectileLocation.Value.y, 69), Quaternion.identity);
                                shotProjectile.GetComponent<SpriteRenderer>().flipX = !facingRight;
                                shotProjectile.GetComponent<SpriteRenderer>().flipX = !facingRight;
                                var shotProjectileScript = shotProjectile.GetComponent<Projectile>();
                                //shotProjectile.transform.position.Set(behaviors.getAction(executingAction).projectileLocation.Value.x, behaviors.getAction(executingAction).projectileLocation.Value.y, 0);
                                shotProjectile.transform.localPosition.Set(projectileLocation.Value.x, projectileLocation.Value.y, 0);
                                shotProjectileScript.Speed = behaviors.GetAction(executingAction).ProjectileSpeed;
                                shotProjectileScript.Strength = behaviors.GetAction(executingAction).ProjectileStrength;
                                shotProjectileScript.Player = this;
                            }
                        }
                    }

                    if (executingAction < 40 || executingAction == 55 || executingAction == 52)
                        PlaceHitboxes();

                    activeFrameCounter++;
                    break;

                // recovery frame
                case 5:
                    // if first active frame in action
                    if (previousFrame != 1)
                    {
                        otherPlayer.GetComponentInChildren<CollisionScript>().InitialFrame = false;
                        if (behaviors.GetAction(executingAction).ProjectileLocation.HasValue)
                        {
                            var projectileLocation = behaviors.GetAction(executingAction).ProjectileLocation;
                            if (projectileLocation != null)
                            {
                                var projectileX = projectileLocation.Value.x;
                                var shotProjectile = Instantiate(projectile, new Vector3(X() + (facingRight ? projectileX : -projectileX), Y() + projectileLocation.Value.y, 69), Quaternion.identity);
                                shotProjectile.GetComponent<SpriteRenderer>().flipX = !facingRight;
                                var shotProjectileScript = shotProjectile.GetComponent<Projectile>();
                                //shotProjectile.transform.position.Set(behaviors.getAction(executingAction).projectileLocation.Value.x, behaviors.getAction(executingAction).projectileLocation.Value.y, 0);
                                shotProjectile.transform.localPosition.Set(projectileLocation.Value.x, projectileLocation.Value.y, 0);
                                shotProjectileScript.Speed = behaviors.GetAction(executingAction).ProjectileSpeed;
                                shotProjectileScript.Strength = behaviors.GetAction(executingAction).ProjectileStrength;
                                shotProjectileScript.Player = this;
                            }
                        }
                    }

                    if (executingAction < 40 || executingAction == 55 || executingAction == 52)
                        PlaceHitboxes();

                    activeFrameCounter++;
                    break;

                // recovery frame
                case 3:
                    // if grounded from a non-infinite action that passed the other player
                    if (!airborn && passedPlayerInAction && !passedPlayerInAir && !behaviors.GetAction(executingAction).Infinite)
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
            if (actionFrameCounter >= behaviors.GetAction(executingAction).Frames.Length)
            {
                if (behaviors.GetAction(executingAction).Infinite)
                    actionFrameCounter--;
                else
                {
                    ActionEnd();
                    BasicBoxes();
                }
            }
        }

        // buffer jump, advancedState, and attackState
        private void BufferAction()
        {
            foreach (var action in behaviors.GetAction(executingAction).ActionCancels)
                //buffer jumps
                if (action == 40 && inputManager.CurrentInput[12] && !airbornActionUsed)
                {
                    bufferedMove = 51;
                    switch (basicState)
                    {
                        case 7:
                            jumpDirection = 7;
                            break;
                        case 8:
                            jumpDirection = 8;
                            break;
                        default:
                            jumpDirection = 9;
                            break;
                    }
                }
            //buffer attacks
            if (advancedState != 0)
                Buffer(advancedState + 40);
            else if (attackState != 0)
                Buffer(attackState);
        }

        // swap current state with buffered action
        private void SwapBuffers()
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

        private void StateCheck()
        {
            if (executingAction != 0)
            {
                if (overrideAction != 0)
                {
                    ActionEnd();
                    executingAction = overrideAction;

                    if (overrideAction == 35)
                    {
                        meterCharge = 0;
                        meterStore = 0;
                    }
                }
            }
            else if (overrideAction != 0)
            {
                executingAction = overrideAction;

                if (overrideAction == 35)
                {
                    meterCharge = 0;
                    meterStore = 0;
                }
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
                    BasicMove();
            }

        }

        private int InputConvert(IReadOnlyList<bool> input)
        {
            if (!airborn)
            {
                if (input[0])
                {
                    if (input[2])
                        return playerSide ? 7 : 9;
                    if (input[3])
                        return playerSide ? 9 : 7;
                    return 8;
                }

                if (input[1])
                {
                    if (input[2])
                        return playerSide ? 1 : 3;
                    if (input[3])
                        return playerSide ? 3 : 1;
                    return 2;
                }

                if (input[2])
                    return playerSide ? 4 : 6;
                if (input[3])
                    return playerSide ? 6 : 4;
                return 5;
            }

            return basicState;
        }

        private void BasicMove()
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
                    hVelocity = basicState == 6 ? forwardSpeed : backwardSpeed;
                }

                if (basicState < 4)
                {
                    hVelocity = 0;
                }
            }

            if (airborn)
            {
                if (inputManager.CurrentInput[12] && inputManager.CurrentInput[2] && playerSide || inputManager.CurrentInput[12] && inputManager.CurrentInput[3] && !playerSide)
                {
                    jumpDirection = 7;

                    if (!airbornActionUsed)
                        basicState = 7;
                }
                else if (inputManager.CurrentInput[12] && inputManager.CurrentInput[3] && playerSide || inputManager.CurrentInput[12] && inputManager.CurrentInput[2] && !playerSide)
                {
                    jumpDirection = 9;

                    if (!airbornActionUsed)
                        basicState = 9;
                }
                else if (inputManager.CurrentInput[12])
                {
                    jumpDirection = 8;
                    if (!airbornActionUsed)
                        basicState = 8;
                }

                if (!airbornActionUsed && jumpDirection >= 7)
                {
                    airbornActionUsed = true;
                    CheckForFlip(true);

                    switch (jumpDirection)
                    {
                        case 7:
                            vVelocity = jumpDirectionSpeed;
                            hVelocity = backwardSpeed;
                            break;
                        case 9:
                            vVelocity = jumpDirectionSpeed;
                            hVelocity = forwardSpeed;
                            break;
                        default:
                            vVelocity = jumpDirectionSpeed;
                            hVelocity = 0;
                            break;
                    }
                }
            }

            BasicBoxes();

        }

        private void BasicBoxes()
        {
            if (previousBasicState == basicState)
            {
                if (basicAnimFrame >= behaviors.GetAction(basicState + 100).HurtboxData.GetLength(0))
                {
                    //killAllBoxes();
                    basicAnimFrame = 0;
                }

                PlaceHurtboxes(basicAnimFrame);
                basicAnimFrame++;
            }
            else
            {
                KillAllBoxes();
                PlaceHurtboxes(0);
                basicAnimFrame = 1;
                previousBasicState = basicState;
            }
        }

        private void ActionMove(int action)
        {
            if (!intialBound && (behaviors.GetAction(action).HMovement[actionFrameCounter] > 0 || behaviors.GetAction(action).VMovement[actionFrameCounter] > 0))
            {
                intialBound = true;

                hVelocity = behaviors.GetAction(action).HMovement[actionFrameCounter];
                vVelocity = behaviors.GetAction(action).VMovement[actionFrameCounter];
            }
            else
            {
                hVelocity += behaviors.GetAction(action).HMovement[actionFrameCounter];
                vVelocity += behaviors.GetAction(action).VMovement[actionFrameCounter];
            }
        }

        private void AttackMove(int action)
        {
            if (playerSide && X() + behaviors.GetAttackMovementHorizontal(action) + pushBuffer < otherPlayer.GetComponent<PlayerScript>().X() + otherPlayer.GetComponent<PlayerScript>().hVelocity || !playerSide && X() - pushBuffer - behaviors.GetAttackMovementHorizontal(action) > otherPlayer.GetComponent<PlayerScript>().X() + otherPlayer.GetComponent<PlayerScript>().hVelocity)
                hKnockback += playerSide ? behaviors.GetAttackMovementHorizontal(action) : -behaviors.GetAttackMovementHorizontal(action);
            vKnockback += behaviors.GetAttackMovementVertical(action);
            alreadyExecutedAttackMove = true;
        }

        private void AdvancedMove()
        {
            var advancedAction = executingAction - 40;
            //Debug.LogFormat("index {0} in {1} length array", advancedAction, behaviors.onAdvancedActionCallbacks.Length);
            if (behaviors.OnAdvancedActionCallbacks[advancedAction] != null)
            {
                behaviors.OnAdvancedActionCallbacks[advancedAction].Invoke(this);
                //Debug.Log("advanced move: " + (behaviors.onAdvancedActionCallbacks[advancedAction].Method.Name.ToString()));
            }
        }

        public void ThrowThatMfOtherPlayer()
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

        public void CheckDashEnd()
        {
            if (!inputManager.CurrentInput[2] && !dashingForwards || !inputManager.CurrentInput[3] && dashingForwards)
            {
                hVelocity = 0;
                dashTimer = 0;
                ActionEnd();
            }
        }

        public void CheckBlockEnd()
        {
            blockTimer--;
            if(blockTimer == 0)
            {
                ActionEnd();
            }
        }

        public void UpdateEnd()
        {
            GetComponent<SpriteRenderer>().flipX = !facingRight;

            KnockbackDecrease();

            MovePlayer();

            UpdateAnimation();

            Cleanup();
        }

        private void KnockbackDecrease()
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

        private void MovePlayer()
        {
            if (executingAction != 43 && executingAction != 44 && executingAction != 48)
                vVelocity += gravity;

            if (vVelocity < -1)
            {
                vVelocity = -1;
            }

            MoveX((facingRight ? hVelocity : -hVelocity) + (playerSide ? -hPush :hPush) + hKnockback);
            MoveY(vVelocity + vKnockback);

            if (X() < -mapWitdh + pushBuffer + wallBuffer && !playerSide && Y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().Y() && otherPlayer.GetComponent<PlayerScript>().Y() <= hitbox.bounds.size.y + Y())
            {
                SetX(-mapWitdh + pushBuffer + wallBuffer);
            }
            if (X() < -mapWitdh)
            {
                SetX(-mapWitdh);
            }
            else if (X() > mapWitdh - pushBuffer - wallBuffer && playerSide && Y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().Y() && otherPlayer.GetComponent<PlayerScript>().Y() <= hitbox.bounds.size.y + Y())
            {
                SetX(mapWitdh - pushBuffer - wallBuffer);
            }
            else if (X() > mapWitdh)
            {
                SetX(mapWitdh);
            }

            SetX(Mathf.Clamp(X(), cameraLeft.position.x - (!playerSide && Y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().Y() && otherPlayer.GetComponent<PlayerScript>().Y() <= hitbox.bounds.size.y + Y()? 5 - pushBuffer : 5 ), cameraRight.position.x + 
                                                                                                                                                                                                                                                                                                   (playerSide && Y() <= otherPlayer.GetComponent<PlayerScript>().hitbox.bounds.size.y + otherPlayer.GetComponent<PlayerScript>().Y() && otherPlayer.GetComponent<PlayerScript>().Y() <= hitbox.bounds.size.y + Y() ? 5 - pushBuffer : 5 )));

            if (Y() <= _floorHeight) //ground snappity
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
                        executingAction = health <= 0 ? 59 : 53;
                    }
                    else if (executingAction > 0)
                        ActionEnd();
                }
                vVelocity = 0;
                SetY(_floorHeight);
            }
            else
            {
                airborn = true;
            }
        }

        public void UpdateAnimation()
        {
            var other = otherPlayer.GetComponent<PlayerScript>();
            if (executingAction != 0)
            {
                if (executingAction == 45)
                {
                    if (basicState <= 3)
                        AnimInt(Animator.StringToHash("StunType"), 3);
                    else if (basicState >= 7)
                        AnimInt(Animator.StringToHash("StunType"), 4);
                    else if (other.behaviors.GetAction(other.executingAction).Block == 1)
                        AnimInt(Animator.StringToHash("StunType"), 0);
                    else if (other.behaviors.GetAction(other.executingAction).Block == 3)
                        AnimInt(Animator.StringToHash("StunType"), 2);
                    else
                        AnimInt(Animator.StringToHash("StunType"), 1);
                }


                AnimInt(Animator.StringToHash("Action"), behaviors.GetAnimAction(behaviors.GetAction(executingAction)));
                AnimInt(Animator.StringToHash("Basic"), 0);
            }
            else
            {
                AnimInt(Animator.StringToHash("Basic"), basicState);
                AnimInt(Animator.StringToHash("Action"), 0);
            }
        }

        private void PlaceHitboxes()
        {
            var hitboxData = behaviors.GetAction(executingAction).HitboxData;

            for (var i = 0; i < hitboxData.GetLength(1); i++)
            {
                var hitbox = hitboxData[activeFrameCounter, i];

                if (!livingHitboxesIds.Contains(hitbox.Id) && hitbox.Id != -1)
                {
                    livingHitboxesIds.Add(hitbox.Id);
                    livingHitboxesLifespans.Add(hitbox.TimeActive);
                    AddBoxCollider2D(hitbox.Id.ToString(), new Vector2(hitbox.Width, hitbox.Height), new Vector2(facingRight ? hitbox.X : -hitbox.X, hitbox.Y), true);
                }
            }
        }

        private void PlaceHurtboxes(int frame)
        {
            Action.Rect[,] hurtboxData;

            if (executingAction != 0)
            {
                hurtboxData = behaviors.GetAction(executingAction).HurtboxData;
            }
            else
            {
                hurtboxData = behaviors.GetAction(basicState + 100).HurtboxData;
            }
            for (var i = 0; i < hurtboxData.GetLength(1); i++)
            {
                var hurtbox = hurtboxData[frame, i];

                if (!livingHurtboxesIds.Contains(hurtbox.Id) && hurtbox.Id != -1)
                {
                    livingHurtboxesIds.Add(hurtbox.Id);
                    livingHurtboxesLifespans.Add(hurtbox.TimeActive);
                    AddBoxCollider2D((hurtbox.Id + 100).ToString(), new Vector2(hurtbox.Width, hurtbox.Height), new Vector2(facingRight ? hurtbox.X : -hurtbox.X, hurtbox.Y), false);
                }
                else if (livingHurtboxesIds.Contains(hurtbox.Id))
                {
                    /*
                Debug.Log(livingHurtboxesIds.Count);
                Debug.Log("repeat call");
                */
                }
            }
        }

        public void DecreaseHitboxLifespan()
        {
            for (var j = livingHitboxesLifespans.Count; j > 0; j--)
                if (livingHitboxesLifespans[j - 1] > 0)
                {
                    livingHitboxesLifespans[j - 1]--;
                }
                else
                {
                    RemoveBoxCollider2D(livingHitboxesIds[j - 1].ToString(), hitbox);
                    livingHitboxesIds.RemoveAt(j - 1);
                    livingHitboxesLifespans.RemoveAt(j - 1);
                }
        }

        public void DecreaseHurtboxLifespan()
        {
            for (var j = livingHurtboxesLifespans.Count; j > 0; j--)
            {
                if (livingHurtboxesLifespans[j - 1] > 1)
                {
                    livingHurtboxesLifespans[j - 1]--;
                }
                else
                {
                    RemoveBoxCollider2D((livingHurtboxesIds[j - 1] + 100).ToString(), false);
                    livingHurtboxesIds.RemoveAt(j - 1);
                    livingHurtboxesLifespans.RemoveAt(j - 1);
                }
            }
        }

        private void AddBoxCollider2D(string name, Vector2 size, Vector2 offset, bool boxType)
        {
            var childbox = new GameObject(name);

            childbox.transform.position = transform.position;
            childbox.transform.SetParent(boxType ? transform.GetChild(1) : transform.GetChild(2));

            childbox.tag = boxType ? "hitbox" + playerId : "hurtbox" + playerId;

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

        private void RemoveBoxCollider2D(string name, bool boxType)
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

        private void KillAllHurtboxes()
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

        private void KillAllHitboxes()
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

        private void KillAllBoxes()
        {
            KillAllHitboxes();
            KillAllHurtboxes();
        }

        public void OnPush(float otherHVel)
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
            KillAllBoxes();
        }

        public void OverrideDamage(int dmg, float knockback, int angle, int blck, float p1, int tier, int newAction)
        {
            var action = otherPlayer.GetComponent<PlayerScript>().executingAction;
            otherPlayer.GetComponent<PlayerScript>().executingAction = newAction;
            Damage(dmg, knockback, angle, blck, p1, tier);
            otherPlayer.GetComponent<PlayerScript>().executingAction = action;
        }

        public void Damage(int damage, float knockback, int angle, int blck, float p1, int tier)
        {
            ActionEnd();

            hKnockback = knockback * Mathf.Cos(angle / 180f * Mathf.PI) * (playerSide ? -1 : 1);
            vKnockback = knockback * Mathf.Sin(angle / 180f * Mathf.PI);
        
            if (comboTimer > 0)
            {
                p2Scale *= otherPlayer.GetComponent<PlayerScript>().Level(7);
                damage = (int)(p1Scale * p2Scale * damage * .6);
                CalculateGuts(damage);
            }

            if (shouldBlock && (blck == 3 && basicState > 3 || blck == 1 && basicState < 4 && basicState < 7 || blck == 2 || basicState > 6))
            {
                hKnockback /= 4;
                vKnockback = 0;
                health -= damage / 10;
                healthStore += damage / 10;
                meterStore += (int)(damage * .4f);
                if (!otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).Super)
                    otherPlayer.GetComponent<PlayerScript>().meterStore += (int)(damage * .2f);
                Block();
                AudioManager.Instance.PlaySound(AudioManager.Sound.Block);
            }
            else
            {
                if (otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).HitSound != null)
                {
                    AudioManager.Instance.PlaySound(otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).HitSound);
                }
                health -= damage;
                healthStore += damage;
                meterStore += (int)(damage * .8f);
                if (!otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).Super)
                    otherPlayer.GetComponent<PlayerScript>().meterStore += (int)(damage * 1.4f);
                Stun();
            }

            if (vKnockback > 0)
            {
                airborn = true;
            }

            vVelocity = 0;
        }

        private int CalculateGuts(int damage)
        {
            if (health / maxHealth <= .5 && health / maxHealth > .4)
            {
                damage = (int)(damage * .94f);
            }
            else if (health / maxHealth <= .4 && health / maxHealth > .35)
            {
                damage = (int)(damage * .85f);
            }
            else if (health / maxHealth <= .35 && health / maxHealth > .15)
            {
                damage = (int)(damage * .78f);
            }
            else if (health / maxHealth <= .15)
            {
                damage = (int)(damage * .66f);
            }
            return damage;
        }

        private void Stun()
        {
            ActionEnd();
            firstStun = true;

            if (otherPlayer.GetComponent<PlayerScript>().currentFrameType == 5 && otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).Knockdown > 0 && otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).Knockdown <= 2)
            {
                executingAction = 54;
            }
            else if (otherPlayer.GetComponent<PlayerScript>().currentFrameType == 5 && otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).Knockdown <= 4)
            {
                executingAction = 45;
                shouldGroundbounce = true;
                stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().Level(1);

            }
            else if (otherPlayer.GetComponent<PlayerScript>().currentFrameType == 5 && otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).Knockdown >= 5)
            {
                executingAction = 45;
                shouldWallbounce = true;
                stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().Level(1);
            }
            else
            {
                executingAction = 45;
                stunTimer = (int)otherPlayer.GetComponent<PlayerScript>().Level(1);
            }

            otherPlayer.GetComponent<PlayerScript>().comboCounter++;

            _comboCounterText.Value = otherPlayer.GetComponent<PlayerScript>().comboCounter;

            if (comboTimer == 0)
                p1Scale = otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(otherPlayer.GetComponent<PlayerScript>().executingAction).P1Scaling;

            comboTimer = 1;
        }

        private void Block()
        {
            ActionEnd();

            if (basicState >= 7)
                executingAction = 48;
            else if (basicState <= 3)
                executingAction = 47;
            else
                executingAction = 46;

            blockTimer = (int)otherPlayer.GetComponent<PlayerScript>().Level(6);
        }

        public float Level(int wanted)
        {
            return _levelScaling[behaviors.GetAction(executingAction).Level, wanted];
        }

        private void AnimInt(int hash, int value)
        {
            animator.SetInteger(hash, value);
        }

        private void AnimBool(bool b, string s)
        {
            animator.SetBool(s, b);
        }

        private void MoveX(float amm)
        {
            transform.Translate(amm, 0, 0);
        }

        private void MoveY(float amm)
        {
            transform.Translate(0, amm, 0);
        }

        private void SetY(float amm)
        {
            MoveY(-transform.position.y + amm);
        }

        private void SetX(float amm)
        {
            MoveX(-transform.position.x + amm);
        }

        private float Y()
        {
            return transform.position.y;
        }

        public float X()
        {
            return transform.position.x;
        }

        public Vector2 Position()
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
    }
}