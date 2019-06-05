using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character
{
    public class Action {

        public int[] ActionCancels;
        public int[] MeterCost;
        public int[] Frames;
        public int[] Damage;

        public bool AirOk;
        public bool Super;
        public Vector2? ProjectileLocation;
        public float ProjectileSpeed;
        public int ProjectileStrength;
        public int Tier;
        public int Level;
        public int HitboxFrames;
        public int HurtboxFrames;
        public int Block;
        public int Knockdown;
        public float P1Scaling;

        public int[] GAngle;
        public int[] AAngle;

        public float[] GStrength;
        public float[] AStrength;
        public float[] HMovement;
        public float[] VMovement;

        public bool Infinite;
        public bool NonPushBlockable;

        public Rect[,] HitboxData;
        public Rect[,] HurtboxData;

        public List<Rect> CurrentStun;

        public AudioManager.Sound? HitSound, WhiffSound;

        public struct Rect
        {
            public readonly float X, Y, Width, Height, TimeActive;
            public readonly int Id;

            public Rect(float x, float y, float width, float height, float timeActive, int id)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                TimeActive = timeActive;
                Id = id;
            }
        }
    }
}
