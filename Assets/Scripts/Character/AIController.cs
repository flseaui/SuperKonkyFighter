using UnityEngine;

namespace Character
{
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

        protected bool[] Input = new bool[13];

        private int _difficulty;

        protected AIController()
        {
            _difficulty = 0;
        }

        public AIController(int difficulty)
        {
            _difficulty = difficulty;
        }

        public virtual void observe(int opposingCharacter, Vector2 opponentPosition, int opposingAction, Vector2 position, bool facingRight) { }

        public bool[] getInput()
        {
            return Input;
        }
    }
}
