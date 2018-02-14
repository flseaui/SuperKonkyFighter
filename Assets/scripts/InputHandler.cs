using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler
{

    private KeyCode upKey,
                    rightKey,
                    downKey,
                    leftKey,
                    lightKey,
                    mediumKey,
                    heavyKey,
                    specialKey;

    public Button lightButton,
                  mediumButton,
                  heavyButton;

    InputHandler(int playerID)
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
    }

    bool[] getInput()
    {

        return new bool[];
    }

    // Handles player input
    void handleInput()
    {
        up1 = false;
        right1 = false;
        down1 = false;
        left1 = false;
        up = false;
        right = false;
        down = false;
        left = false;
        light = false;
        medium = false;
        heavy = false;
        special = false;

        if (button)
        {
            if (Input.GetKeyDown(lightKey) || lightButton.isActiveAndEnabled)
            {
                light = true;
            }

            if (Input.GetKeyDown(mediumKey) || mediumButton.isActiveAndEnabled)
            {
                medLock = true;
            }

            if (Input.GetKeyDown(heavyKey) || heavyButton.isActiveAndEnabled)
            {
                hevLock = true;
            }

            if (Input.GetKeyDown(specialKey))
            {
                special = true;
            }

        }
        else
        {
            if (Input.GetKeyDown(lightKey))
            {
                if (!lightLock)
                {
                    lightLock = true;
                    light = true;
                }
            }
            else
            {
                lightLock = false;
            }
            if (Input.GetKeyDown(mediumKey))
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
            if (Input.GetKeyDown(heavyKey))
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
            if (Input.GetKeyDown(specialKey))
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
        }

        if (joy)
        {
            if (Input.GetKeyDown(upKey) || JoyScript.Up)
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

            if (Input.GetKeyDown(leftKey) || JoyScript.Left)
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

            if (Input.GetKeyDown(downKey) || JoyScript.Down)
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

            if (Input.GetKeyDown(rightKey) || JoyScript.Right)
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
        }
        else
        {
            if (Input.GetKeyDown(upKey))
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

            if (Input.GetKeyDown(leftKey))
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

            if (Input.GetKeyDown(downKey))
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

            if (Input.GetKeyDown(rightKey))
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
        }

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
        }
        else if (special)
        {
            iAttack = SPECIAL_ATTACK;
        }
        else
        {
            iAttack = NO_ATTACK_STRENGTH;
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

        if (iAttack != NO_ATTACK_STRENGTH)
        {
            history.Add(iAttack);
            delays.Add(inputTimer);
            inputTimer = 0;
            history.RemoveAt(0);
            delays.RemoveAt(0);
        }

        if (iAttack == NO_ATTACK_STRENGTH && iState == 5)
        {
            ++inputTimer;
        }
        //

        if (air)
        {
            airLock = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
