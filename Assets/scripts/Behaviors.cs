using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Behaviors {

    enum ATTACK_IDS
    {
        // DOWN BACK
        _1L = 1,
        _1M = 11,
        _1H = 21,

        // DOWN
        _2L = 2,
        _2M = 12,
        _2H = 22,

        // DOWN FORWARD
        _3L = 3,
        _3M = 13,
        _3H = 23,

        // BACK
        _4L = 4,
        _4M = 14,
        _4H = 24,

        // NEUTRAL
        _5L = 5,
        _5M = 15,
        _5H = 25,

        // FORWARD
        _6L = 6,
        _6M = 16,
        _6H = 26,

        // UP BACK
        _7L = 7,
        _7M = 17,
        _7H = 27,

        // UP
        _8L = 8,
        _8M = 18,
        _8H = 28,

        // UP FORWARD
        _9L = 9,
        _9M = 19,
        _9H = 29,

        // SPECIALS
        _1S = 31,
        _2S = 32,
        _3S = 33,
        _4S = 34,
        _5S = 35,
        _6S = 36
    };

    private Attack[] actions;

    public Behaviors()
    {
       
    }

	public Attack getAction(int attack)
	{
		return actions[attack];
	}


}

