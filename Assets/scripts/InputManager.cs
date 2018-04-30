using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager
{

    public bool[] currentInput = new bool[13];

    public bool up = false,
                right = false,
                down = false,
                left = false,
                light = false,
                medium = false,
                heavy = false,
                special = false;

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

    public InputManager(int playerID)
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
    public void pollInput(int inputType)
    {
        switch (inputType)
        {
            case 0:
                // Keyboard
                currentInput[0] = (Input.GetKey(upKey));
                currentInput[1] = (Input.GetKey(downKey));
                currentInput[2] = (Input.GetKey(leftKey));
                currentInput[3] = (Input.GetKey(rightKey));
                currentInput[4] = (Input.GetKeyDown(lightKey));
                currentInput[5] = (Input.GetKeyDown(mediumKey));
                currentInput[6] = (Input.GetKeyDown(heavyKey));
                currentInput[7] = (Input.GetKeyDown(specialKey));
                currentInput[8] = (Input.GetKeyDown(leftKey));
                currentInput[9] = (Input.GetKeyDown(rightKey));
                currentInput[10] = (Input.GetKeyUp(leftKey));
                currentInput[11] = (Input.GetKeyUp(rightKey));
                currentInput[12] = (Input.GetKeyDown(upKey));
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
                currentInput[0] = (Input.GetKey(upKey));
                currentInput[1] = (Input.GetKey(downKey));
                currentInput[2] = (Input.GetKey(leftKey));
                currentInput[3] = (Input.GetKey(rightKey));
                currentInput[4] = (Input.GetKeyDown(lightKey));
                currentInput[5] = (Input.GetKeyDown(mediumKey));
                currentInput[6] = (Input.GetKeyDown(heavyKey));
                currentInput[7] = (Input.GetKeyDown(specialKey));
                currentInput[8] = (Input.GetKeyDown(leftKey));
                currentInput[9] = (Input.GetKeyDown(rightKey));
                currentInput[10] = (Input.GetKeyUp(leftKey));
                currentInput[11] = (Input.GetKeyUp(rightKey));
                currentInput[12] = (Input.GetKeyDown(upKey));
                break;
        }
    }

    // Handles player input
    public void handleInput()
    {
        pollInput(0);

        up      = currentInput[0];
        down    = currentInput[1];
        left    = currentInput[2];
        right   = currentInput[3];
        light   = currentInput[4];
        medium  = currentInput[5];
        heavy   = currentInput[6];
        special = currentInput[7];
    }

    // Update is called once per frame
    void Update()
    {

    }
}