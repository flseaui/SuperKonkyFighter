using Core;
using UnityEngine;

namespace UI
{
    public class MenuOverlay : MonoBehaviour
    {
        //broken record studios?

        public GameObject Selector;

        private InputManager _inputManager, _inputManager2;

        public static int State = 0;
        public static int State2 = 1;
        public int Menu = 0;

        public int S, S2;

        bool _firstLoad = false, _cameFromNext = false;

        void Start ()
        {
            _inputManager = new InputManager(1);
            _inputManager2 = new InputManager(2);
            InputManager.IsInputEnabled = true;
        }

        void Update()
        {
            bool skip = false;
            S = State;
            S2 = State2;
            _inputManager.PollInput(0, 1);
            _inputManager2.PollInput(0, 2);

            if (_inputManager2.CurrentInput[8])
            {
                State2--;
            }
            else if (_inputManager2.CurrentInput[9])
            {
                State2++;
            }

            if (_inputManager.CurrentInput[Menu < 2 ? 12 : 8])
            {
                State--;
            }
            else if (_inputManager.CurrentInput[Menu < 2 ? 13 : 9])
            {
                State++;
            }
            //if pressed light (confirm)
            else if (_inputManager.CurrentInput[4])
            {
                switch (Menu)
                {
                    // main menu
                    case 0:
                        switch (State)
                        {
                            // play button
                            case 0:
                                GetComponentInParent<MenuScript>().TriggerEvent(1);
                                Selector.SetActive(false);
                                _firstLoad = false;
                                _cameFromNext = false;
                                State = 0;
                                Menu = 2;
                                break;
                            // settings button
                            case 1:
                                GetComponentInParent<MenuScript>().TriggerEvent(15);
                                State = 0;
                                Menu = 1;
                                break;
                        }
                        break;
                    // settings
                    case 1:
                        switch (State)
                        {
                            case 0:
                                GetComponentInParent<MenuScript>().TriggerEvent(16);
                                break;
                            case 1:
                                GetComponentInParent<MenuScript>().TriggerEvent(17);
                                break;
                            case 2:
                                GetComponentInParent<MenuScript>().TriggerEvent(18);
                                break;
                            case 3:
                                GetComponentInParent<MenuScript>().TriggerEvent(19);
                                break;
                            case 4:
                                GetComponentInParent<MenuScript>().TriggerEvent(20);
                                break;
                        }
                        break;
                    case 3:
                        GetComponentInParent<MenuScript>().BeginGame();
                        break;
                    // player select go button
                    case 10:
                        GetComponentInParent<MenuScript>().TriggerEvent(2);
                        Selector.SetActive(false);
                        Menu = 3;
                        break;
                }
            }
            // if pressed medium (back)
            else if (_inputManager.CurrentInput[5])
                switch (Menu)
                {
                    case 1:
                        GetComponentInParent<MenuScript>().TriggerEvent(0);
                        State = 1;
                        Menu = 0;
                        break;
                    case 2:
                        GetComponentInParent<MenuScript>().TriggerEvent(0);
                        State = 0;
                        Menu = 0;
                        Selector.SetActive(true);
                        break;
                    case 3:
                        GetComponentInParent<MenuScript>().TriggerEvent(1);
                        Menu = 2;
                        State = GetComponentInParent<MenuScript>().Player1C;
                        State2 = GetComponentInParent<MenuScript>().Player2C;
                        GetComponentInParent<MenuScript>().Player1C = -1;
                        GetComponentInParent<MenuScript>().Player2C = -1;
                        _cameFromNext = true;
                        skip = true;
                        Selector.SetActive(false);
                        break;
                    case 10:
                        Menu = 2;
                        State = GetComponentInParent<MenuScript>().Player1C;
                        State2 = GetComponentInParent<MenuScript>().Player2C;
                        GetComponentInParent<MenuScript>().Player1C = -1;
                        GetComponentInParent<MenuScript>().Player2C = -1;
                        GetComponentInParent<MenuScript>().UpdateChar(State, false);
                        GetComponentInParent<MenuScript>().UpdateChar(State2, true);
                        _cameFromNext = true;
                        Selector.SetActive(false);
                        skip = true;
                        break;
                }

            if (!skip)
            {
                switch (Menu)
                {
                    case 0:
                        if (State > 1)
                            State = 0;
                        else if (State < 0)
                            State = 1;
                        break;
                    case 1:
                        if (State > 4)
                            State = 0;
                        else if (State < 0)
                            State = 4;
                        break;
                    case 2:
                        if (State > 3)
                            State = 0;
                        else if (State < 0)
                            State = 3;


                        if (State2 > 3)
                            State2 = 0;
                        else if (State2 < 0)
                            State2 = 3;
                        break;
                    case 3:
                        if (State > 4)
                            State = 0;
                        else if (State < 0)
                            State = 4;
                        break;
                }
            }
            else
                skip = false;

            MoveSelector();

        }

        void MoveSelector()
        {
            switch (Menu)
            {
                // main menu
                case 0:
                    switch (State)
                    {
                        // play button
                        case 0:
                            Selector.transform.position = new Vector3(0.3f, -3.1f, 0);
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(10.4f, 4.2f);
                            break;
                        // settings button
                        case 1:
                            Selector.transform.position = new Vector3(0.2f, -7, 0);
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(12.4f, 3.5f);
                            break;
                    }
                    break;
                // settings menu
                case 1:
                    switch (State)
                    {
                        case 0:
                            Selector.transform.position = new Vector3(-10.5f, 4, 0);                        
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(7.44f, 2.68f);
                            break;
                        case 1:
                            Selector.transform.position = new Vector3(-10.5f, 0, 0);
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(7.44f, 2.68f);
                            break;
                        case 2:
                            Selector.transform.position = new Vector3(-10.5f, -4, 0);
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(7.44f, 2.68f);
                            break;
                        case 3:
                            Selector.transform.position = new Vector3(5.5f, 3, 0);
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(7.79f, 2.8f);
                            break;
                        case 4:
                            Selector.transform.position = new Vector3(5.5f, -3.51f, 0);
                            Selector.GetComponent<SpriteRenderer>().size = new Vector2(7.79f, 2.8f);
                            break;
                    }
                    break;
                case 2:
                    if (_firstLoad)
                    {
                        if (_inputManager.CurrentInput[4])
                            GetComponentInParent<MenuScript>().UpdateChar(State, false);
                        if (_inputManager2.CurrentInput[4])
                            GetComponentInParent<MenuScript>().UpdateChar(State2, true);
                        GetComponentInParent<MenuScript>().UpdateSelection(State, State2);
                    }
                    else
                        _firstLoad = true;

                    if (GetComponentInParent<MenuScript>().Player1C != -1 && GetComponentInParent<MenuScript>().Player2C != -1)
                    {
                        Selector.transform.position = new Vector3(0.27f, -3.17f, 0);
                        Selector.GetComponent<SpriteRenderer>().size = new Vector2(5.8f, 3.9f);
                        Menu = 10;
                        Selector.SetActive(true);
                    }
                    break;
                case 3:
                    switch (State)
                    {
                        case 0:
                            GetComponentInParent<MenuScript>().TriggerEvent(4);
                            break;
                        case 1:
                            GetComponentInParent<MenuScript>().TriggerEvent(5);
                            break;
                        case 2:
                            GetComponentInParent<MenuScript>().TriggerEvent(6);
                            break;
                        case 3:
                            GetComponentInParent<MenuScript>().TriggerEvent(7);
                            break;
                        case 4:
                            GetComponentInParent<MenuScript>().TriggerEvent(8);
                            break;

                    }
                    break;
            }
        }

    }
}
