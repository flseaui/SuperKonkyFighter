using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager
{
    public static bool isInputEnabled = false;

    // controller 1 and 2 ids
    public static int c1id, c2id;

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

    public float hAxis, vAxis, dhAxis, dvAxis, 
                 hAxisPrev, vAxisPrev, dhAxisPrev, dvAxisPrev;

    public bool x, y, a, b, rb;

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
     * inputType = 0 : keyboards
     * inputType = 1 : joystick 
     */
    public void pollInput(int inputType, int playerID)
    {
        if (c1id >= 3 && playerID == 1)
            inputType = 0;
        if (c2id >= 3 && playerID == 2)
            inputType = 0;

        if (isInputEnabled)
        {
            foreach (string name in Input.GetJoystickNames())
                Debug.Log(name);

            hAxisPrev = hAxis;
            vAxisPrev = vAxis;
            dhAxisPrev = dhAxis;
            dvAxisPrev = dvAxis;

            if (inputType == 1)
            {
                if (playerID == 1)
                {
                    if (c1id == 1)
                    {
                        hAxis = Input.GetAxis("Horizontal");
                        vAxis = Input.GetAxis("Vertical");
                        dhAxis = Input.GetAxis("XboxDpadHorizontal");
                        dvAxis = Input.GetAxis("XboxDpadVertical");
                        x = (Input.GetButton("XboxX"));
                        y = (Input.GetButton("XboxY"));
                        rb = (Input.GetButton("XboxRB"));
                        a = (Input.GetButton("XboxA"));
                    }
                    else if (c2id == 1)
                    {
                        hAxis = Input.GetAxis("Horizontal_2");
                        vAxis = Input.GetAxis("Vertical_2");
                        dhAxis = Input.GetAxis("XboxDpadHorizontal_2");
                        dvAxis = Input.GetAxis("XboxDpadVertical_2");
                        x = (Input.GetButton("XboxX_2"));
                        y = (Input.GetButton("XboxY_2"));
                        rb = (Input.GetButton("XboxRB_2"));
                        a = (Input.GetButton("XboxA_2"));
                    }
                }
                else if (playerID == 2)
                {
                    if (c1id == 2)
                    {
                        hAxis = Input.GetAxis("Horizontal");
                        vAxis = Input.GetAxis("Vertical");
                        dhAxis = Input.GetAxis("XboxDpadHorizontal");
                        dvAxis = Input.GetAxis("XboxDpadVertical");
                        x = (Input.GetButton("XboxX"));
                        y = (Input.GetButton("XboxY"));
                        rb = (Input.GetButton("XboxRB"));
                        a = (Input.GetButton("XboxA"));
                    }
                    else if (c2id == 2)
                    {
                        hAxis = Input.GetAxis("Horizontal_2");
                        vAxis = Input.GetAxis("Vertical_2");
                        dhAxis = Input.GetAxis("XboxDpadHorizontal_2");
                        dvAxis = Input.GetAxis("XboxDpadVertical_2");
                        x = (Input.GetButton("XboxX_2"));
                        y = (Input.GetButton("XboxY_2"));
                        rb = (Input.GetButton("XboxRB_2"));
                        a = (Input.GetButton("XboxA_2"));
                    }
                }
            }

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
                    currentInput[4] = x;
                    currentInput[5] = y;
                    currentInput[6] = rb;
                    currentInput[7] = a;
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

}