using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyBehaviours : Behaviors{

	//0 total frames
	//1 recovery frames
	//3 damage
	//4 chip damage DO THIS
	//5 blockstun

	private Action crouchL = new Action() {attack = true, frames = 19, startup = 6, active = 3, recovery = 10, damage = new int[] {300}, level = 0, cancels = new int[]{a5m, a2m, a6m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s} };
	private Action crouchM = new Action() {attack = true, frames = 24, startup = 7, active = 4, recovery = 13, damage = new int[] {650}, level = 1, cancels = new int[] {a5m, a5h, a2h, a1s, a2s, a3s, a4s, a5s, a6s } };
	private Action crouchH = new Action() { };

	static Action[] konkyActions = new Action[]
	{
		crouchL,
		crouchM,
		crouchH,
		new Action() {attack = true, frames = 13, startup = 4, active = 3, recovery = 6, damage = new int[]{200}, level = 0, cancels = new int[]{a1l } },
	};
	
	static int[,] konkyFrames = new int[,]
    {//Total, Recovery, Damage
        {15,  8, 350},//5L  0  | Knockback: , Angle: , Damage: 350, Chip Damage: 35, Attribute: Mid, Level: 0, P1 Scaling: 1
        {20, 12, 625},//5M  1  | Knockback: , Angle: , Damage: 625, Chip Damage: 63, Attribute: Mid, Level: 1, P1 Scaling: 1
        {34, 18, 450},//6M  2  | Knockback: , Angle: , Damage: 450, Chip Damage: 45, Attribute: High, Level: 0, P1 Scaling: 1.1
        {30, 17, 950},//5H  3  | Knockback: , Angle: , Damage: 950, Chip Damage: 95, Attribute: Mid, Level: 2, P1 Scaling: 1
        {19, 10, 250},//2L  4  | Knockback: , Angle: , Damage: 250, Chip Damage: 25, Attribute: Low, Level: 0, P1 Scaling: 1
        {24, 13, 650},//2M  5  | Knockback: , Angle: , Damage: 650, Chip Damage: 65, Attribute: Mid, Level: 1, P1 Scaling: 1
        {20, 23, 650},//2H  6  | Knockback: , Angle: , Damage: 200+650, Chip Damage: 20+65, Attribute: Low, Level: 2, P1 Scaling: 1
        {16,  7, 350},//jL  7  | Knockback: , Angle: , Damage: 350, Chip Damage: 35, Attribute: High, Level: 2, P1 Scaling: .8
        {26, 13, 650},//jM  8  | Knockback: , Angle: , Damage: 650, Chip Damage: 65, Attribute: High, Level: 2, P1 Scaling: .8
        {31, 17, 1000},//jH  9  | Knockback: , Angle: , Damage: 1000, Chip Damage: 100, Attribute: High, Level: 3+KD, P1 Scaling: .8
        {52, 40, 1200},//1S  10 | Knockback: , Angle: , Damage: 1200, Chip Damage: -, Attribute: Throw, Level: KD, P1 Scaling: 1
        {26, 13, 1500},//6S  11 | Knockback: , Angle: , Damage: 1500, Chip Damage: 150, Attribute: Mid, Level: 5, P1 Scaling: 1
        {34, 20, 400},//3S  12 | Knockback: , Angle: , Damage: 400, Chip Damage: 40, Attribute: Mid, Level: 4, P1 Scaling: 1
        {53, 33, 500},//2S  13 | Knockback: , Angle: , Damage: 500, Chip Damage: 50, Attribute: Mid, Level: 5, P1 Scaling: 1
		{999, 989, 500},//4S   | Knockback: , Angle: , Damage: 400+400+600, Chip Damage: 40+40+60, Attribute: High, Level: 2+KD, P1 Scaling: 1
        //Throw Knockback: , Angle: , Damage: 1500, Chip Damage: -, Attribute: Throw, Level: 5+KD, P1 Scaling: .5
		{0,0,0 },//15
		{0,0,0 },//16
		{0,0,0 },//17
		{0,0,0 },//18
		{0,0,0 },//19
		{0,0,0 },//20
		{0,0,0 },//21
		{0,0,0 },//22
		{0,0,0 },//23
		{0,0,0 },//24
		{0,0,0 },//25
		{0,0,0 },//26
		{0,0,0 },//27
		{0,0,0 },//28
		{0,0,0 },//29
		{0,0,0 },//30
		{0,0,0 },//31
		{6, 0,0},//32  TURN AROUND
		{8, 0,0},//33  JUMP SQUAT
		{20,0,0},//34  BACK DASH
		{20,0,0},//35  FORWARD DASH
		{6, 0,0},//36  GROUND TURN
		{100, 0, 0},//37 STANDING BLOCK
    };

    public KonkyBehaviours() : base(konkyActions) {
		
	}
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