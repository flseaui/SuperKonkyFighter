using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviors{

    private int[,] moveMap;

    private int[,] frames;

    public Behaviors(int[,] moveMap, int[,] frames)
    {
        this.moveMap = moveMap;
        this.frames = frames;
    }

    public int getAttack(int str, int state)
    {
        return moveMap[str,state-1];
    }

    public int getTotalTime(int attack)
    {
        return frames[attack,0];
    }

    public int getRecoveryTime(int attack)
    {
        return frames[attack, 1];
    }
}
