using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    public int[] actionCancels;
    public int[] meterCost;
    public int[] frames;
    public int[] damage;

    public bool airOK;
    public bool super;
    public Vector2? projectileLocation;
    public float projectileSpeed;
    public int projectileStrength;
    public int tier;
	public int level;
    public int hitboxFrames;
    public int hurtboxFrames;
    public int block;
    public int knockdown;
    public float p1scaling;

    public int[] gAngle;
    public int[] aAngle;

    public float[] gStrength;
    public float[] aStrength;
    public float[] hMovement;
    public float[] vMovement;

    public bool infinite;
    public bool nonPushBlockable;

    public rect[,] hitboxData;
    public rect[,] hurtboxData;

    public List<rect> currentStun;

    public AudioManager.Sound? hitSound, whiffSound;

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
