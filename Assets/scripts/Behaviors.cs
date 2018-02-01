using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviors{

	protected const int a1l = 0;
	protected const int a1m = 1;
	protected const int a1h = 2;
	protected const int a2l = 3;
	protected const int a2m = 4;
	protected const int a2h = 5;
	protected const int a3l = 6;
	protected const int a3m = 7;
	protected const int a3h = 8;
	protected const int a4l = 9;
	protected const int a4m = 10;
	protected const int a4h = 11;
	protected const int a5l = 12;
	protected const int a5m = 13;
	protected const int a5h = 14;
	protected const int a6l = 15;
	protected const int a6m = 16;
	protected const int a6h = 17;
	protected const int a7l = 18;
	protected const int a7m = 19;
	protected const int a7h = 20;
	protected const int a8l = 21;
	protected const int a8m = 22;
	protected const int a8h = 23;
	protected const int a9l = 24;
	protected const int a9m = 25;
	protected const int a9h = 26;
	protected const int a1s = 27;
	protected const int a2s = 28;
	protected const int a3s = 29;
	protected const int a4s = 30;
	protected const int a5s = 31;
	protected const int a6s = 32;
	protected const int a7s = 33;
	protected const int a8s = 34;
	protected const int a9s = 35;
	protected const int aTurn = 36;
	protected const int aCTurn = 37;
	protected const int aJump = 38;
	protected const int aBDash = 39;
	protected const int aDash = 40;
	protected const int aBlock = 41;
	protected const int aCBlock = 42;
	protected const int aABlock = 43;
	protected const int aStun = 44;
	protected const int aRecover = 45;

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
