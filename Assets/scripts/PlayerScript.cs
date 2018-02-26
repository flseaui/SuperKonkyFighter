using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    float FLOOR_HEIGHT = 0;
    float BASE_GRAVITY = -0.05f;

    public float[,] levelScaling = new float[,]
    {
        { 8,  12, 23, 9,  .75f },
        { 10, 14, 26, 11, .8f  },
        { 12, 16, 28, 13, .85f },
        { 14, 19, 33, 16, .89f },
        { 16, 21, 36, 18, .92f },
        { 18, 24, 40, 20, .94f }
    };

    bool damageDealt;
    public bool waitForGround;
    public bool waitForEnd;
    public bool air;
    public bool hitStopped;
    public bool stunned;
    public bool flipFacing;
    public bool flip;
    public bool facingRight;

    public int bufferedMove;
    public int maxHealth;
    public int health;
    public int currentFrame;
    public int currentActionFrame;
    public int damageCounter;
    public int playerID;
    public int meter;
    public int basicState;
    public int AttackState;
    public int advState;
    public int jump;
    public int dashTimer;
    public int currentAction;
    public bool dashDirection; // false = left, true = right
    public bool airDashed;
    private int stunTimer;
    public int updateEnd;

    public float hKnockback;
    public float vKnockback;
    public float gravity;
    public float vVelocity;
    public float hVelocity;
    public float baseHeight;
    public float width;
    public float height;
    public float hPush;
    private float forwardSpeed;
    private float backwardSpeed;
    private float jumpSpeed;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Behaviors behaviors;
    public BoxCollider2D hitbox;
    public BoxCollider2D hurtbox;
    public GameObject otherPlayer;
    public JoyScript JoyScript;
    InputManager inputManager;

    public List<Vector2> livingHitboxes;

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
        hitbox.tag = "collisionHitbox" + playerID.ToString();
        hurtbox.tag = playerID.ToString();

        forwardSpeed = 0.25f;
        backwardSpeed = -0.15f;
        jumpSpeed = 1.25f;
        vVelocity = 0;
        hVelocity = 0;
        gravity = BASE_GRAVITY;

        //konky specific things...
        maxHealth = 11000;
        health = maxHealth;
        behaviors = new KonkyBehaviours();
        baseHeight = 8;
        width = 4;

        livingHitboxes = new List<Vector2>();

        if (CompareTag("1"))
            inputManager = new InputManager(1);
        else if (CompareTag("2"))
            inputManager = new InputManager(2);
    }

    // Update is called once per frame
    void Update()
    {
        GameUpdate();

        if (stunned)
        {

        }
    }

    public void updateAnimation()
    {
        if (currentAction != 0)
        {
            animInt(Animator.StringToHash("Action"), behaviors.getAnimAction(behaviors.getAction(currentAction)));
            animInt(Animator.StringToHash("Basic"), 0);
        }
        else
        {
            animInt(Animator.StringToHash("Basic"), basicState);
            animInt(Animator.StringToHash("Action"), 0);
        }
    }

    private void GameUpdate()
    {
        inputManager.pollInput(0);

        basicState = inputConvert(inputManager.currentInput);
        setAttackInput(inputManager.currentInput);
        setAdvancedInput(inputManager.currentInput);

        if (hitStopped)
        {
            if (advState != 0)
                buffer(advState + 40);
            else if (AttackState != 0)
                buffer(currentAction = AttackState);

            updateEnd = 1;
        }
        else
        {
            if (currentAction != 0)
                incrementFrame(behaviors.getAction(currentAction).frames);
            stateCheck();

            if (currentAction >= 40)
                advancedMove();

            updateEnd = 2;
        }
    }

    public void UpdateEnd()
    {
        this.transform.localScale = facingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        movePlayer();

        updateAnimation();
    }

    private void buffer(int bufferedInput)
    {
        foreach (int Actions in behaviors.getAction(currentAction).actionCancels)
            if (Actions == bufferedInput)
                bufferedMove = bufferedInput;
    }

    private void movePlayer()
    {
        if (currentAction != 43 && currentAction != 44)
            vVelocity += gravity;

        if (vVelocity < -1)
        {
            vVelocity = -1;
        }

        moveX((facingRight ? hVelocity - hPush : -hVelocity + hPush) + hKnockback);
        moveY(vVelocity + vKnockback);

        if (x() < -64f)
        {
            if (hPush != 0)
                otherPlayer.GetComponent<PlayerScript>().moveX(hPush);
            setX(-64);
        }
        else if (x() > 64f)
        {
            if (hPush != 0)
                otherPlayer.GetComponent<PlayerScript>().moveX(-hPush);
            setX(64);
        }

        if (y() <= FLOOR_HEIGHT) //ground snap
        {
            if (air)
            {
                air = false;
                ActionEnd();
                airDashed = false;
            }
            vVelocity = 0;
            setY(FLOOR_HEIGHT);
        }
        else
        {
            air = true;
        }
    }

    public void onPush(float otherVel)
    {
        float diff;

        if (facingRight)
            hPush = (hVelocity + otherVel) / 2;
        else
            hPush = (otherVel + hVelocity) / 2;
        if (facingRight)
        {
            if ((hitbox.transform.position.x + (hitbox.size.x / 2)) > (otherPlayer.GetComponent<PlayerScript>().hitbox.transform.position.x - (otherPlayer.GetComponent<PlayerScript>().hitbox.size.x / 2)))
            {
                diff = (hitbox.transform.position.x + (hitbox.size.x / 2)) - (otherPlayer.GetComponent<PlayerScript>().hitbox.transform.position.x - (otherPlayer.GetComponent<PlayerScript>().hitbox.size.x / 2));
            }
            else
                diff = 0;
        }
        else
        {
            if ((hitbox.transform.position.x - (hitbox.size.x / 2)) > (otherPlayer.GetComponent<PlayerScript>().hitbox.transform.position.x + (otherPlayer.GetComponent<PlayerScript>().hitbox.size.x / 2)))
            {
                diff = ((hitbox.transform.position.x - (hitbox.size.x / 2)) - (otherPlayer.GetComponent<PlayerScript>().hitbox.transform.position.x + (otherPlayer.GetComponent<PlayerScript>().hitbox.size.x / 2)));
            }
            else
                diff = 0;
        }

        if (hVelocity < otherPlayer.GetComponent<PlayerScript>().hVelocity)
            hPush += diff;
        else if (hVelocity == otherPlayer.GetComponent<PlayerScript>().hVelocity)
            hPush += diff / 2;
    }

    private int inputConvert(bool[] input)
    {
        if (!air)
        {
            if (input[0])
            {
                if (input[2])
                    return (facingRight ? 7 : 9);
                else if (input[3])
                    return (facingRight ? 9 : 7);
                else
                    return 8;
            }
            else if (input[1])
            {
                if (input[2])
                    return (facingRight ? 1 : 3);
                else if (input[3])
                    return (facingRight ? 3 : 1);
                else
                    return 2;
            }
            else if (input[2])
                return (facingRight ? 4 : 6);
            else if (input[3])
                return (facingRight ? 6 : 4);
            else
                return 5;
        }
        else
        {
            return basicState;
        }
    }

    private void setAdvancedInput(bool[] input)
    {
        if (currentAction != 41)
        {
            if (dashTimer == 0 && advState <= 4)
                advState = 0;

            if (input[8] && !dashDirection && dashTimer != 0)
            {
                if (facingRight)
                {
                    if (air)
                    {
                        if (!airDashed)
                        {
                            advState = 4;
                            airDashed = true;
                        }
                    }
                    else
                        advState = 2;
                }
                else
                {
                    if (air)
                    {
                        if (!airDashed)
                        {
                            advState = 3;
                            airDashed = true;
                        }
                    }
                    else
                        advState = 1;
                }
                dashTimer = 0;
            }
            else if (input[9] && dashDirection && dashTimer != 0)
            {
                if (facingRight)
                {
                    if (air)
                    {
                        if (!airDashed)
                        {
                            advState = 3;
                            airDashed = true;
                        }
                    }
                    else
                        advState = 1;
                }
                else
                {
                    if (air)
                    {
                        if (!airDashed)
                        {
                            advState = 4;
                            airDashed = true;
                        }
                    }
                    else
                        advState = 2;
                }
                dashTimer = 0;
            }

            if (waitForGround && !air)
            {
                waitForGround = false;
                advState = 9;
            }

            if ((!input[8] || !input[9]) && dashTimer != 0)
                dashTimer--;

            if (input[8] || input[9])
            {
                dashTimer = 15;
                if (input[8])
                    dashDirection = false;
                else
                    dashDirection = true;
            }
        }

        if (flip)
        {
            flip = false;
            if (currentAction != 0)
                waitForEnd = true;

            if (air)
                waitForGround = true;

            if (!air && currentAction == 0)
            {
                if (basicState <= 3)
                    advState = 10;
                else
                    advState = 9;
                dashTimer = 0;
            }
        }
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
        currentFrame = frames[currentActionFrame];
        currentActionFrame++;

        // Debug.Log("currentActionFrame" + currentActionFrame);
        if (previousFrame != 1 && currentFrame == 1)
        {
            otherPlayer.GetComponentInChildren<HitboxScript>().initialFrame = false;
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
            if (!air && waitForEnd && !waitForGround && !behaviors.getAction(currentAction).infinite && damageDealt)
            {
                Debug.Log("WaitForEnd ActionEnd");
                ActionEnd();
            }
            else if (advState != 0 && !waitForEnd)
            {
                foreach (int Actions in behaviors.getAction(currentAction).actionCancels)
                    if (Actions == advState + 40)
                        bufferedMove = advState + 40;
            }
            else if (basicState >= 7 && !waitForEnd)
            {
                foreach (int Actions in behaviors.getAction(currentAction).actionCancels)
                    if (Actions == 40 && inputManager.currentInput[12])
                    {
                        Debug.Log("Jump Cancled");
                        bufferedMove = 40;
                        if (basicState == 7)
                            jump = 7;
                        else if (basicState == 8)
                            jump = 8;
                        else
                            jump = 9;


                    }
            }
            else if (AttackState != 0 && !waitForEnd)
            {
                foreach (int Actions in behaviors.getAction(currentAction).actionCancels)
                    if (Actions == AttackState)
                        bufferedMove = AttackState;
            }

            if (bufferedMove != 0 && !waitForEnd)
            {
                Debug.Log("Buffered");
                if (bufferedMove > 40)
                    advState = bufferedMove - 40;
                else if (bufferedMove == 40)
                {
                    advState = 0;
                    AttackState = 0;
                }
                else
                    AttackState = bufferedMove;
                bufferedMove = 0;
                ActionEnd();
            }
        }
    }

    private void placeHitboxes()
    {
        Action.rect[,] hitboxData = behaviors.getAction(currentAction).hitboxData;
        int height = behaviors.getAction(currentAction).hitboxData.GetLength(1);
        for (int i = 0; i < behaviors.getAction(currentAction).hitboxData.GetLength(0); i++)
        {
            for (int j = 0; j < height; j++)
            {
                Action.rect hitbox = hitboxData[i, j];
                if (!livingHitboxes.Contains(new Vector2(hitbox.id, 1)))
                    livingHitboxes.Add(new Vector2(hitbox.id, hitbox.timeActive));
                livingHitboxes[i * height + j].Set(livingHitboxes[i].x, livingHitboxes[i].y - 1);
                if (livingHitboxes[i].y >= hitbox.timeActive)
                    addBoxCollider2D(new Vector2(hitbox.width, hitbox.height), new Vector2(hitbox.x, hitbox.y));
            }
        }
    }

    private void addBoxCollider2D(Vector2 size, Vector2 offset)
    {
        BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        boxCollider2D.size = size;
        boxCollider2D.offset = offset;
    }

    private void stateCheck()
    {
        if (currentAction != 0)
        {
            advState = 0;
            AttackState = 0;
            if (!air)
                hVelocity = 0;
            if (currentActionFrame >= behaviors.getAction(currentAction).frames.Length)
            {
                if (behaviors.getAction(currentAction).infinite)
                    currentActionFrame--;
                else
                    ActionEnd();
            }
        }
        else if (advState != 0)
        {
            currentAction = advState + 40;
        }
        else if (AttackState != 0)
        {
            currentAction = AttackState;
        }
        else
        {
            basicMove();
        }
    }

    private void basicMove()
    {
        if (!air)
        {
            jump = 0;

            if (basicState == 8)
                vVelocity = jumpSpeed;
            else if (basicState == 7)
            {
                vVelocity = jumpSpeed;
                hVelocity = backwardSpeed;
            }
            else if (basicState == 9)
            {
                vVelocity = jumpSpeed;
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

        if (air)
        {
            if ((inputManager.currentInput[12] && inputManager.currentInput[2] && facingRight) || (inputManager.currentInput[12] && inputManager.currentInput[3] && !facingRight))
            {       
                jump = 7;
                basicState = 7;
            }
            else if ((inputManager.currentInput[12] && inputManager.currentInput[3] && facingRight) || (inputManager.currentInput[12] && inputManager.currentInput[2] && !facingRight))
            {
                jump = 9;
                basicState = 9;
            }
            else if (inputManager.currentInput[12])
            {
                jump = 8;
                basicState = 8;
            }

            if (!airDashed && jump >= 7)
            {
                airDashed = true;

                if (jump == 7)
                {
                    vVelocity = jumpSpeed;
                    hVelocity = backwardSpeed;
                }
                else if (jump == 9)
                {
                    vVelocity = jumpSpeed;
                    hVelocity = forwardSpeed;
                }
                else
                    vVelocity = jumpSpeed;
            }
        }
    }
    private void advancedMove()
    {
        switch (currentAction - 40)
        {
            case 1:
                hVelocity = forwardSpeed * 3;
                Debug.Log("Dashing");
                if ((!inputManager.currentInput[2] && !dashDirection) || (!inputManager.currentInput[3] && dashDirection))
                {
                    hVelocity = 0;
                    ActionEnd();
                }
                break;
            case 2:
                hVelocity = backwardSpeed * 3f;
                break;
            case 3:
                vVelocity = 0;
                hVelocity = forwardSpeed * 3;
                break;
            case 4:
                vVelocity = 0;
                hVelocity = backwardSpeed * 3f;
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }

    private void ActionEnd()
    {
        if (currentAction == 49 || currentAction == 50)
        {
            facingRight = flipFacing;
        }

        currentAction = 0;
        currentFrame = 0;
        currentActionFrame = 0;

        if (waitForEnd && !waitForGround)
        {
            Debug.Log("WaitForEnd Flip");

            waitForEnd = false;
            if (basicState <= 3)
                currentAction = 50;
            else
                currentAction = 49;
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

    public int damage(int ammount, float k, int angle, int ac, bool bl)
    {
        health -= ammount;
        damageDealt = true;
        hKnockback = k * Mathf.Cos(((float)angle / 180f) * Mathf.PI) * (facingRight ? -1 : 1);
        vKnockback = k * Mathf.Sin(((float)angle / 180f) * Mathf.PI);

        if (vKnockback > 0)
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
        return levelScaling[behaviors.getAction(currentAction).level, wanted];
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

}