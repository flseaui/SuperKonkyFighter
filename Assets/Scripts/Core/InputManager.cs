using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class InputManager
    {
        public static bool IsInputEnabled = false;

        // controller 1 and 2 ids
        public static int C1Id = 3, C2Id = 3;

        public readonly bool[] CurrentInput = new bool[14];

        public bool Up = false,
            Right = false,
            Down = false,
            Left = false,
            Light = false,
            Medium = false,
            Heavy = false,
            Special = false;


        private JoyScript _joyScript;

        private readonly KeyCode _upKey,
            _rightKey,
            _downKey,
            _leftKey,
            _lightKey,
            _mediumKey,
            _heavyKey,
            _specialKey;

        public Button LightButton,
            MediumButton,
            HeavyButton;

        private float _hAxis, _vAxis, _dhAxis, _dvAxis, 
            _hAxisPrev, _vAxisPrev, _dhAxisPrev, _dvAxisPrev;

        private bool _x, _y, _a, _b, _rb;
        private bool _xPrev, _yPrev, _aPrev, _bPrev, _rbPrev;

        public InputManager(int playerId)
        {
            switch (playerId)
            {
                case 1:
                    _upKey = KeyCode.W;
                    _rightKey = KeyCode.D;
                    _downKey = KeyCode.S;
                    _leftKey = KeyCode.A;
                    _lightKey = KeyCode.J;
                    _mediumKey = KeyCode.K;
                    _heavyKey = KeyCode.L;
                    _specialKey = KeyCode.Slash;
                    break;
                case 2:
                    _upKey = KeyCode.UpArrow;
                    _rightKey = KeyCode.RightArrow;
                    _downKey = KeyCode.DownArrow;
                    _leftKey = KeyCode.LeftArrow;
                    _lightKey = KeyCode.Keypad4;
                    _mediumKey = KeyCode.Keypad5;
                    _heavyKey = KeyCode.Keypad6;
                    _specialKey = KeyCode.KeypadEnter;
                    break;
            }
        }

        /* 
     * Polls user input
     * inputType = 0 : keyboards
     * inputType = 1 : joystick 
     */
        public void PollInput(int inputType, int playerId)
        {
            if (C1Id >= 3 && playerId == 1)
                inputType = 0;
            if (C2Id >= 3 && playerId == 2)
                inputType = 0;

            if (IsInputEnabled)
            {
                _hAxisPrev = _hAxis;
                _vAxisPrev = _vAxis;
                _dhAxisPrev = _dhAxis;
                _dvAxisPrev = _dvAxis;

                _xPrev = _x;
                _yPrev = _y;
                _rbPrev = _rb;
                _bPrev = _b;
                _aPrev = _a;

                if (inputType == 1)
                {
                    switch (playerId)
                    {
                        case 1 when C1Id == 1:
                            _hAxis = Input.GetAxis("Horizontal");
                            _vAxis = Input.GetAxis("Vertical");
                            _dhAxis = Input.GetAxis("XboxDpadHorizontal");
                            _dvAxis = Input.GetAxis("XboxDpadVertical");
                            _x = Input.GetButton("XboxX");
                            _y = Input.GetButton("XboxY");
                            _rb = Input.GetButton("XboxRB");
                            _b = Input.GetButton("XboxB");
                            _a = Input.GetButton("XboxA");
                            break;
                        case 1:
                        {
                            if (C2Id == 1)
                            {
                                _hAxis = Input.GetAxis("Horizontal_2");
                                _vAxis = Input.GetAxis("Vertical_2");
                                _dhAxis = Input.GetAxis("XboxDpadHorizontal_2");
                                _dvAxis = Input.GetAxis("XboxDpadVertical_2");
                                _x = Input.GetButton("XboxX_2");
                                _y = Input.GetButton("XboxY_2");
                                _rb = Input.GetButton("XboxRB_2");
                                _b = Input.GetButton("XboxB_2");
                                _a = Input.GetButton("XboxA_2");
                            }

                            break;
                        }

                        case 2 when C1Id == 2:
                            _hAxis = Input.GetAxis("Horizontal");
                            _vAxis = Input.GetAxis("Vertical");
                            _dhAxis = Input.GetAxis("XboxDpadHorizontal");
                            _dvAxis = Input.GetAxis("XboxDpadVertical");
                            _x = Input.GetButton("XboxX");
                            _y = Input.GetButton("XboxY");
                            _rb = Input.GetButton("XboxRB");
                            _b = Input.GetButton("XboxB");
                            _a = Input.GetButton("XboxA");
                            break;
                        case 2:
                        {
                            if (C2Id == 2)
                            {
                                _hAxis = Input.GetAxis("Horizontal_2");
                                _vAxis = Input.GetAxis("Vertical_2");
                                _dhAxis = Input.GetAxis("XboxDpadHorizontal_2");
                                _dvAxis = Input.GetAxis("XboxDpadVertical_2");
                                _x = Input.GetButton("XboxX_2");
                                _y = Input.GetButton("XboxY_2");
                                _rb = Input.GetButton("XboxRB_2");
                                _b = Input.GetButton("XboxB_2");
                                _a = Input.GetButton("XboxA_2");
                            }

                            break;
                        }
                    }
                }

                switch (inputType)
                {
                    case 0:
                        // Keyboard
                        CurrentInput[0] = Input.GetKey(_upKey);
                        CurrentInput[1] = Input.GetKey(_downKey);
                        CurrentInput[2] = Input.GetKey(_leftKey);
                        CurrentInput[3] = Input.GetKey(_rightKey);
                        CurrentInput[4] = Input.GetKeyDown(_lightKey);
                        CurrentInput[5] = Input.GetKeyDown(_mediumKey);
                        CurrentInput[6] = Input.GetKeyDown(_heavyKey);
                        CurrentInput[7] = Input.GetKeyDown(_specialKey);
                        CurrentInput[8] = Input.GetKeyDown(_leftKey);
                        CurrentInput[9] = Input.GetKeyDown(_rightKey);
                        CurrentInput[10] = Input.GetKeyUp(_leftKey);
                        CurrentInput[11] = Input.GetKeyUp(_rightKey);
                        CurrentInput[12] = Input.GetKeyDown(_upKey);
                        CurrentInput[13] = Input.GetKeyDown(_downKey);
                        break;
                    case 1:
                        CurrentInput[0] = _vAxis < 0 || _dvAxis > 0;
                        CurrentInput[1] = _vAxis > 0 || _dvAxis < 0;
                        CurrentInput[2] = _hAxis < 0 || _dhAxis < 0;
                        CurrentInput[3] = _hAxis > 0 || _dhAxis > 0;
                        CurrentInput[4] = JustPressedButton("x");
                        CurrentInput[5] = JustPressedButton("y");
                        CurrentInput[6] = JustPressedButton("rb") || JustPressedButton("b");
                        CurrentInput[7] = JustPressedButton("a");
                        CurrentInput[8] = JustPressed("left");
                        CurrentInput[9] = JustPressed("right");
                        CurrentInput[10] = JustReleased("left");
                        CurrentInput[11] = JustReleased("right");
                        CurrentInput[12] = JustPressed("up");
                        CurrentInput[13] = JustPressed("down");
                        break;
                    case 2:
                        // Joystick
                        CurrentInput[0] = _joyScript.Up;
                        CurrentInput[1] = _joyScript.Down;
                        CurrentInput[2] = _joyScript.Left;
                        CurrentInput[3] = _joyScript.Right;
                        //currentInput[4] = (light button);
                        //currentInput[5] = (medium button);
                        //currentInput[6] = (heavy button);
                        //currentInput[7] = (specialbutton);
                        break;
                    default:
                        // Default - Keyboard
                        CurrentInput[0] = Input.GetKey(_upKey);
                        CurrentInput[1] = Input.GetKey(_downKey);
                        CurrentInput[2] = Input.GetKey(_leftKey);
                        CurrentInput[3] = Input.GetKey(_rightKey);
                        CurrentInput[4] = Input.GetKeyDown(_lightKey);
                        CurrentInput[5] = Input.GetKeyDown(_mediumKey);
                        CurrentInput[6] = Input.GetKeyDown(_heavyKey);
                        CurrentInput[7] = Input.GetKeyDown(_specialKey);
                        CurrentInput[8] = Input.GetKeyDown(_leftKey);
                        CurrentInput[9] = Input.GetKeyDown(_rightKey);
                        CurrentInput[10] = Input.GetKeyUp(_leftKey);
                        CurrentInput[11] = Input.GetKeyUp(_rightKey);
                        CurrentInput[12] = Input.GetKeyDown(_upKey);
                        CurrentInput[13] = Input.GetKeyDown(_downKey);
                        break;
                }
            }
        }

        private bool JustPressedButton(string name)
        {
            switch (name)
            {
                case "x":
                    return _xPrev == false && _x;
                case "y":
                    return _yPrev == false && _y;
                case "rb":
                    return _rbPrev == false && _rb;
                case "b":
                    return _bPrev == false && _b;
                case "a":
                    return _aPrev == false && _a;
                default:
                    return false;
            }
        }

        private bool JustPressed(string direction)
        {
            switch (direction)
            {
                case "left":
                    return _hAxisPrev == 0 && _hAxis < 0 || _dhAxisPrev == 0 && _dhAxis < 0;
                case "right":
                    return _hAxisPrev == 0 && _hAxis > 0 || _dhAxisPrev == 0 && _dhAxis > 0;
                case "up":
                    return _vAxisPrev == 0 && _vAxis < 0 || _dvAxisPrev == 0 && _dvAxis > 0;
                case "down":
                    return _vAxisPrev == 0 && _vAxis > 0 || _dvAxisPrev == 0 && _dvAxis < 0;
                default:
                    return false;
            }
        }

        private bool JustReleased(string direction)
        {
            switch (direction)
            {
                case "left":
                    return _hAxisPrev < 0 && _hAxis == 0 || _dhAxisPrev < 0 && _dhAxis == 0;
                case "right":
                    return _hAxisPrev > 0 && _hAxis == 0 || _dhAxisPrev > 0 && _dhAxis == 0;
                case "up":
                    return _vAxisPrev < 0 && _vAxis == 0 || _dvAxisPrev > 0 && _dvAxis == 0;
                case "down":
                    return _vAxisPrev > 0 && _vAxis == 0 || _dvAxisPrev < 0 && _dvAxis == 0;
                default:
                    return false;
            }
        }

    }
}