using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
        lite = false;
        medium = false;
        heavy = false;
        special = false;

        if (button)
        {
            if (Input.GetKey(lightKey) || lightButton.isActiveAndEnabled)
            {
                if (!litLock)
                {
                    litLock = true;
                    lite = true;
                }
            }
            else
            {
                litLock = false;
            }
            if (Input.GetKey(mediumKey) || mediumButton.isActiveAndEnabled)
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
            if (Input.GetKey(heavyKey) || heavyButton.isActiveAndEnabled)
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
            if (Input.GetKey(specialKey))
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
        else
        {
            if (Input.GetKey(lightKey))
            {
                if (!litLock)
                {
                    litLock = true;
                    lite = true;
                }
            }
            else
            {
                litLock = false;
            }
            if (Input.GetKey(mediumKey))
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
            if (Input.GetKey(heavyKey))
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
            if (Input.GetKey(specialKey))
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
            if (Input.GetKey(upKey) || JoyScript.Up)
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

            if (Input.GetKey(leftKey) || JoyScript.Left)
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

            if (Input.GetKey(downKey) || JoyScript.Down)
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

            if (Input.GetKey(rightKey) || JoyScript.Right)
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
            if (Input.GetKey(upKey))
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

            if (Input.GetKey(leftKey))
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

            if (Input.GetKey(downKey))
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

            if (Input.GetKey(rightKey))
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
	void Update () {
		
	}
}
