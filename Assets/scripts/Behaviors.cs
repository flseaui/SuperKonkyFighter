using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Behaviors {

	public static int a1l       = 0;
	public static int a1m       = 1;
	public static int a1h       = 2;
	public static int a2l       = 3;
	public static int a2m       = 4;
	public static int a2h       = 5;
	public static int a3l       = 6;
	public static int a3m       = 7;
	public static int a3h       = 8;
	public static int a4l       = 9;
	public static int a4m       = 10;
	public static int a4h       = 11;
	public static int a5l       = 12;
	public static int a5m       = 13;
	public static int a5h       = 14;
	public static int a6l       = 15;
	public static int a6m       = 16;
	public static int a6h       = 17;
	public static int a7l       = 18;
	public static int a7m       = 19;
	public static int a7h       = 20;
	public static int a8l       = 21;
	public static int a8m       = 22;
	public static int a8h       = 23;
	public static int a9l       = 24;
	public static int a9m       = 25;
	public static int a9h       = 26;
	public static int a1s       = 27;
	public static int a2s       = 28;
	public static int a3s       = 29;
	public static int a4s       = 30;
	public static int a5s       = 31;
	public static int a6s       = 32;
	public static int a7s       = 33;
	public static int a8s       = 34;
	public static int a9s       = 35;
	public static int aTurn     = 36;
	public static int aCTurn    = 37;
	public static int aJump     = 38;
	public static int aBDash    = 39;
	public static int aDash     = 40;
	public static int aBlock    = 41;
	public static int aCBlock   = 42;
	public static int aABlock   = 43;
	public static int aStun     = 44;
	public static int aRecover  = 45;
	public static int aADash    = 46;

	private Action[] actions;

    public Behaviors(Action[] ac)
    {
        this.actions = ac;
    }

	public Action getAction(int attack)
	{
		return actions[attack];
	}
}
