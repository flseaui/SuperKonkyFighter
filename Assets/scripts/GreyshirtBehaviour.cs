using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyshirtBehaviours : Behaviors
{
    const int LOW = 1;//must be crouchblocked
    const int MID = 2;//can be crouch or stand blocked
    const int HIGH = 3;//must be stand blocked
    const int UNBLOCKABLE = 4;//cannot be blocked

    const int NONE = 0; //no knockdown (follows hitstun numbers)
    const int SOFTKD = 1; //soft knockdown (techable)
    const int HARDKD = 2; //hard knockdown (untechable for 20 frames, OTG possible)
    const int SOFTGB = 3; //soft ground bounce (ground bounce with soft knockdown)
    const int HARDGB = 4; //hard ground bounce (ground bounce with hard knockdown)
    const int SOFTWB = 5; //soft wall bounce (wall bounce with soft knockdown)
    const int HARDWB = 6; //hard wall bounce (wall bounce with hard knockdown)

    /* 
     * Action ID FORMAT
     * id = numpad + power
     * - light    = + 0
     * - medium   = + 10
     * - heavy    = + 20
     * - special  = + 30
     * - advanced = + 40
     * example: standM = 5 + 10 = 15
     */

    IDictionary<int, Action> greyshirtActionIds;
    IDictionary<Action, int> greyshirtAnimAction;

    /* 
       * ADVANCED ID FORMAT
       * 1 - Forward Dash
       * 2 - Back Dash
       * 3 - Forward Air Dash
       * 4 - Back Air Dash
       * 5 - Stun
       * 6 - Block
       * 7 - crouchBlock
       * 8 - airBlock
       * 9 - flip
       * 10 - crouchFlip
       * 11 - jumpSquat
       * 12 - throw
       * Other
       * 0 - Jump
       */

    public GreyshirtBehaviours()
    {
        greyshirtActionIds = new Dictionary<int, Action>()
        {
            { 1,  crouchL },
            { 11, crouchM },
            { 21, crouchH },

            { 2,  crouchL },
            { 12, crouchM },
            { 22, crouchH },

            { 3,  crouchL },
            { 13, crouchM },
            { 23, crouchH },

            { 4,  standL },
            { 14, standM },
            { 24, standH },

            { 5,  standL },
            { 15, standM },
            { 25, standH },

            { 6,  standL },
            { 16, standM },
            { 26, standH },

            { 7,  jumpL },
            { 17, jumpM },
            { 27, jumpH },

            { 8,  jumpL },
            { 18, jumpM },
            { 28, jumpH },

            { 9,  jumpL },
            { 19, jumpM },
            { 29, jumpH },

            { 31, oneS   },
            { 32, twoS   },
            { 33, threeS },
            { 34, fourS  },
            { 35, fiveS  },
            { 36, sixS   },

            { 41,    forwardDash},
            { 42,       backDash},
            { 43, forwardAirDash},
            { 44,    backAirDash},
            { 45,          stun },
            { 46,         block },
            { 47,   crouchBlock },
            { 48,      airBlock },
            { 49,          flip },
            { 50,    crouchFlip },
            { 51,     jumpSquat },
            { 52,         Throw },

            { 101, crouch},
            { 102, crouch},
            { 103, crouch},
            { 104, walkBack},
            { 105, idle},
            { 106, walkForward},
            { 107, jumpBack},
            { 108, jump},
            { 109, jumpForward}
        };

        greyshirtAnimAction = new Dictionary<Action, int>()
        {
            {crouchL, 2 },
            {crouchM, 12 },
            {crouchH, 22 },

            {standL, 5 },
            {standM, 15 },
            {standH, 25 },

            {jumpL, 8 },
            {jumpM, 18 },
            {jumpH, 28 },

            { oneS, 31   },
            { twoS, 32   },
            { threeS, 33 },
            { fourS, 34  },
            { fiveS, 35  },
            { sixS, 36   },

            { forwardDash, 41},
            { backDash, 42},
            { forwardAirDash, 43},
            { backAirDash, 44},
            { stun, 45 },
            { block, 46 },
            { crouchBlock, 47 },
            { airBlock, 48 },
            { flip, 49 },
            { crouchFlip, 50 },
            { jumpSquat, 51 },
            { Throw, 52 }
        };

        setIds(greyshirtActionIds, greyshirtAnimAction);
        setDelegates();
    }

    //0 - Startup
    //1 - Active
    //2 - Multihit Recovery
    //3 - Recovery

    //Action class:
    //0 = light
    //1 = medium
    //2 = heavy
    //3 = super

    // Standing Light
    private Action standL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3 },// 5 | 2 | 5
        damage = new int[] { 0, 0, 0, 0, 0, 300, 300, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(6, 7, 7, 2, 2, 0), },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 12, 1), new Action.rect(1.5f, 9f, 4, 8, 12, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 0,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, },
        aStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
    };

    // Standing Medium
    private Action standM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3,  },// 8 | 3 | 12
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(6, 10, 5, 5, 3, 0), },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 8, 1), new Action.rect(1.5f, 9f, 4, 8, 8, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 15, 1), new Action.rect(2.5f, 7, 6, 5, 15, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 2,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 12, 25, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
    };

    // Standing Heavy
    private Action standH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 10 | 3 (9) 3 | 17
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(24, 10, 40, 2, 3, 0), },
            {nullBox },
            {nullBox },
            { new Action.rect(24, 10, 40, 2, 3, 0), },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 42, 1), new Action.rect(2, 9f, 7, 8, 42, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 1,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
    };

    // Crouching Light
    private Action crouchL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 4 | 3 | 9
        damage = new int[] { 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4.5f, 3, 7, 2, 3, 0), },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 16, 5), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 0,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, },
        aStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    };

    // Crouching Medium
    private Action crouchM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 5 | 12
        damage = new int[] { 0, 0, 0, 0, 0, 0, 500, 500, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4.25f, 1.25f, 8.5f, 2.5f, 3, 0), },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 6, 0), new Action.rect(0, 0, 0, 0, 0, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(.5f, 4, 5, 8, 17, 0), new Action.rect(3.5f, 1, 8, 2, 17, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 1,
        block = LOW,
        knockdown = NONE,
        actionCancels = new int[] { 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
    };

    // Crouching Heavy
    private Action crouchH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 15 | 6 | 10
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4, 10, 5, 13, 6, 0), new Action.rect(8, 10, 2, 7, 6, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 29, 5), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 4,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 31, 32, 33, 34, 35, 36, 40 },
        gAngle = new int[] { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 },
        gStrength = new float[] { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 },
        aAngle = new int[] { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 },
        aStrength = new float[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
    };

    // Jumping Light
    private Action jumpL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 4 (6) 4 | 9
        damage = new int[] { 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(6, 10, 5, 7, 4, 0), },
            {nullBox },
            {nullBox },
            {nullBox },
            { new Action.rect(6, 10, 5, 7, 4, 0), },
            {nullBox },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 3, 3, 6, 29, 1), new Action.rect(2, 9, 4, 8, 29, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 2,
        block = HIGH,
        knockdown = NONE,
        actionCancels = new int[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45 },
        aStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
    };

    // Jumping Medium
    private Action jumpM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 10 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(-2, 6, 7, 3, 10, 0), new Action.rect(4, 7, 7, 3, 10, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(.5f, 9, 6, 8, 31, 1)},
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 2,
        block = HIGH,
        knockdown = NONE,
        actionCancels = new int[] { 27, 28, 29, 40, 43, 44 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    };

    // Jumping Heavy
    private Action jumpH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 13 | 7 | 19
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4, 5, 5, 10, 7, 0), new Action.rect(6, 1, 5, 5, 7, 1),  new Action.rect(9, -2, 5, 5, 7, 2), },
            {nullBox, nullBox, nullBox },
            {nullBox, nullBox, nullBox },
            {nullBox, nullBox, nullBox },
            {nullBox, nullBox, nullBox },
            {nullBox, nullBox, nullBox },
            {nullBox, nullBox, nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 3, 3, 6, 39, 1), new Action.rect(2, 9, 4, 8, 39, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 3,
        block = HIGH,
        knockdown = SOFTKD,
        actionCancels = new int[] { 40, 43, 44 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -45, -45, -45, -45, -45, -45, -45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    };

    // One Super
    private Action oneS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 11 | 2 (6) 2 (6) 2 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 550, 550, 0, 0, 0, 0, 0, 0, 550, 550, 0, 0, 0, 0, 0, 0, 550, 550, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(7, 2.5f, 7, 5, 2, 0), },
            {nullBox },
            { new Action.rect(7, 2.5f, 7, 5, 2, 0), },
            {nullBox },
            { new Action.rect(7, 2.5f, 7, 5, 2, 0), },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 42, 1), new Action.rect(1.5f, 9f, 4, 8, 42, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 0,
        block = LOW,
        knockdown = SOFTKD,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f },
    };

    // Two Super
    private Action twoS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 10 | 18
        damage = new int[] { 0, 0, 0, 0, 0, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4.25f, 1.25f, 8.5f, 2.5f, 10, 0), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 5, 0), new Action.rect(0, 0, 0, 0, 0, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(0, 3, 8, 6, 10, 0), new Action.rect(3.5f, 1, 8, 2, 10, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(1, 4, 8, 8, 16, 0), new Action.rect(0, 0, 0, 0, 0, 1),},
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hMovement = new float[] { 0, 0, .5f, 1, 1, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1, 1, .5f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        block = LOW,
        knockdown = NONE,
        actionCancels = new int[] { 33, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
    };

    // Three Super
    private Action threeS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 11 | 3 | 18
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(7, 8, 10, 6, 3, 0), },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 32, 1), new Action.rect(2, 7, 6, 7, 32, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 4,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 32, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
    };

    // Four Super
    private Action fourS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 16 | 6 | 21
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 800, 800, 800, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4, 5, 5, 10, 6, 0), new Action.rect(2, 10, 8, 4, 6, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 16, 0), new Action.rect(0, 0, 0, 0, 0, 1)},
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 6, 0), new Action.rect(2.5f, 7, 6, 5, 6, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 21, 0), new Action.rect(1.5f, 9f, 4, 8, 21, 1), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hMovement = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 2,
        block = MID,
        knockdown = SOFTKD,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 60, 60, 60, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 60, 60, 60, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    };

    // Five Super
    private Action fiveS = new Action()
    {
        tier = 3,
        frames = new int[] { 1 },//  |  | 
        damage = new int[] { 1 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {
            
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 0,
        block = MID,
        knockdown = HARDKD,
        actionCancels = new int[] { },
        gAngle = new int[] { 0 },
        gStrength = new float[] { 0 },
        aAngle = new int[] { 0 },
        aStrength = new float[] { 0 },
    };

    // Six Super
    private Action sixS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 17 | 4 | 18
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(8, 5, 4, 8, 4, 0), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 17, 1), new Action.rect(2.5f, 7, 6, 7, 17, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 4, 1), new Action.rect(4, 6, 6, 7, 4, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 18, 1), new Action.rect(4, 6, 6, 7, 18, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        block = HIGH,
        knockdown = HARDKD,
        actionCancels = new int[] { 32, 33, 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -90, -90, -90, -90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    };

    // Throw
    private Action Throw = new Action()
    {
        tier = 2,
        frames = new int[] { 1 },
        damage = new int[] { 1500 },
        level = 5,
        p1scaling = .5f,
        block = UNBLOCKABLE,
        knockdown = HARDKD,
        actionCancels = new int[] {  },
        gAngle = new int[] { 0 },
        gStrength = new float[] { 0 },
        aAngle = new int[] { 0 },
        aStrength = new float[] { 0 },
        airOK = false,
    };

    // Jump Squat
    private Action jumpSquat = new Action()
    {
        frames = new int[] { 0, 0, 0 },
        actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 3, 1), new Action.rect(0.5f, 9f, 4, 8, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };

    // Turns
    private Action flip = new Action()
    {
        frames = new int[] { 0, 0, 0 },
        actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 3, 1), new Action.rect(0.5f, 9f, 4, 8, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };

    // crouch Turns
    private Action crouchFlip = new Action()
    {
        frames = new int[] { 0, 0, 0, },
        actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 3, 5), },
            {nullBox },
            {nullBox },
        },
    };

    // Back Dash
    private Action backDash = new Action()
    {
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 5, 1), new Action.rect(0, 0, 0, 0, 5, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {new Action.rect(0.5f, 2.5f, 6.5f, 5, 17, 1), new Action.rect(-1.5f, 9f, 4, 8, 17, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, }
        },
    };

    // Forward Dash
    private Action forwardDash = new Action()
    {
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        actionCancels = new int[] {  },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 2.5f, 6, 5, 14, 1), new Action.rect(2.5f, 7, 5, 6, 14, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
        },
    };

    // Stun
    private Action stun = new Action()
    {
        frames = new int[] { 3 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 6, 4, 12, 40, 1)},
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
    };

    // Block
    private Action block = new Action() {
        frames = new int[] { 0 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 1, 1), new Action.rect(1.5f, 9f, 4, 8, 1, 2), },
        }
    };

    // Crouching Block
    private Action crouchBlock = new Action() {
        frames = new int[] { 0 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 1, 5), },
        }
    };

    // Air Block
    private Action airBlock = new Action() {
        frames = new int[] { 0 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 6, 4, 12, 10, 10), },
        }
    };

    // Air Dash
    private Action forwardAirDash = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        actionCancels = new int[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 2.5f, 6, 5, 14, 1), new Action.rect(2.5f, 7, 5, 6, 14, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
        },
    };
    private Action backAirDash = new Action()
    {
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 2, 1), new Action.rect(0, 0, 0, 0, 2, 2), },
            {nullBox, nullBox, },
            { new Action.rect(0, 2.5f, 6, 5, 20, 1), new Action.rect(-2.5f, 7, 5, 6, 20, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, }
        },
    };















    private Action crouch = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 4.5f, 7, 9, 40, 5), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
    };
    private Action walkBack = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 20, 3), new Action.rect(1.5f, 9f, 4, 8, 20, 4), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
    };
    private Action idle = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 40, 1), new Action.rect(1.5f, 9f, 4, 8, 40, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };
    private Action walkForward = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 25, 6), new Action.rect(1.5f, 9f, 4, 8, 25, 7), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
    };
    private Action jumpBack = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(3, 3, 3, 6, 10, 8), new Action.rect(1, 9, 3, 6, 10, 9), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };
    private Action jump = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 6, 4, 12, 10, 10), },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox },
            {nullBox }
        },
    };
    private Action jumpForward = new Action()
    {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 3, 3, 6, 10, 11), new Action.rect(2, 9, 4, 8, 10, 12), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };

    public override void setDelegates()
    {
        onAdvancedActionCallbacks = new OnAdvancedAction[]
        {
             new OnAdvancedAction(advDashForward),
             new OnAdvancedAction(advDashBack),
             new OnAdvancedAction(advAirDashForward),
             new OnAdvancedAction(advAirDashBack),
             new OnAdvancedAction(advStun),
             new OnAdvancedAction(advBlock),
             new OnAdvancedAction(advCrouchBlock),
             new OnAdvancedAction(advAirBlock),
             new OnAdvancedAction(advFlip),
             new OnAdvancedAction(advCrouchFlip),
             new OnAdvancedAction(advJumpSquat),
             new OnAdvancedAction(advThrow)
        };
    }


    // ADVANCED ACTIONS
    public void advDashForward(PlayerScript player)
    {
        player.hVelocity = forwardSpeed * dashForwardSpeed;
        if (infiniteDashForward)
            player.checkDashEnd();
    }

    public void advDashBack(PlayerScript player)
    {
        player.hVelocity = backwardSpeed * dashBackSpeed;
    }

    public void advAirDashForward(PlayerScript player)
    {
        player.vVelocity = 0;
        player.hVelocity = forwardSpeed * airDashForwardSpeed;
    }

    public void advAirDashBack(PlayerScript player)
    {
        player.vVelocity = 0;
        player.hVelocity = backwardSpeed * airDashBackSpeed;
    }

    public void advStun(PlayerScript player)
    {
        player.stunTimer--;
        if (player.stunTimer <= 0)
        {
            player.ActionEnd();
        }
    }

    public void advBlock(PlayerScript player)
    {
        player.hVelocity = 0;
        player.checkBlockEnd();
    }

    public void advCrouchBlock(PlayerScript player)
    {
        player.hVelocity = 0;
        player.checkBlockEnd();
    }

    public void advAirBlock(PlayerScript player)
    {
        player.hVelocity = 0;
        player.vVelocity = 0;
        player.checkBlockEnd();
    }

    public void advFlip(PlayerScript player)
    {

    }

    public void advCrouchFlip(PlayerScript player)
    {

    }

    public void advJumpSquat(PlayerScript player)
    {
        player.hVelocity = 0;
    }

    public void advThrow(PlayerScript player)
    {
        Debug.Log("Throw Executed");
        player.throwThatMfOtherPlayer();
    }

    public override void setStats()
    {
        forwardSpeed = 0.2f;
        backwardSpeed = -0.2f;
        jumpDirectionSpeed = 1.25f;
        dashForwardSpeed = 3f;
        dashBackSpeed = 2.9f;
        airDashForwardSpeed = 3.1f;
        airDashBackSpeed = 2.5f;
        gravity = -0.05f;
        maxHealth = 10000;
        infiniteDashForward = false;
    }

    public override float getAttackMovementHorizontal(int attackState)
    {
        switch (attackState)
        {
            //Crouching Light
            case 1:
                return 1;
            case 2:
                return 1;
            case 3:
                return 1;

            //Crouching Medium
            case 11:
                return 1;
            case 12:
                return 1;
            case 13:
                return 1;

            //Crouching Heavy
            case 21:
                return 0;
            case 22:
                return 0;
            case 23:
                return 0;

            //Standing Light
            case 4:
                return 1.5f;
            case 5:
                return 1.5f;
            case 6:
                return 1.5f;

            //Standing Medium
            case 14:
                return 1;
            case 15:
                return 1;
            case 16:
                return 1;

            //Standing Heavy
            case 24:
                return 0;
            case 25:
                return 0;
            case 26:
                return 0;

            //Jumping Light
            case 7:
                return 1.5f;
            case 8:
                return 1.5f;
            case 9:
                return 1.5f;

            //Jumping Medium
            case 17:
                return .5f;
            case 18:
                return .5f;
            case 19:
                return .5f;

            //Jumping Heavy
            case 27:
                return 0;
            case 28:
                return 0;
            case 29:
                return 0;

            default:
                return 0;

        }
    }

    public override float getAttackMovementVertical(int attackState)
    {
        switch (attackState)
        {
            //Jumping Light
            case 7:
                return 1.5f;
            case 8:
                return 1.5f;
            case 9:
                return 1.5f;

            //Jumping Medium
            case 17:
                return .5f;
            case 18:
                return .5f;
            case 19:
                return .5f;

            //Jumping Heavy
            case 27:
                return 0f;
            case 28:
                return 0f;
            case 29:
                return 0f;

            default:
                return 0;

        }
    }
}