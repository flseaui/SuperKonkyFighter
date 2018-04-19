using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    /*
     * 0  - getkey     up
     * 1  - getkey     down
     * 2  - getkey     left
     * 3  - getkey     right
     * 4  - getkeydown light
     * 5  - getkeydown medium
     * 6  - getkeydown heavy
     * 7  - getkeydown special
     * 8  - getkeydown left
     * 9  - getkeydown right
     * 10 - getkeyup   left
     * 11 - getkeyup   right
     * 12 - getkeydown up
     */

    public bool[] input = new bool[13];

    private int difficulty { get; set; }

    public AIController()
    {
        difficulty = 0;
    }

    public AIController(int difficulty)
    {
        this.difficulty = difficulty;
    }

    public virtual void observe(int opposingCharacter, Vector2 opponentPosition, int opposingAction, Vector2 position, bool facingRight) { }

    public bool[] getInput()
    {
        return input;
    }
}
