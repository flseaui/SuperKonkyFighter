using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors {

    /* 
     * ATTACK ID FORMAT
     * id = numpad + power
     * - light    = + 0
     * - medium   = + 10
     * - heavy    = + 20
     * - special  = + 30
     * - advanced = + 40
     * example: standM = 5 + 10 = 15
     */

    IDictionary<int, Attack> konkyAttackIds;

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

    IDictionary<int, Advanced> konkyAdvancedIds;

    public KonkyBehaviours()
    {
        konkyAttackIds = new Dictionary<int, Attack>()
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
            { 36, sixS   }
        };

        konkyAdvancedIds = new Dictionary<int, Advanced>()
        {
            { 41,    forwardDash},
            { 42,       backDash},
            { 43, forwardAirDash},
            { 44,    backAirDash},
            { 45,          stun },
            { 46,         block },
            { 47,         flip  }
        };

        setIds(konkyAttackIds, konkyAdvancedIds);
    }

    //0 total frames
    //1 recovery frames
    //3 damage
    //4 chip damage DO THIS
    //5 blockstun

    //attack class:
    //0 = light
    //1 = medium
    //2 = heavy
    //3 = super

    // Standing Light
    private Attack standL = new Attack()
    {
        attackClass = 0,
        frames = new int[] { 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3 },
        damage = new int[] { 300 },
        level = 0,
        attackCancels = new int[] { 5, 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Standing Medium
    private Attack standM = new Attack()
    {
        attackClass = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 600 },
        level       = 1,
        attackCancels     = new int[] { 12, 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Standing Heavy
    private Attack standH = new Attack()
    {
        attackClass = 2,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 900 },
        level       = 2,
        attackCancels     = new int[] { 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 4,
        aAngle      = 30,
        aStrength   = 4
    };

    // Crouching Light
    private Attack crouchL = new Attack()
    {
        attackClass = 0,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 300 },
        level       = 0,
        attackCancels     = new int[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Crouching Medium
    private Attack crouchM = new Attack()
    {
        attackClass = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 500 },
        level       = 1,
        attackCancels     = new int[] { 15, 25, 22, 31, 32, 33, 34, 35, 36 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Crouching Heavy
    private Attack crouchH = new Attack()
    {
        attackClass = 2,
        frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage = new int[] { 700, 700 },
        level = 3,
        attackCancels = new int[] { 31, 32, 33, 34, 35, 36 },
        advCancels = new int[] { 40 },
        gAngle      = 80,
        gStrength   = 4,
        aAngle      = 80,
        aStrength   = 5
    };

    // Jumping Light
    private Attack jumpL = new Attack()
    {
        attackClass = 0,
        frames      = new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 300 },
        level       = 2,
        attackCancels     = new int[] { 17, 18, 19, 27, 28, 29 },
        advCancels = new int[] { 40 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Jumping Medium
    private Attack jumpM = new Attack()
    {
        attackClass = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 600 },
        level       = 2,
        attackCancels     = new int[] { 27, 28, 29 },
        advCancels = new int[] { 40 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Jumping Heavy
    private Attack jumpH = new Attack()
    {
        attackClass = 2,
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
    private Attack forwardM = new Attack()
    {
        attackClass = 1,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 800 },
        level       = 0,
        attackCancels     = new int[] { 25, 31, 32, 33, 34, 35, 36 },
        gAngle      = 180,
        gStrength   = 2,
        aAngle      = 330,
        aStrength   = 5
    };


    // One Super
    private Attack oneS = new Attack()
    {
        attackClass = 3,
        frames      = new int[] { 0 },
        damage      = new int[] { 1200 },
        level       = 0,
        attackCancels     = new int[] { 35 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 0,
        aStrength   = 0
    };

    // Two Super
    private Attack twoS = new Attack()
    {
        attackClass = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 900 },
        level       = 5,
        attackCancels     = new int[] { 35,},
        advCancels = new int[] { 41, 42 },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Three Super
    private Attack threeS = new Attack()
    {
        attackClass = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 800 },
        level       = 4,
        attackCancels     = new int[] { 35 },
        gAngle      = 0,
        gStrength   = 1,
        aAngle      = 30,
        aStrength   = 1
    };

    // Four Super
    private Attack fourS = new Attack()
    {
        attackClass = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3 },
        damage      = new int[] { 400, 400, 600 },
        level       = 2,
        attackCancels     = new int[] { 35 },
        gAngle      = 45,
        gStrength   = 1,
        aAngle      = 45,
        aStrength   = 1
    };

    // Five Super
    private Attack fiveS = new Attack()
    {
        attackClass = 3,
        frames      = new int[] { 0, 0, 0, 0, 0, 1, 3, 3, 3, 3, 3 },
        damage      = new int[] { 0 },
        level       = 0,
        attackCancels     = new int[] { },
        gAngle      = 0,
        gStrength   = 5,
        aAngle      = 0,
        aStrength   = 0
    };

    // Six Super
    private Attack sixS = new Attack()
    {
        attackClass = 3,
        frames      = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 1500 },
        level       = 5,
        attackCancels     = new int[] { 35 },
        gAngle      = 60,
        gStrength   = 8,
        aAngle      = 60,
        aStrength   = 8
    };

    // Throw
    private Attack Throw = new Attack()
    {
        attackClass = 2,
        frames      = new int[] { 0 },
        damage      = new int[] { 1500 },
        level       = 5,
        attackCancels     = new int[] { 1, 2 },
        gAngle      = 30,
        gStrength   = 10,
        aAngle      = 0,
        aStrength   = 0
    };

    // Turns
    private Advanced flip = new Advanced()       { frames = new int[] { 0, 0, 0,                                                    }, attackCancels  = new int[] {       } };

    // Back Dash
    private Advanced backDash = new Advanced()    { frames = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, advCancels  = new int[] { 40 } };

    // Forward Dash
    private Advanced forwardDash = new Advanced() { frames = new int[] { 0                                                          }, advCancels  = new int[] { 40 }, infinite = true };

    // Stun
    private Advanced stun = new Advanced()        { frames = new int[] { 3                                                          }, attackCancels = new int[] {       } };

    // Block
    private Advanced block = new Advanced()       { frames = new int[] { 4                                                          }, attackCancels = new int[] {       } };

    // Air Dash
    private Advanced forwardAirDash = new Advanced() { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, attackCancels = new int[] {       } };
    private Advanced backAirDash = new Advanced()    { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, attackCancels = new int[] {       } };
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
//Have an Attack Blocked: 2 points
//Take Damage: 2 points
//Meter Size: 70