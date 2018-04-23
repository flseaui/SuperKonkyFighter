using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonkyAI : AIController {

    public override void observe(int opposingCharacter, Vector2 opponentPosition, int opposingAction, Vector2 position, bool facingRight)
    {
        float dx = Mathf.Abs(opponentPosition.x - position.x);

        flipInputs(dx);

        bool closeRange = dx < 10;
        bool mediumRange = dx < 20;
        bool farRange = dx > 30;

        switch (opposingCharacter)
        {
            // Konky
            case 0:
                if (closeRange)
                {
                    if (opposingAction == 41)
                    {
                        //block
                    }
                }
                else if (mediumRange)
                {
                    if (opposingAction == 41)
                        input[facingRight ? 8 : 9] = true;
                    else if (opposingAction == 0)
                        input[facingRight ? 8 : 9] = true;
                    else
                        input[facingRight ? 2 : 3] = true;

                }
                else if (farRange)
                {
                    input[facingRight ? 3 : 2] = true;
                }
                break;
            // Grey Shirt
            case 1:
                break;
        }
    }

    private void flipInputs(float dx)
    {
        input[0] = false;
        input[1] = false;
        if (dx < 25)
            input[2] = false;
        if (dx < 25)
            input[3] = false;
        input[4] = false;
        input[5] = false;
        input[6] = false;
        input[7] = false;
        input[8] = false;
        input[9] = false;
        input[10] = false;
        input[11] = false;
        input[12] = false;
    }
}
