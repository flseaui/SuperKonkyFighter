using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviors {

    IDictionary<int, Action> ActionIds;
    IDictionary<Action, int> AnimAction;

    public float forwardSpeed = 0.25f,
                 backwardSpeed = -0.15f,
                 dashForwardSpeed = 3f,
                 dashBackSpeed = 3f,
                 airDashForwardSpeed = 3f,
                 airDashBackSpeed = 3f,
                 jumpDirectionSpeed = 1.25f,
                 gravity = -0.05f;
    public int maxHealth = 11000;
    public bool infiniteDashForward = true;

    public static Action.rect nullBox = new Action.rect(0, 0, -100, -100, 0, -1);

    // Returns Action object from given id
    public Action getAction(int id)
    {
        return ActionIds[id];
    }

    protected void setIds(IDictionary<int, Action> ActionIds, IDictionary<Action, int> AnimAction)
    {
        this.ActionIds = ActionIds;
        this.AnimAction = AnimAction;
    }

    public void setStats()
    {
        forwardSpeed = 0.25f;
        backwardSpeed = -0.15f;
        jumpDirectionSpeed = 1.25f;
        dashForwardSpeed = 3f;
        dashBackSpeed = 3f;
        airDashForwardSpeed = 3f;
        airDashBackSpeed = 3f;
        gravity = -0.05f;
        maxHealth = 11000;
        infiniteDashForward = false;
    }

    public int getAnimAction(Action id)
    {
        return AnimAction[id];
    }

    public float getForwardSpeed()
    {
        return forwardSpeed;
    }

    public float getBackwardSpeed()
    {
        return backwardSpeed;
    }

    public float getJumpDirectionSpeed()
    {
        return jumpDirectionSpeed;
    }

    public float getDashForwardSpeed()
    {
        return dashForwardSpeed;
    }

    public float getDashBackSpeed()
    {
        return dashBackSpeed;
    }

    public float getAirDashForwardSpeed()
    {
        return airDashForwardSpeed;
    }

    public float getAirDashBackSpeed()
    {
        return airDashBackSpeed;
    }

    public bool getInfiniteDashForward()
    {
        return infiniteDashForward;
    }

    public float getGravity()
    {
        return gravity;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }
}

