using UnityEngine;

namespace Character.Konky
{
    public class KonkyAI : AIController {

        public override void observe(int opposingCharacter, Vector2 opponentPosition, int opposingAction, Vector2 position, bool facingRight)
        {
            var dx = Mathf.Abs(opponentPosition.x - position.x);

            FlipInputs(dx);

            var closeRange = dx < 10;
            var mediumRange = dx < 20;
            var farRange = dx > 30;

            switch (opposingCharacter)
            {
                // Konky
                case 0:

               

                    /*if (closeRange)
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
                }*/
                    break;
                // Grey Shirt
                case 1:
                    break;
            }
        }

        private void FlipInputs(float dx)
        {
            Input[0] = false;
            Input[1] = false;
            if (dx < 25)
                Input[2] = false;
            if (dx < 25)
                Input[3] = false;
            Input[4] = false;
            Input[5] = false;
            Input[6] = false;
            Input[7] = false;
            Input[8] = false;
            Input[9] = false;
            Input[10] = false;
            Input[11] = false;
            Input[12] = false;
        }
    }
}
