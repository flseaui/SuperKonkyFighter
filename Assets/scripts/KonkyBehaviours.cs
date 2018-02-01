using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

	//0 total frames
	//1 recovery frames
	//3 damage
	//4 chip damage DO THIS
	//5 blockstun

	static Action crouchL = new Action() {attack = true, frames = 19, startup = 6, active = 3, recovery = 10, damage = new int[] {300}, level = 0, cancels = new int[]{a5m, a2m, a6m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s} };
	static Action crouchM = new Action() {attack = true, frames = 23, startup = 7, active = 4, recovery = 13, damage = new int[] {650}, level = 1, cancels = new int[] {a5m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s } };
	static Action crouchH = new Action() {attack = true, frames = 40, startup = 8, active = 8, recovery = 24, damage = new int[] { 200, 650 }, level = 2, cancels = new int[] {aJump, a1s, a2s, a3s, a4s, a5s, a6s } };
	static Action l = new Action() { attack = true, frames = 13, startup = 4, active = 3, recovery = 6, damage = new int[] { 200 }, level = 0, cancels = new int[] {a5l, a2l, a5m, a2m, a6m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s } };
	static Action turns = new Action() { attack = false, frames = 6, cancels = new int[] { } };
	static Action jump = new Action() { attack = false, frames = 8, cancels = new int[] { } };
	static Action backDash = new Action() { attack = false, frames = 20, cancels = new int[] {aJump} };
	static Action forwardDash = new Action() { attack = false, frames = -1, cancels = new int[] { aJump } };

	static Action[] konkyActions = new Action[]
	{
		crouchL,
		crouchM,
		crouchH,
		crouchL,
		crouchM,
		crouchH,
		crouchL,
		crouchM,
		crouchH,
		null,
		null,
		null,
		l,
		l,
		l,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		turns,
		turns,
		jump,
		backDash,
		forwardDash,
		null,
		null,
		null,
		null,
		null
	};

	public KonkyBehaviours() : base(konkyActions) { }
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