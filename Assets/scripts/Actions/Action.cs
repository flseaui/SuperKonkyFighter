using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action{

	string name;

	bool attack;

	int frames;

	int startup;
	int active;
	int recovery;

	int[] damage;
	int[] hitStun;

	int[] chipDamage;
	int[] blockStun;

	int[] meterCost;

	double scaling;
}
