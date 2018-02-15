using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors {

    // Konky Attack Ids
    public enum K : int
    {
        // DOWN BACK
        _1L = _2L,
        _1M = _2M,
        _1H = _2H,

        // DOWN
        _2L = 2,
        _2M = 12,
        _2H = 22,

        // DOWN FORWARD
        _3L = _2L,
        _3M = _2M,
        _3H = _2H,

        // BACK
        _4L = _5L,
        _4M = _5M,
        _4H = _5H,

        // NEUTRAL
        _5L = 5,
        _5M = 15,
        _5H = 25,

        // FORWARD
        _6L = _5L,
        _6M = 16,
        _6H = _5H,

        // UP BACK
        _7L = _8L,
        _7M = _8M,
        _7H = _8H,

        // UP
        _8L = 8,
        _8M = 18,
        _8H = 28,

        // UP FORWARD
        _9L = _8L,
        _9M = _8M,
        _9H = _8H,

        // SPECIALS
        _1S = 31,
        _2S = 32,
        _3S = 33,
        _4S = 34,
        _5S = 35,
        _6S = 36
    };


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
        cancels = new int[] { (int) K._5L, (int) K._2L, (int) K._5M, (int) K._2M, (int) K._6M, (int) K._5H, (int) K._2H, (int) K._1S, (int) K._2S, (int) K._3S, (int) K._4S, (int) K._5S, (int) K._6S },
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
        cancels     = new int[] { a2m, a5h, a5h, a1s, a2s, a3s, a4s, a5s, a6s },
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
        cancels     = new int[] { a2h, a1s, a2s, a3s, a4s, a5s, a6s },
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
        cancels     = new int[] { a5m, a2m, a6m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s },
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
        cancels     = new int[] { a5m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s },
        gAngle      = 0,
        gStrength   = 2,
        aAngle      = 30,
        aStrength   = 2
    };

    // Crouching Heavy
    private Attack crouchH = new Attack()
    {
        attackClass = 2,
        frames      = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        damage      = new int[] { 700, 700 },
        level       = 3,
        cancels     = new int[] { aJump, a1s, a2s, a3s, a4s, a5s, a6s },
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
        cancels     = new int[] { aJump, a7m, a8m, a9m, a7h, a8h, a9h },
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
        cancels     = new int[] { aJump, a7h, a8h, a9h },
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
        cancels     = new int[] { aJump },
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
        cancels     = new int[] { a5h, a5h, a1s, a2s, a3s, a4s, a5s, a6s },
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
        cancels     = new int[] { a5s },
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
        cancels     = new int[] { a5s, aDash, aBDash },
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
        cancels     = new int[] { a5s },
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
        cancels     = new int[] { a5s },
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
        cancels     = new int[] { },
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
        cancels     = new int[] { a5s },
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
        cancels     = new int[] { aDash, aBDash },
        gAngle      = 30,
        gStrength   = 10,
        aAngle      = 0,
        aStrength   = 0
    };

    // Turns
    private Attack turns = new Attack()       { frames = new int[] { 3, 3, 3, 3, 3, 3                                           },                  cancels  = new int[] {       } };

    // Jump
    private Attack jump = new Attack()        { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3                                     },                  cancels  = new int[] {       } };

    // Back Dash
    private Attack backDash = new Attack()    { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },                  cancels  = new int[] {aJump  } };

    // Forward Dash
    private Attack forwardDash = new Attack() { frames = new int[] { 3                                                          }, infinite = true, cancels  = new int[] { aJump } };

    // Stun
    private Attack stun = new Attack()        { frames = new int[] { 3                                                          }, infinite = true, cancels  = new int[] {       } };

    // Block
    private Attack block = new Attack()       { frames = new int[] { 4                                                          }, infinite = true, cancels  = new int[] {       } };

    // Air Dash
    private Attack airDash = new Attack()     { frames = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },                  cancels  = new int[] {       } };
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