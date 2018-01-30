using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

	public bool attack;

	public int frames;

	public int startup;
	public int active;
	public int recovery;

	public int[] damage;
	public int level;

	public int[] cancels;

	public int[] meterCost;
}
