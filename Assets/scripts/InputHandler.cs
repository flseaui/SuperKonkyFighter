using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler
{

    bool[] currentInput;

    JoyScript joyScript;

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

    /* 
     * Polls user input
     * inputType = 0 : keyboard
     * inputType = 1 : joystick 
     */
    void pollInput(int inputType)
    {
        switch (inputType)
        {
            case 0:
                // Keyboard
                currentInput[0] = (Input.GetKeyDown(upKey));
                currentInput[1] = (Input.GetKeyDown(downKey));
                currentInput[2] = (Input.GetKeyDown(leftKey));
                currentInput[3] = (Input.GetKeyDown(rightKey));
                currentInput[4] = (Input.GetKeyDown(lightKey));
                currentInput[5] = (Input.GetKeyDown(mediumKey));
                currentInput[6] = (Input.GetKeyDown(heavyKey));
                currentInput[7] = (Input.GetKeyDown(specialKey));
                break;
            case 1:
                // Joystick
                currentInput[0] = (joyScript.Up);
                currentInput[1] = (joyScript.Down);
                currentInput[2] = (joyScript.Left);
                currentInput[3] = (joyScript.Right);
                //currentInput[4] = (light button);
                //currentInput[5] = (medium button);
                //currentInput[6] = (heavy button);
                //currentInput[7] = (specialbutton);
                break;
            default:
                // Default - Keyboard
                currentInput[0] = (Input.GetKeyDown(upKey));
                currentInput[1] = (Input.GetKeyDown(downKey));
                currentInput[2] = (Input.GetKeyDown(leftKey));
                currentInput[3] = (Input.GetKeyDown(rightKey));
                currentInput[4] = (Input.GetKeyDown(lightKey));
                currentInput[5] = (Input.GetKeyDown(mediumKey));
                currentInput[6] = (Input.GetKeyDown(heavyKey));
                currentInput[7] = (Input.GetKeyDown(specialKey));
                break;
        }
    }

    // Handles player input
    void handleInput()
    {
        pollInput(0);

        bool up1 = false,
            right1 = false,
            down1 = false,
            left1 = false,
            up = false,
            right = false,
            down = false,
            left = false,
            light = false,
            medium = false,
            heavy = false,
            special = false;

        up      = currentInput[0];
        down    = currentInput[1];
        left    = currentInput[2];
        right   = currentInput[3];
        light   = currentInput[4];
        medium  = currentInput[5];
        heavy   = currentInput[6];
        special = currentInput[7];

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
