using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyshirtBehaviours : Behaviors
{
    const int LOW = 1;//must be crouchblocked
    const int MID = 2;//can be crouch or stand blocked
    const int HIGH = 3;//must be stand blocked
    const int UNBLOCKABLE = 4;//cannot be blocked

    const int NONE = 0;//no knockdown (follows hitstun numbers)
    const int SOFTKD = 1;//soft knockdown (techable)
    const int HARDKD = 1;//hard knockdown (untechable for 20 frames, OTG possible)
    const int SOFTGB = 1;//soft ground bounce (ground bounce with soft knockdown)
    const int HARDGB = 1;//hard ground bounce (ground bounce with hard knockdown)
    const int SOFTWB = 1;//soft wall bounce (wall bounce with soft knockdown)
    const int HARDWB = 1;//hard wall bounce (wall bounce with hard knockdown)
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
            { 50,    crouchFlip }
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

            {forwardDash, 41},
            {backDash, 42},
            {forwardAirDash, 43},
            {backAirDash, 44},
            {stun, 45 },
            {block, 46 },
            {crouchBlock, 47 },
            {airBlock, 48 },
            {flip, 49 },
            {crouchFlip, 50 },
        };

        setIds(greyshirtActionIds, greyshirtAnimAction);
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
        frames = new int[] { 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 4 | 3 | 6
        damage = new int[] { 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 0,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 5, 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = 0,
        gStrength = 1,
        aAngle = 30,
        aStrength = 1
    };

    // Standing Medium
    private Action standM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3,  },// 8 | 3 | 12
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 2,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 12, 25, 31, 32, 33, 34, 35, 36 },
        gAngle = 0,
        gStrength = 2,
        aAngle = 30,
        aStrength = 2
    };

    // Standing Heavy
    private Action standH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 10 | 3 (9) 3 | 17
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 1,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle = 0,
        gStrength = 4,
        aAngle = 30,
        aStrength = 4
    };

    // Crouching Light
    private Action crouchL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },// 4 | 3 | 9
        damage = new int[] { 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 0,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = 0,
        gStrength = 1,
        aAngle = 30,
        aStrength = 1
    };

    // Crouching Medium
    private Action crouchM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 5 | 12
        damage = new int[] { 0, 0, 0, 0, 0, 0, 500, 500, 500, 500, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 1,
        block = LOW,
        knockdown = NONE,
        actionCancels = new int[] { 15, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = 0,
        gStrength = 2,
        aAngle = 30,
        aStrength = 2
    };

    // Crouching Heavy
    private Action crouchH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 15 | 6 | 10
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 4,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 31, 32, 33, 34, 35, 36, 40 },
        gAngle = 80,
        gStrength = 4,
        aAngle = 80,
        aStrength = 5
    };

    // Jumping Light
    private Action jumpL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 4 (6) 4 | 9
        damage = new int[] { 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 2,
        block = HIGH,
        knockdown = NONE,
        actionCancels = new int[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
        gAngle = 0,
        gStrength = 1,
        aAngle = 30,
        aStrength = 1
    };

    // Jumping Medium
    private Action jumpM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 10 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 2,
        block = HIGH,
        knockdown = NONE,
        actionCancels = new int[] { 27, 28, 29, 40, 43, 44 },
        gAngle = 0,
        gStrength = 1,
        aAngle = 30,
        aStrength = 1
    };

    // Jumping Heavy
    private Action jumpH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 13 | 7 | 19
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 3,
        block = HIGH,
        knockdown = SOFTKD,
        actionCancels = new int[] {  },
        gAngle = 0,
        gStrength = 2,
        aAngle = -90,
        aStrength = 6
    };

    // One Super
    private Action oneS = new Action()
    {
        tier = 3,
        frames = new int[] {  },// 11 | 2 (1) 2 (1) 2 | 13
        damage = new int[] {  },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 0,
        block = LOW,
        knockdown = SOFTKD,
        actionCancels = new int[] { 35 },
        gAngle = 0,
        gStrength = 1,
        aAngle = 0,
        aStrength = 0
    };

    // Two Super
    private Action twoS = new Action()
    {
        tier = 3,
        frames = new int[] { },// 5 | 10 | 18
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = LOW,
        knockdown = NONE,
        actionCancels = new int[] { 35, 41, 42 },
        gAngle = 0,
        gStrength = 2,
        aAngle = 30,
        aStrength = 2
    };

    // Three Super
    private Action threeS = new Action()
    {
        tier = 3,
        frames = new int[] { },// 11 | 3 | 18
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 4,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 35 },
        gAngle = 0,
        gStrength = 1,
        aAngle = 30,
        aStrength = 1
    };

    // Four Super
    private Action fourS = new Action()
    {
        tier = 3,
        frames = new int[] { },// 16 | 6 | 21
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 2,
        block = MID,
        knockdown = SOFTKD,
        actionCancels = new int[] { 35 },
        gAngle = 45,
        gStrength = 1,
        aAngle = 45,
        aStrength = 1
    };

    // Five Super
    private Action fiveS = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 0,
        block = MID,
        knockdown = HARDKD,
        actionCancels = new int[] { },
        gAngle = 0,
        gStrength = 5,
        aAngle = 0,
        aStrength = 0
    };

    // Six Super
    private Action sixS = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = HIGH,
        knockdown = HARDKD,
        actionCancels = new int[] { 35 },
        gAngle = 60,
        gStrength = 8,
        aAngle = 60,
        aStrength = 8
    };

    // Rekka Bridge
    private Action rekkaBridge = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 35 },
        gAngle = 60,
        gStrength = 8,
        aAngle = 60,
        aStrength = 8
    };

    // L Ender
    private Action rekkaL = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = LOW,
        knockdown = NONE,
        actionCancels = new int[] { 35 },
        gAngle = 60,
        gStrength = 8,
        aAngle = 60,
        aStrength = 8
    };

    // M Ender
    private Action rekkaM = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = MID,
        knockdown = SOFTWB,
        actionCancels = new int[] { 35 },
        gAngle = 60,
        gStrength = 8,
        aAngle = 60,
        aStrength = 8
    };

    // H Ender
    private Action rekkaH = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = MID,
        knockdown = HARDWB,
        actionCancels = new int[] { 35 },
        gAngle = 60,
        gStrength = 8,
        aAngle = 60,
        aStrength = 8
    };

    // S Ender
    private Action rekkaS = new Action()
    {
        tier = 3,
        frames = new int[] { },//  |  | 
        damage = new int[] {  },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 0,
        //block = ;
        //knockdown = ;
        actionCancels = new int[] { 35 },
        gAngle = 0,
        gStrength = 0,
        aAngle = 0,
        aStrength = 0
    };

    // Throw
    private Action Throw = new Action()
    {
        tier = 2,
        frames = new int[] { },//  |  | 
        damage = new int[] { },
        hitboxData = new Action.rect[,]
        {

        },
        hurtboxData = new Action.rect[,]
        {

        },
        level = 5,
        block = UNBLOCKABLE,
        knockdown = HARDKD,
        actionCancels = new int[] { 1, 2 },
        gAngle = 30,
        gStrength = 10,
        aAngle = 0,
        aStrength = 0
    };

    // Turns
    private Action flip = new Action() { frames = new int[] { 0, 0, 0 }, actionCancels = new int[] { } };

    // crouch Turns
    private Action crouchFlip = new Action() { frames = new int[] { 0, 0, 0, }, actionCancels = new int[] { } };

    // Back Dash
    private Action backDash = new Action() { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, actionCancels = new int[] { } };

    // Forward Dash
    private Action forwardDash = new Action() { frames = new int[] { 3 }, actionCancels = new int[] { 40 }, infinite = true };

    // Stun
    private Action stun = new Action() { frames = new int[] { 3 }, actionCancels = new int[] { } };

    // Block
    private Action block = new Action() { frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Crouching Block
    private Action crouchBlock = new Action() { frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Air Block
    private Action airBlock = new Action() { frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Air Dash
    private Action forwardAirDash = new Action() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, actionCancels = new int[] { } };
    private Action backAirDash = new Action() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, actionCancels = new int[] { } };
}