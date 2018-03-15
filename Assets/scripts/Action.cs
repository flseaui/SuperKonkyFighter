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
    public int hitboxFrames;
    public int hurtboxFrames;
    public float gStrength;
    public float aStrength;

    public bool infinite;

    public rect[,] hitboxData;
    public rect[,] hurtboxData;
    public List<int> hitframes;
    public List<Action.rect> hurtboxes;

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
