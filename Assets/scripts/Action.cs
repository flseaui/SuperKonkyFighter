using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    public int[] actionCancels;
    public int[] meterCost;
    public int[] frames;
    public int[] damage;

    public int tier;
	public int level;
    public int gAngle;
    public int aAngle;
    public int startup;
    public int active;
    public float gStrength;
    public float aStrength;

    public bool infinite;

    public rect[,] hitboxData;

    public struct rect
    {
        public float x, y, width, height, timeActive;
        public int id;

        public rect(float x, float y, float width, float height, float timeActive, int id)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.timeActive = timeActive;
            this.id = id;
        }
    }

}
