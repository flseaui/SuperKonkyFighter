using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors {

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
            { 51,     jumpSquat }
        };

        konkyAnimAction = new Dictionary<Action, int>()
        {
            {crouchL, 2 },
            {crouchM, 12 },
            {crouchH, 22 },

            {standL, 5 },
            {standM, 15 },
            {forwardM, 16 },
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
            {jumpSquat, 51 }
        };

        setIds(konkyActionIds, konkyAnimAction);
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
        frames = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 3 | 9
        hitboxData = new Action.rect[,]
        {
            { new Action.rect(0, 0, 20, 10, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            { new Action.rect(10, 15, 20, 10, 2, 1) } // Frame 2 - 1 hitbox lasts 2 frames
        },
        damage = new int[] { 300 },
        level = 0,
        actionCancels = new int[] { 5, 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Standing Medium
    private Action standM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 2 | 12
        damage      = new int[] { 600 },
        level       = 2,
        actionCancels     = new int[] { 12, 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Standing Heavy
    private Action standH = new Action()
    {
        tier = 2,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 9 | 4 | 17
        damage      = new int[] { 900 },
        level       = 1,
        actionCancels     = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 4,
        aAngle      = 30,
        aStrength   = 4
    };

    // Crouching Light
    private Action crouchL = new Action()
    {
        tier = 0,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 3 | 10
        damage      = new int[] { 300 },
        level       = 0,
        actionCancels     = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Crouching Medium
    private Action crouchM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 4 | 13
        damage      = new int[] { 500 },
        level       = 1,
        actionCancels     = new int[] { 15, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Crouching Heavy
    private Action crouchH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 8 | 2 (1) 5 | 24
        damage = new int[] { 700, 700 },
        level = 3,
        actionCancels = new int[] { 31, 32, 33, 34, 35, 36, 40 },
        gAngle      = 80,
        gStrength   = 4,
        aAngle      = 80,
        aStrength   = 5
    };

    // Jumping Light
    private Action jumpL = new Action()
    {
        tier = 0,
        frames      = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 5 | 13
        damage      = new int[] { 300 },
        level       = 2,
        actionCancels     = new int[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Jumping Medium
    private Action jumpM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 
        damage      = new int[] { 600 },
        level       = 2,
        actionCancels     = new int[] { 27, 28, 29, 40, 43, 44 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Jumping Heavy
    private Action jumpH = new Action()
    {
        tier = 2,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 900 },
        level       = 3,
        actionCancels     = new int[] { 40, 43, 44 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = -90,
        aStrength   = 6
    };

    // Forward Medium
    private Action forwardM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 13 | 2 | 19
        damage      = new int[] { 800 },
        level       = 0,
        actionCancels     = new int[] { 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 180,
        gStrength   = 2,
        aAngle      = 330,
        aStrength   = 5
    };


    // One Super
    private Action oneS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0 },
        damage      = new int[] { 1200 },
        level       = 0,
        actionCancels     = new int[] { 35 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 0,
        aStrength   = 0
    };

    // Two Super
    private Action twoS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 900 },
        level       = 5,
        actionCancels     = new int[] { 35, 41, 42 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Three Super
    private Action threeS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 800 },
        level       = 4,
        actionCancels     = new int[] { 35 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Four Super
    private Action fourS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3 },
        damage      = new int[] { 400, 400, 600 },
        level       = 2,
        actionCancels     = new int[] { 35 },
        gAngle      = 45,
        gStrength   = 1,
        aAngle      = 45,
        aStrength   = 1
    };

    // Five Super
    private Action fiveS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3 },
        damage      = new int[] { 0 },
        level       = 0,
        actionCancels     = new int[] { },
        gAngle      = 0,
        gStrength   = 5,
        aAngle      = 0,
        aStrength   = 0
    };

    // Six Super
    private Action sixS = new Action()
    {
        tier = 3,
        frames      = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 1500 },
        level       = 5,
        actionCancels     = new int[] { 35 },
        gAngle      = 60,
        gStrength   = 8,
        aAngle      = 60,
        aStrength   = 8
    };

    // Throw
    private Action Throw = new Action()
    {
        tier = 2,
        frames      = new int[] { 0 },
        damage      = new int[] { 1500 },
        level       = 5,
        actionCancels     = new int[] { 1, 2 },
        gAngle      = 30,
        gStrength   = 10,
        aAngle      = 0,
        aStrength   = 0
    };


    // Jump Squat
    private Action jumpSquat = new Action() { frames = new int[] { 0, 0, 0 }, actionCancels = new int[] { } };

    // Turns
    private Action flip = new Action()       { frames = new int[] { 0, 0, 0                                                     }, actionCancels  = new int[] {       } };

    // crouch Turns
    private Action crouchFlip = new Action() { frames = new int[] { 0, 0, 0, }, actionCancels = new int[] { } };

    // Back Dash
    private Action backDash = new Action()    { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, actionCancels  = new int[] {  } };

    // Forward Dash
    private Action forwardDash = new Action() { frames = new int[] { 3                                                         }, actionCancels = new int[] { 40 }, infinite = true };

    // Stun
    private Action stun = new Action()        { frames = new int[] { 3                                                          }, actionCancels = new int[] {       } };

    // Block
    private Action block = new Action()       { frames = new int[] { 0                                                          }, actionCancels = new int[] {       } };

    // Crouching Block
    private Action crouchBlock = new Action() { frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Air Block
    private Action airBlock = new Action()    { frames = new int[] { 0 }, actionCancels = new int[] { } };

    // Air Dash
    private Action forwardAirDash = new Action() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, actionCancels = new int[] {       } };
    private Action backAirDash = new Action()    { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, actionCancels = new int[] {       } };
}
//Level | Hitstop | Hitstun | Counterhit | Blockstun | Scaling
//0     | 8       | 12      | 23         | 9         | .75
//1     | 10      | 14      | 26         | 11        | .8
//2     | 12      | 16      | 28         | 13        | .85
//3     | 14      | 19      | 33         | 16        | .89
//4     | 16      | 21      | 36         | 18        | .92
//5     | 18      | 24      | 40         | 20        | .94

//Whiff Medium/Hard Normal: 1 point
//Whiff Special Move: 4 points
//Connect Light: 4 points
//Connect Medium: 8 points
//Connect Heavy: 12 points
//Connect Special: 8 points
//Land Throw: 5 points
//Have an Action Blocked: 2 points
//Take Damage: 2 points
//Meter Size: 70