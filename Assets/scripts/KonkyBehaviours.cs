using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors
{
    const int LOW = 1; //must be crouchblocked
    const int MID = 2; //can be crouch or stand blocked
    const int HIGH = 3; //must be stand blocked
    const int UNBLOCKABLE = 4; //cannot be blocked

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

    IDictionary<int, Action> konkyActionIds;
    IDictionary<Action, int> konkyAnimAction;

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

    public KonkyBehaviours()
    {
        konkyActionIds = new Dictionary<int, Action>()
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

            { 6,  standL   },
            { 16, forwardM },
            { 26, standH   },

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
            { 37, sixS  },
            { 38, sixS  },
            { 39, sixS  },


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
            { 53,     knockdown },
            { 54, knockdownFall },
            { 55,          grab },
            { 56,    wallbounce },
            { 57,  groundbounce },
            { 58,       victory },
            { 59,        defeat },

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

        konkyAnimAction = new Dictionary<Action, int>()
        {
            { crouchL, 2 },
            { crouchM, 12 },
            { crouchH, 22 },

            { standL, 5 },
            { standM, 15 },
            { forwardM, 16 },
            { standH, 25 },

            { jumpL, 8 },
            { jumpM, 18 },
            { jumpH, 28 },

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
            { Throw, 52 },
            { knockdown, 53 },
            { knockdownFall, 54 },
            { grab, 55 },
            { wallbounce, 56 },
            { groundbounce, 57 },
            { victory, 58 },
            { defeat, 59 }
        };

        setIds(konkyActionIds, konkyAnimAction);
        setDelegates();
    }

    //0 - Startup
    //1 - Active
    //2 - Multihit Recovery
    //3 - Recovery
    //4 - Buffer Window

    //Action class:
    //0 = light
    //1 = medium
    //2 = heavy
    //3 = special
    //4 = super

    // Standing Light
    private Action standL = new Action()
    {
        tier = 0,
        frames = new int[] { 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 3 | 9
        damage = new int[] { 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxFrames = 3,
        hurtboxFrames = 17,
        hitboxData = new Action.rect[,]
        {
            { new Action.rect( 6, 3, 5, 2, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 12, 0), new Action.rect(1.5f, 9f, 4, 8, 12, 1), },
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
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 1,
        p1scaling = .9f,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, .5f, .5f, .5f, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        aStrength = new float[] { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitSound = AudioManager.Sound.LIGHT,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Standing Medium
    private Action standM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 3 | 11
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5, 9, 6, 2, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 23, 1), new Action.rect(1.5f, 9f, 4, 8, 23, 2), },
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
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 2,
        p1scaling = 1f,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 12, 22, 25, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        hitSound = AudioManager.Sound.MEDIUM,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Standing Heavy
    private Action standH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 12 | 4 | 12
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(8, 7.5f, 7.5f, 2, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1.5f, 2.5f, 6.5f, 5, 12, 1), new Action.rect(2.5f, 9f, 4, 8, 12, 2), },
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
            { new Action.rect(4, 2.5f, 6.5f, 5, 10, 1), new Action.rect(3, 9f, 4, 8, 10, 9), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(1.5f, 2.5f, 6.5f, 5, 6, 1), new Action.rect(2.5f, 9f, 4, 8, 6, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 1,
        p1scaling = 1f,
        block = MID,
        knockdown = SOFTWB,
        actionCancels = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitSound = AudioManager.Sound.HEAVY,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Crouching Light
    private Action crouchL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 3 | 10
        damage = new int[] { 0, 0, 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(3, 0.5f, 8, 1, 3, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 5, 5, 19, 1), new Action.rect(-1.5f, 5.5f, 4, 4, 19, 2), },
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
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 1,
        p1scaling = .9f,
        block = LOW,
        knockdown = NONE,
        actionCancels = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f },
        hitSound = AudioManager.Sound.LIGHT,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Crouching Medium
    private Action crouchM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 4, 4, 4, 4, 4, 1, 2, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 4 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 250, 250, 250, 250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(2, 6, 8, 7, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            { nullBox, },
            { new Action.rect(2, 6, 8, 7, 2, 0), },
            { nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2, 7, 4, 24, 5), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 2,
        p1scaling = 1f,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
        aStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
        hitSound = AudioManager.Sound.MEDIUM,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Crouching Heavy
    private Action crouchH = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 2 (1) 5 | 24
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 700, 700, 0, 700, 700, 700, 700, 700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5.5f, 3, 4, 5, 2, 0), nullBox,  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, nullBox, },
            { new Action.rect(6, 9.5f, 4, 14, 5, 0), new Action.rect(9, 10, 3, 8, 5, 1), }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 11, 2), nullBox },
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
            { new Action.rect(2, 2.5f, 8, 5, 27, 2), new Action.rect(3.5f, 9f, 4, 8, 27, 3), },
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
            {new Action.rect(0.5f, 4, 7, 8, 4, 2), nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        p1scaling = 1f,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 31, 32, 33, 34, 35, 36, 40 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 0, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 9, 9, 9, 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 0, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 9, 9, 9, 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitSound = AudioManager.Sound.HEAVY,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Jumping Light
    private Action jumpL = new Action()
    {
        tier = 0,
        frames = new int[] { 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 5 | 13
        damage = new int[] { 0, 0, 0, 0, 0, 300, 300, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(3.5f, 6, 9, 3, 5, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1, 9, 4, 6, 23, 1),  new Action.rect(3, 6, 8, 2, 5, 0),},
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
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 3,
        p1scaling = .8f,
        block = HIGH,
        knockdown = NONE,
        actionCancels = new int[] { 17, 18, 19, 27, 28, 29, 34, 36, 40, 43, 44 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45 },
        aStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        hitSound = AudioManager.Sound.LIGHT,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Jumping Medium
    private Action jumpM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 4, 4, 4, 4, 4, 1, 2, 1, 2, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 6 | 17
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 400, 400, 400, 400, 400, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(6, 8, 6, 7, 2, 0), }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            { new Action.rect(6, 8, 6, 7, 2, 1), },
            {nullBox, },
            { new Action.rect(6, 8, 6, 7, 2, 0), },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-0.5f, 5, 5, 6, 30, 1), new Action.rect(4, 9, 5, 5, 30, 2), },
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox},
            {nullBox, nullBox}
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 4,
        p1scaling = .8f,
        block = HIGH,
        knockdown = NONE,
        actionCancels = new int[] { 27, 28, 29, 34, 36, 40, 43, 44 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitSound = AudioManager.Sound.MEDIUM,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Jumping Heavy
    private Action jumpH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 10 | 4 | 23
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5, 9, 7, 9, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(1.5f, 8.5f, 6, 12, 10, 2), nullBox,},
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, nullBox },
            {nullBox, new Action.rect(5, 4, 6, 4, 17, 2), },
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
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        p1scaling = .8f,
        block = HIGH,
        knockdown = SOFTKD,
        actionCancels = new int[] { 34, 36, 40, 43, 44 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitSound = AudioManager.Sound.HEAVY,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Forward Medium
    private Action forwardM = new Action()
    {
        tier = 1,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 13 | 2 | 19
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(8, 2, 12, 4, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-0.5f, 7, 6, 14, 13, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect(4, 4, 15, 8, 21, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 1,
        p1scaling = 1.1f,
        block = HIGH,
        knockdown = SOFTGB,
        actionCancels = new int[] { 22, 25, 31, 32, 33, 34, 35, 36 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        aAngle = new int[] { 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330, 330 },
        aStrength = new float[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
        hitSound = AudioManager.Sound.MEDIUM,
        whiffSound = AudioManager.Sound.WHIFF,
    };


    // One Super
    private Action oneS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 6 | 30
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1000, 1000, 1000, 1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect( 2.5f, 5, 5, 5, 6, 0), new Action.rect( 7.5f, 10, 5, 5, 6, 1), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect( .5f, 6, 9, 9, 8, 2 ), },//startup
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect( 0, 0, 0, 0, 6, 2 ), },//active
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect( .5f, 6, 9, 9, 30, 2 ), },//recovery
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect(1, 6, 4, 12, 50, 0), },//fall
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
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 1.5f, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 1.5f, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        p1scaling = 1,
        block = MID,
        knockdown = SOFTWB,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = true,
        hitSound = AudioManager.Sound.DP,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Two Super
    private Action twoS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 11 | 1 | 25
        damage = new int[] { 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(3, 0.5f, 8, 1, 2, 0),  },
            {nullBox, },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-0.5f, 7, 6, 14, 12, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect(4, 4, 15, 8, 19, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            { new Action.rect(-0.5f, 7, 6, 14, 6, 1), },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 4,
        p1scaling = 1f,
        block = MID,
        knockdown = NONE,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = false,
        nonPushBlockable = true,
        projectileLocation = new Vector2(7, 0),
        projectileSpeed = 2,
        projectileStrength = 500,
        //projectileHits = 1,
        hitSound = AudioManager.Sound.FIREBALL,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Three Super
    private Action threeS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 1, 1, 2, 2, 1, 1, 2, 2, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 2 (2) 2 (2) 2 (2) 2 (2) 2 (2) 2 (8) 3 | 21
        damage = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 0, 0, 1200, 1200, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(5, 9, 6, 2, 2, 0),  },
            {nullBox, },
            { new Action.rect(8.5f, 9, 12, 2, 3, 0),  },
            {nullBox, },
            {nullBox, },
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
            { new Action.rect(1.5f, 2.5f, 6.5f, 5, 30, 1), new Action.rect(2.5f, 9f, 4, 8, 30, 2), },
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
            { new Action.rect(7, 2.5f, 8, 5, 21, 1), new Action.rect(8, 8, 4, 8, 21, 2), },
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
            { new Action.rect(2.5f, 2.5f, 6.5f, 5, 3, 1), new Action.rect(3.5f, 9f, 4, 8, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
        },
        hMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 4,
        p1scaling = 1,
        block = MID,
        knockdown = HARDKD,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, .75f, .75f, 0, 0, .75f, .75f, 0, 0, .75f, .75f, 0, 0, .75f, .75f, 0, 0, .75f, .75f, 0, 0, .75f, .75f, 0, 0, .75f, .75f, 0, 0, 0, 0, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 0, 0, 45, 45, 0, 0, 45, 45, 0, 0, 45, 45, 0, 0, 45, 45, 0, 0, 45, 45, 0, 0, 45, 45, 0, 0, 0, 0, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = false,
        hitSound = AudioManager.Sound.MEDIUM,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Four Super
    private Action fourS = new Action()
    {
        tier = 3,
        frames = new int[] { 0, 4, 4, 4, 4, 4, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage = new int[] { 0, 0, 0, 0, 0, 0, 400, 400, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 400, 400, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4.5f, 4.5f, 9, 9, 3, 0),  },
            {nullBox, },
            {nullBox, },
            { new Action.rect(4.5f, 4.5f, 9, 9, 3, 0),  },
            {nullBox, },
            {nullBox, },
            { new Action.rect(5, 9, 7, 9, 4, 0),  },
            {nullBox, },
            {nullBox, },
            {nullBox, }

        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 31, 0), new Action.rect(0, 0, 0, 0, 0, 1), },
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
            {nullBox, new Action.rect(5, 4, 6, 4, 17, 2), },
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
        hMovement = new float[] { 1.1f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 1.1f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 3,
        p1scaling = 1f,
        block = MID,
        knockdown = SOFTKD,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -45, -45, -45, -45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -45, -45, -45, -45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = true,
        hitSound = AudioManager.Sound.MEDIUM,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Five Super
    private Action fiveS = new Action()
    {
        tier = 4,
        frames = new int[] { 0, 0, 1, 2, 1, 2, 1, 2, 1, 2, 5, 2, 5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },// 4 | 12 | 55
        damage = new int[] { 0, 0, 800, 800, 800, 800, 800, 800, 800, 800, 800, 800, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4, 10, 4, 12, 2, 0), },
            { nullBox, },
            { new Action.rect(4, 10, 4, 12, 2, 0), },
            { nullBox, },
            { new Action.rect(4, 10, 4, 12, 2, 0), },
            { nullBox, },
            { new Action.rect(4, 10, 4, 12, 2, 0), },
            { nullBox, },
            { new Action.rect(4, 10, 4, 12, 2, 0), },
            { nullBox, },
            { new Action.rect(4, 10, 4, 12, 2, 0), },
            { nullBox, },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 14, 0), },
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
            { new Action.rect(1, 6, 4, 12, 40, 0), },
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
            { new Action.rect(0.5f, 4, 7, 8, 4, 0), },
            {nullBox },
            {nullBox },
            {nullBox },
        },
        hMovement = new float[] { 0, 0, .25f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        vMovement = new float[] { 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -2.5f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        p1scaling = 1.5f,
        block = MID,
        knockdown = HARDKD,
        super = true,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 85, 85, 85, 85, 85, 85, 85, 85, 85, 85, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 85, 85, 85, 85, 85, 85, 85, 85, 85, 85, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 5, 5, 4, 4, 4, 4, 4, 4, 30, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = false,
        hitSound = AudioManager.Sound.SUPER,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Six Super
    private Action sixS = new Action()
    {
        tier = 3,
        frames = new int[] { 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 4 | 10 (8) 4 | 70
        damage = new int[] { 0, 0, 0, 0, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 800, 800, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(4, 10, 4, 12, 10, 0), },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { nullBox, },
            { new Action.rect(5, 9, 7, 9, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            {nullBox, },
            {nullBox, },
            {nullBox, }
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 14, 0), },
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
            { new Action.rect(0, 0, 0, 0, 12, 0), },
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
            { new Action.rect(1, 6, 4, 12, 70, 0), },
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
        },
        hMovement = new float[] { 0, 0, 0, 0, .2f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        vMovement = new float[] { 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        level = 5,
        p1scaling = 1f,
        block = MID,
        knockdown = SOFTKD,
        actionCancels = new int[] { 35 },
        gAngle = new int[] { 0, 0, 0, 0, 90, 90, 90, 90, 90, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, -80, -80, -80, -80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 90, 90, 90, 90, 90, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, -80, -80, -80, -80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = true,
        hitSound = AudioManager.Sound.DP,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Throw
    private Action Throw = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage = new int[] { 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 20, 120, 3, 20), },
            {nullBox },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
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
        },
        level = 5,
        p1scaling = .5f,
        block = UNBLOCKABLE,
        knockdown = SOFTWB,
        actionCancels = new int[] { },
        gAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        gStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aAngle = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        aStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        airOK = true,
        hitSound = AudioManager.Sound.HEAVY,
        whiffSound = AudioManager.Sound.WHIFF,
    };

    // Throw
    private Action grab = new Action()
    {
        tier = 2,
        frames = new int[] { 1, 1 },
        damage = new int[] { 0, 0 },
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(2, 6, 2, 12, 2, 6), },
            {nullBox },
        },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 2, 6), },
            {nullBox },

        },
        level = 5,
        p1scaling = .5f,
        block = UNBLOCKABLE,
        knockdown = NONE,
        actionCancels = new int[] {  },
        gAngle = new int[] { 0, 0 },
        gStrength = new float[] { 0, 0 },
        aAngle = new int[] { 0, 0 },
        aStrength = new float[] { 0, 0  },
        airOK = false,
    };


    // Jump Squat
    private Action jumpSquat = new Action() { frames = new int[] { 0, 0}, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 2, 1), new Action.rect(0.5f, 9f, 4, 8, 2, 2), },
            {nullBox, nullBox },
        },
    };

    // Turns
    private Action flip = new Action() { frames = new int[] { 0, 0, 0, }, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 3, 1), new Action.rect(0.5f, 9f, 4, 8, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox }
        },
    };

    // crouch Turns
    private Action crouchFlip = new Action() { frames = new int[] { 0, 0, 0, }, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 3, 5), },
            {nullBox },
            {nullBox },
        },
    };

    // Back Dash
    private Action backDash = new Action() { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 10, 1), new Action.rect(0, 0, 0, 0, 10, 2), },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {nullBox, nullBox, },
            {new Action.rect(0.5f, 2.5f, 6.5f, 5, 10, 1), new Action.rect(-1.5f, 9f, 4, 8, 10, 2), },
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
    private Action forwardDash = new Action() { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 }, actionCancels = new int[] { 40 }, infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(-4, 2.5f, 8, 5, 14, 1), new Action.rect(2.5f, 7, 5, 8, 14, 2), },
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
            { new Action.rect(-4, 2.5f, 8, 5, 1000, 1), new Action.rect(2.5f, 7, 5, 8, 1000, 2), }
        },
    };

    // Stun
    private Action stun = new Action() { frames = new int[] { 3 }, actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 6, 4, 12, 40, 9)},
        },
    };

    private Action knockdown = new Action()
    {
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        actionCancels = new int[] { },
        infinite = false,
        hurtboxData = new Action.rect[,]
       {
            { new Action.rect(0, 0, 0, 0, 50, 1)},
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

       },
    };

    private Action knockdownFall = new Action()
    {
        frames = new int[] { 0 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
    {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 2, 9), new Action.rect(1.5f, 9f, 4, 8, 2, 10), },
    }
    };

    private Action wallbounce = new Action()
    {
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        actionCancels = new int[] { },
        infinite = false,
        hurtboxData = new Action.rect[,]
        {
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
        }
    };

    private Action groundbounce = new Action()
    {
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
        actionCancels = new int[] { },
        infinite = false,
        hurtboxData = new Action.rect[,]
    {
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
    }
    };

    private Action victory = new Action()
    {
        frames = new int[] { 0 },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            {nullBox, nullBox }
        },
    };

    private Action defeat = new Action()
    {
        frames = new int[] { 0 },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            {nullBox, nullBox }
        },
    };


    // Block
    private Action block = new Action() {
        frames = new int[] { 0 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 2.5f, 6.5f, 5, 1, 3), new Action.rect(1.5f, 9f, 4, 8, 1, 4), },
        },
        hitSound = AudioManager.Sound.BLOCK
    };

    // Crouching Block
    private Action crouchBlock = new Action() {
        frames = new int[] { 0 },
        actionCancels = new int[] { },
        infinite = true,
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 1, 5), },
        },
        hitSound = AudioManager.Sound.BLOCK
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
    private Action forwardAirDash = new Action() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, }, actionCancels = new int[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
        hurtboxData = new Action.rect[,]
        {
            //{ new Action.rect(2, 7, 14, 4, 15, 1),},
            { new Action.rect(-4, 2.5f, 8, 5, 15, 1), new Action.rect(2.5f, 7, 5, 8, 15, 2), },
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
    private Action backAirDash = new Action() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, actionCancels = new int[] { },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 0, 0, 3, 1), new Action.rect(0, 0, 0, 0, 3, 2), },
            {nullBox, nullBox },
            {nullBox, nullBox },
            { new Action.rect(3, 3, 3, 6, 17, 1), new Action.rect(1, 9, 3, 6, 17, 2), },
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

    private Action crouch = new Action() {
        frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        hurtboxData = new Action.rect[,]
        {
            { new Action.rect(0.5f, 4, 7, 8, 40, 5), },
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

    private Action walkBack = new Action() {
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

    private Action idle = new Action() {
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

    private Action walkForward = new Action() {
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

    private Action jumpBack = new Action() {
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

    private Action jump = new Action() {
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

    private Action jumpForward = new Action() {
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
             new OnAdvancedAction(advJump),
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
             new OnAdvancedAction(advThrow),
             new OnAdvancedAction(advKnockdown),
             new OnAdvancedAction(advKnockdownFall),
             new OnAdvancedAction(advGrab),
             new OnAdvancedAction(advWallbounce),
             new OnAdvancedAction(advGroundbounce),
             new OnAdvancedAction(advVictory),
             new OnAdvancedAction(advDeath)
        };
    }

    // ADVANCED ACTIONS
    public void advJump(PlayerScript player)
    {

    }

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
        if(player.firstStun)
        {
            player.hVelocity = 0;
            player.vVelocity = 0;
            player.firstStun = false;
        }
         
        if(player.shouldWallbounce && (player.transform.position.x <= player.cameraLeft.position.x || player.transform.position.x >= player.cameraRight.position.x))
        {
            player.ActionEnd();
            player.executingAction = 56;
        }
        if (player.shouldGroundbounce && player.transform.position.y <= 0)
        {
            player.ActionEnd();
            player.executingAction = 57;
        }

        player.stunTimer--;
        if (player.stunTimer <= 0)
        {
            player.ActionEnd();
        }
    }

    public void advBlock(PlayerScript player)
    {
        if (player.inputManager.currentInput[7] && !player.otherPlayer.GetComponent<PlayerScript>().behaviors.getAction(player.otherPlayer.GetComponent<PlayerScript>().executingAction).nonPushBlockable)
        {
            player.otherPlayer.GetComponent<PlayerScript>().hKnockback = (player.otherPlayer.GetComponent<PlayerScript>().playerSide ? -2 : 2);
            AudioManager.Instance.PlaySound(AudioManager.Sound.PUSHBLOCK);
        }
        
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
        player.vVelocity = 0;
        
        if (player.actionFrameCounter == 1)
        {
            player.jumpSquated = true;
        }
    }

    public void advThrow(PlayerScript player)
    {
        if (player.currentFrameType == 1 || player.currentFrameType == 5)     
            player.throwThatMfOtherPlayer();
    }

    public void advKnockdown(PlayerScript player)
    {
        player.hVelocity = 0;
        player.hKnockback = 0;
        player.vVelocity = 0;
        player.vKnockback = 0;
    }

    public void advKnockdownFall(PlayerScript player)
    {
        player.hVelocity = 0;
        player.hKnockback = 0;

    }

    public void advGrab(PlayerScript player)
    {
        player.hVelocity = 0;
        if (player.actionFrameCounter > 0)
        {
            if (player.damageDealt && !player.otherPlayer.GetComponent<PlayerScript>().airborn)
            {
                player.ActionEnd();
                player.executingAction = 52;
            }
            else
            {
                player.ActionEnd();
                player.executingAction = 25;
            }
        }
    }

    public void advWallbounce(PlayerScript player)
    {

        if(player.actionFrameCounter == 1)
        {
            player.stored = player.hKnockback;
        }

        if (player.actionFrameCounter == 9)
        {
            player.hKnockback = player.stored * -.5f;
            player.ActionEnd();
            player.executingAction = 54;
        }
    }

    public void advGroundbounce(PlayerScript player)
    {
        if (player.actionFrameCounter == 1)
        {
            player.stored = player.vKnockback * -1;
            player.vKnockback = 0;
            player.hKnockback = 0;
        }

        if (player.actionFrameCounter == 9)
        {
            player.vKnockback = player.stored + 3;
            player.ActionEnd();
            player.executingAction = 54;
        }
    }

    public void advVictory(PlayerScript player)
    {

    }

    public void advDeath(PlayerScript player)
    {

    }


    public override void setStats()
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
        maxMeter = 8000;
        infiniteDashForward = true;
    }

    public override float getAttackMovementHorizontal(int attackState)
    {
        switch (attackState) {
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
                return 0;
            case 5:
                return 0;
            case 6:
                return 0;

            //Standing Medium
            case 14:
                return 1;
            case 15:
                return 1;

            //Forward Medium
            case 16:
                return 1;

            //Standing Heavy
            case 24:
                return 1;
            case 25:
                return 1;
            case 26:
                return 1;

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