using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Behaviors{

	public const int a1l = 0;
	public const int a1m = 1;
	public const int a1h = 2;
	public const int a2l = 3;
	public const int a2m = 4;
	public const int a2h = 5;
	public const int a3l = 6;
	public const int a3m = 7;
	public const int a3h = 8;
	public const int a4l = 9;
	public const int a4m = 10;
	public const int a4h = 11;
	public const int a5l = 12;
	public const int a5m = 13;
	public const int a5h = 14;
	public const int a6l = 15;
	public const int a6m = 16;
	public const int a6h = 17;
	public const int a7l = 18;
	public const int a7m = 19;
	public const int a7h = 20;
	public const int a8l = 21;
	public const int a8m = 22;
	public const int a8h = 23;
	public const int a9l = 24;
	public const int a9m = 25;
	public const int a9h = 26;
	public const int a1s = 27;
	public const int a2s = 28;
	public const int a3s = 29;
	public const int a4s = 30;
	public const int a5s = 31;
	public const int a6s = 32;
	public const int a7s = 33;
	public const int a8s = 34;
	public const int a9s = 35;
	public const int aTurn = 36;
	public const int aCTurn = 37;
	public const int aJump = 38;
	public const int aBDash = 39;
	public const int aDash = 40;
	public const int aBlock = 41;
	public const int aCBlock = 42;
	public const int aABlock = 43;
	public const int aStun = 44;
	public const int aRecover = 45;

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
