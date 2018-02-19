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
       * 7 - Flip
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
            { 47,         flip  }
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
        };

        setIds(konkyActionIds, konkyAnimAction);
    }

    //0 total frames
    //1 recovery frames
    //3 damage
    //4 chip damage DO THIS
    //5 blockstun

    //Action class:
    //0 = light
    //1 = medium
    //2 = heavy
    //3 = super

    // Standing Light
    private Action standL = new Action()
    {
        tier = 0,
        frames = new int[] { 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3 },
        damage = new int[] { 300 },
        level = 0,
        ActionCancels = new int[] { 5, 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Standing Medium
    private Action standM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 600 },
        level       = 1,
        ActionCancels     = new int[] { 12, 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Standing Heavy
    private Action standH = new Action()
    {
        tier = 2,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 900 },
        level       = 2,
        ActionCancels     = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 4,
        aAngle      = 30,
        aStrength   = 4
    };

    // Crouching Light
    private Action crouchL = new Action()
    {
        tier = 0,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 300 },
        level       = 0,
        ActionCancels     = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Crouching Medium
    private Action crouchM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 500 },
        level       = 1,
        ActionCancels     = new int[] { 15, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Crouching Heavy
    private Action crouchH = new Action()
    {
        tier = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage = new int[] { 700, 700 },
        level = 3,
        ActionCancels = new int[] { 31, 32, 33, 34, 35, 36 },
        advCancels = new int[] { 40 },
        gAngle      = 80,
        gStrength   = 4,
        aAngle      = 80,
        aStrength   = 5
    };

    // Jumping Light
    private Action jumpL = new Action()
    {
        tier = 0,
        frames      = new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 300 },
        level       = 2,
        ActionCancels     = new int[] { 17, 18, 19, 27, 28, 29 },
        advCancels = new int[] { 40 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Jumping Medium
    private Action jumpM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 600 },
        level       = 2,
        ActionCancels     = new int[] { 27, 28, 29 },
        advCancels = new int[] { 40 },
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
        advCancels     = new int[] { 40 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = -90,
        aStrength   = 6
    };

    // Forward Medium
    private Action forwardM = new Action()
    {
        tier = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 800 },
        level       = 0,
        ActionCancels     = new int[] { 25, 31, 32, 33, 34, 35, 36 },
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
        ActionCancels     = new int[] { 35 },
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
        ActionCancels     = new int[] { 35,},
        advCancels = new int[] { 41, 42 },
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
        ActionCancels     = new int[] { 35 },
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
        ActionCancels     = new int[] { 35 },
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
        ActionCancels     = new int[] { },
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
        ActionCancels     = new int[] { 35 },
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
        ActionCancels     = new int[] { 1, 2 },
        gAngle      = 30,
        gStrength   = 10,
        aAngle      = 0,
        aStrength   = 0
    };

    // Turns
    private Action flip = new Action()       { frames = new int[] { 0, 0, 0,                                                    }, ActionCancels  = new int[] {       } };

    // Back Dash
    private Action backDash = new Action()    { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, advCancels  = new int[] { 40 } };

    // Forward Dash
    private Action forwardDash = new Action() { frames = new int[] { 0                                                          }, advCancels  = new int[] { 40 }, infinite = true };

    // Stun
    private Action stun = new Action()        { frames = new int[] { 3                                                          }, ActionCancels = new int[] {       } };

    // Block
    private Action block = new Action()       { frames = new int[] { 4                                                          }, ActionCancels = new int[] {       } };

    // Air Dash
    private Action forwardAirDash = new Action() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, ActionCancels = new int[] {       } };
    private Action backAirDash = new Action()    { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, ActionCancels = new int[] {       } };
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