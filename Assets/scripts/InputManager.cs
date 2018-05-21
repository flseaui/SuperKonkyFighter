using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager
{
    public static bool isInputEnabled = false;

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

    public float hAxis, vAxis, dhAxis, dvAxis, aAxis, htAxis, vtAxis;
    public float hAxisPrev, vAxisPrev, dhAxisPrev, dvAxisPrev;
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

    public bool justPressed(string direction)
    {
        switch (direction)
        {
            case "left":
                return (hAxisPrev == 0 && hAxis < 0) || (dhAxisPrev == 0 && dhAxis < 0);
            case "right":
                return (hAxisPrev == 0 && hAxis > 0) || (dhAxisPrev == 0 && dhAxis > 0);
            case "up":
                return (vAxisPrev == 0 && vAxis < 0) || (dvAxisPrev == 0 && dvAxis > 0);
            case "down":
                return (vAxisPrev == 0 && vAxis > 0) || (dvAxisPrev == 0 && dvAxis < 0);
            default:
                return false;
        }
    }

    public bool justReleased(string direction)
    {
        switch (direction)
        {
            case "left":
                return (hAxisPrev < 0 && hAxis == 0) || (dhAxisPrev < 0 && dhAxis == 0);
            case "right":
                return (hAxisPrev > 0 && hAxis == 0) || (dhAxisPrev > 0 && dhAxis == 0);
            case "up":
                return (vAxisPrev < 0 && vAxis == 0) || (dvAxisPrev > 0 && dvAxis == 0);
            case "down":
                return (vAxisPrev > 0 && vAxis == 0) || (dvAxisPrev < 0 && dvAxis == 0);
            default:
                return false;
        }
    }

    /* 
     * Polls user input
     * inputType = 0 : keyboard
     * inputType = 1 : joystick 
     */
    public void pollInput(int inputType)
    {
        if (isInputEnabled)
        {
            hAxisPrev = hAxis;
            vAxisPrev = vAxis;
            dhAxisPrev = dhAxis;
            dvAxisPrev = dvAxis;
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");
            htAxis = Input.GetAxis("HorizontalTurn");
            vtAxis = Input.GetAxis("VerticalTurn");
            dhAxis = Input.GetAxis("XboxDpadHorizontal");
            dvAxis = Input.GetAxis("XboxDpadVertical");

            ControllerCheck();

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
                    currentInput[0] = vAxis < 0 || dvAxis > 0;
                    currentInput[1] = vAxis > 0 || dvAxis < 0;
                    currentInput[2] = hAxis < 0 || dhAxis < 0;
                    currentInput[3] = hAxis > 0 || dhAxis > 0;
                    currentInput[4] = (Input.GetButton("XboxX"));
                    currentInput[5] = (Input.GetButton("XboxY"));
                    currentInput[6] = (Input.GetButton("XboxRB"));
                    currentInput[7] = (Input.GetButton("XboxA"));
                    currentInput[8] = justPressed("left");
                    currentInput[9] = justPressed("right");
                    currentInput[10] = justReleased("left");
                    currentInput[11] = justReleased("right");
                    currentInput[12] = justPressed("up");
                    break;
                case 2:
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
    }

    void ControllerCheck()
    {
        float ltaxis = Input.GetAxis("XboxLeftTrigger");
        float rtaxis = Input.GetAxis("XboxRightTrigger");

        bool xbox_a = Input.GetButton("XboxA");
        bool xbox_b = Input.GetButton("XboxB");
        bool xbox_x = Input.GetButton("XboxX");
        bool xbox_y = Input.GetButton("XboxY");
        bool xbox_lb = Input.GetButton("XboxLB");
        bool xbox_rb = Input.GetButton("XboxRB");
        bool xbox_ls = Input.GetButton("XboxLS");
        bool xbox_rs = Input.GetButton("XboxRS");
        bool xbox_view = Input.GetButton("XboxView");
        bool xbox_menu = Input.GetButton("XboxMenu");

        //Debug.LogFormat("h: {0}, v: {1}", dhAxis, dvAxis);
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

}