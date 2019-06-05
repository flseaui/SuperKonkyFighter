using Core;
using Misc;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class CameraScript : MonoBehaviour
    {
        public Canvas Canvas;
        public UIScript Uis;

        public GameObject PlayerPrefab;
        public GameObject Ghost;
        public GameObject Background;
        public GameObject Ground;
        public Camera Self;

        public GameObject Player1;
        public PlayerScript P1S;
        public CollisionScript P1H;
        public GameObject Player2;
        public PlayerScript P2S;
        public CollisionScript P2H;

        public Sprite Background0;
        public Sprite Background1;
        public Sprite Background2;
        public Sprite Background3;
        public Sprite Background4;
        public Sprite Background5;
        public Sprite Background6;

        public Sprite Ground0;
        public Sprite Ground1;
        public Sprite Ground2;
        public Sprite Ground3;

        [FormerlySerializedAs("Background")] public Sprite[] BackgroundSprites;
        [FormerlySerializedAs("Ground")] public Sprite[] GroundSprites;

        public Sprite KonkyPortrait, GreyshirtPortrait, DkPortrait, ShrulkPortrait;

        public JoyScript JoyScript;

        public Button LightButton;
        public Button MediumButton;
        public Button HeavyButton;

        public Transform CameraLeft, CameraRight;
        public Transform CameraFarLeft, CameraFarRight;
        public Transform CameraLeftPos, CameraRightPos;
        public Transform LeftEdge, RightEdge, TopEdge, BottomEdge;

        public IntVariable Time, P1Wins, P2Wins, RoundCounter;

        float _vertExtent, _horzExtent;

        private int _megaKek;

        public float ShakeX;
        public float ShakeY;
        public bool LastP1Side;
        public bool LastP2Side;

        public float Magnitude, Roughness, FadeIn, FadeOut;

        private bool _justShook, _justWon = false;
        private Vector3 _preShakePos;

        public Image PlayerPortrait1, PlayerPortrait2;

        public int WhoWon;

        void OnApplicationQuit()
        {
            RoundCounter.Value = 0;
            P1Wins.Value = 0;
            P2Wins.Value = 0;
            PlayerPrefs.DeleteKey("player1w");
            PlayerPrefs.DeleteKey("player2w");
            PlayerPrefs.GetInt("loadedFromEditor");
        }

        void Start()
        {
            CameraLeftPos.position = CameraLeft.position;
            CameraRightPos.position = CameraRight.position;
            Uis = Canvas.GetComponent<UIScript>();

            Player1 = Instantiate(PlayerPrefab);

            SetX(Player1, -16f);
            P1S = Player1.GetComponent<PlayerScript>();
            P1H = Player1.GetComponentInChildren<CollisionScript>();
            P1S.facingRight = true;
            P1S.playerId = 1;
            P1S.joyScript = JoyScript;
            P1S.cameraLeft = CameraLeftPos;
            P1S.cameraRight = CameraRightPos;
            //p1s.lightButton = lightButton;
            //p1s.mediumButton = mediumButton;
            //p1s.heavyButton = heavyButton;

            Player2 = Instantiate(PlayerPrefab);
            SetX(Player2, 16f);
            P2S = Player2.GetComponent<PlayerScript>();
            P2H = Player2.GetComponentInChildren<CollisionScript>();
            P2S.facingRight = false;
            P2S.playerId = 2;
            P2S.cameraLeft = CameraLeftPos;
            P2S.cameraRight = CameraRightPos;

            P1S.otherPlayer = Player2;
            P2S.otherPlayer = Player1;

            P1S.meterCharge = PlayerPrefs.GetInt("P1Meter");
            P2S.meterCharge = PlayerPrefs.GetInt("P2Meter");

            Ghost.GetComponent<BackGroundScript>().SetScripts(P1S, P2S);

            BackgroundSprites = new Sprite[] { Background0, Background1, Background2, Background3 };
            GroundSprites = new Sprite[] { Ground0, Ground1, Ground2, Ground3 };

            GetComponentInParent<Follow>().SetTargets(Player1.transform, Player2.transform);

            _vertExtent = GetComponentInParent<Follow>().VertExtent;
            _horzExtent = GetComponentInParent<Follow>().HorzExtent;

            int stage = PlayerPrefs.GetInt("stage");

            Background.GetComponent<SpriteRenderer>().sprite = BackgroundSprites[stage < 2 ? stage : stage - 1];
            Ground.GetComponent<SpriteRenderer>().sprite = GroundSprites[stage < 2 ? stage : stage - 1];

            switch (PlayerPrefs.GetInt("player1c"))
            {
                case 0:
                    PlayerPortrait1.sprite = KonkyPortrait;
                    if (PlayerPrefs.GetInt("player1c") == PlayerPrefs.GetInt("player2c"))
                    {
                        P2S.GetComponent<SpriteRenderer>().color = Color.red;
                        PlayerPortrait2.color = Color.red;
                    }
                    break;
                case 1:
                    PlayerPortrait1.sprite = GreyshirtPortrait;
                    if (PlayerPrefs.GetInt("player1c") == PlayerPrefs.GetInt("player2c"))
                    {
                        P2S.GetComponent<SpriteRenderer>().color = Color.cyan;
                        PlayerPortrait2.color = Color.cyan;
                    }
                    break;
                case 2:
                    PlayerPortrait1.sprite = DkPortrait;
                    if (PlayerPrefs.GetInt("player1c") == PlayerPrefs.GetInt("player2c"))
                    {
                        P2S.GetComponent<SpriteRenderer>().color = Color.green;
                        PlayerPortrait2.color = Color.green;
                    }
                    break;
            }
            switch (PlayerPrefs.GetInt("player2c"))
            {
                case 0:
                    PlayerPortrait2.sprite = KonkyPortrait;
                    break;
                case 1:
                    PlayerPortrait2.sprite = GreyshirtPortrait;
                    break;
                case 2:
                    PlayerPortrait2.sprite = DkPortrait;
                    break;
            }

            Debug.LogFormat("STAGE {0}", stage);
        }

        void Update()
        {
            /*
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        } 
        */

            if (GetX(Player1) < GetX(Player2) - 1)
            {
                P1S.playerSide = true;
                P2S.playerSide = false;

                if (GetX(Player1) < CameraLeft.position.x)
                    CameraLeftPos.position = CameraLeft.position;
                else
                    CameraLeftPos.position = CameraFarLeft.position;

                if (GetX(Player2) > CameraRight.position.x)
                    CameraRightPos.position = CameraRight.position;
                else
                    CameraRightPos.position = CameraFarRight.position;
            }
            else if (GetX(Player1) > GetX(Player2) + 1)
            {
                P1S.playerSide = false;
                P2S.playerSide = true;

                if (GetX(Player2) < CameraLeft.position.x)
                    CameraLeftPos.position = CameraLeft.position;
                else
                    CameraLeftPos.position = CameraFarLeft.position;

                if (GetX(Player1) > CameraRight.position.x)
                    CameraRightPos.position = CameraRight.position;
                else
                    CameraRightPos.position = CameraFarRight.position;
            }

            P1S.cameraLeft.position = CameraLeftPos.position;
            P1S.cameraRight.position = CameraRightPos.position;

            P2S.cameraLeft.position = CameraLeftPos.position;
            P2S.cameraRight.position = CameraRightPos.position;

            Uis.Health1.maxValue = P1S.maxHealth;
            Uis.Health1.minValue = 0;
            Uis.Health1P.maxValue = P1S.maxHealth;
            Uis.Health1P.minValue = 0;
            Uis.Health2.maxValue = P2S.maxHealth;
            Uis.Health2.minValue = 0;
            Uis.Health2P.maxValue = P2S.maxHealth;
            Uis.Health2P.minValue = 0;
            Uis.Meter1.maxValue = P1S.maxMeter;
            Uis.Meter1.minValue = 0;
            Uis.Meter1P.maxValue = P1S.maxMeter;
            Uis.Meter1P.minValue = 0;
            Uis.Meter2.maxValue = P2S.maxMeter;
            Uis.Meter2.minValue = 0;
            Uis.Meter2P.maxValue = P2S.maxMeter;
            Uis.Meter2P.minValue = 0;
            Uis.Health1.value = P1S.health;
            Uis.Health1P.value = P1S.health + P1S.healthStore;
            Uis.Meter1.value = P1S.meterCharge;
            Uis.Meter1P.value = P1S.meterCharge + P1S.meterStore;
            Uis.Health2.value = P2S.health;
            Uis.Health2P.value = P2S.health + P2S.healthStore;
            Uis.Meter2.value = P2S.meterCharge;
            Uis.Meter2P.value = P2S.meterCharge + P2S.meterStore;

            if (P1S.health <= 0)
            {
                //RoundManager.instance.nextRound();
                //PlayerPrefs.SetInt("menu_state", 1);
                if (!_justWon && P2S.executingAction == 58)
                {
                    _justWon = true;
                    WhoWon = 2;
                    P2Wins.Value++;
                    Invoke("NextRound", 2);
                }
            }
            else if (P2S.health <= 0)
            {
                if (!_justWon && P1S.executingAction == 58)
                {
                    _justWon = true;
                    WhoWon = 1;
                    P1Wins.Value++;
                    Invoke("NextRound", 2);
                }
            }
            else if (Time.Value == 0)
            {
                if (!_justWon)
                {
                    _justWon = true;
                    if (P1S.health > P2S.health)
                    {
                        P1Wins.Value++;
                        WhoWon = 1;
                    }
                    else
                    {
                        P2Wins.Value++;
                        WhoWon = 2;
                    }
                    Invoke("NextRound", 2);
                }
            }

            if (Ghost.GetComponent<BackGroundScript>().Shake)
            {
                _horzExtent = 26.66667f;

                if (_justShook == false)
                {
                    _preShakePos = transform.position;

                    P1S.camLeft = _preShakePos.x - _horzExtent;
                    P2S.camLeft = _preShakePos.x - _horzExtent;

                    P1S.camRight = _preShakePos.x + _horzExtent;
                    P2S.camRight = _preShakePos.x + _horzExtent;
                
                    _justShook = true;
                }

                ShakeX = Random.Range(-0.75f, 0.75f);
                ShakeY = Random.Range(-0.75f, 0.75f);

                float camX = transform.parent.transform.position.x + ShakeX;

                if (camX - (_horzExtent) < LeftEdge.position.x)
                    camX = LeftEdge.position.x + (_horzExtent);
                else if (camX + (_horzExtent) > RightEdge.position.x)
                    camX = RightEdge.position.x - (_horzExtent);

                Debug.Log(_horzExtent);
                //Debug.LogFormat("x:{0}, width: {1}, leftEdge: {2}, rightEdge: {3}", camX, horzExtent, leftEdge.position.x, rightEdge.position.x);

                if (camX + _horzExtent > RightEdge.position.x)
                    camX = RightEdge.position.x - _horzExtent;
                else if (camX - _horzExtent < LeftEdge.position.x)
                    camX = LeftEdge.position.x - _horzExtent;

                SetX(Self, camX);
                SetY(Self, _preShakePos.y + ShakeY);
            
            }
            else if (_justShook)
            {
                transform.position = _preShakePos;
                _justShook = false;
            }
        }

        private void NextRound()
        {
            PlayerPrefs.SetInt("loadedFromEditor", 1);
            int limit = 2;
            switch (PlayerPrefs.GetInt("settingBestOf"))
            {
                case 3:
                    limit = 2;
                    break;
                case 5:
                    limit = 3;
                    break;
                case 7:
                    limit = 4;
                    break;
                case 9:   
                    limit = 5; 
                    break;
            }
            if (!(P1Wins.Value >= limit || P2Wins.Value >= limit))
            {
                PlayerPrefs.SetInt("P1Meter", P1S.meterCharge + P1S.meterStore);
                PlayerPrefs.SetInt("P2Meter", P2S.meterCharge + P2S.meterStore);
                SceneManager.LoadScene("SKF");
            }
            else
            {
                RoundManager.Instance.RoundManagerObj.SetActive(true);
                RoundManager.Instance.StopAllCoroutines();
                RoundManager.Instance.StartCoroutine(RoundManager.Instance.FadeInRound(true, WhoWon));
            }
        }

        private float GetX(GameObject o)
        {
            return o.transform.position.x;
        }
        private float GetX(Camera o)
        {
            return o.transform.position.x;
        }

        private float GetY(GameObject o)
        {
            return o.transform.position.y;
        }
        private float GetY(Camera o)
        {
            return o.transform.position.y;
        }

        private void MoveX(Camera o, float amm)
        {
            Vector3 position = o.transform.position;
            position.x += amm;
        }

        private void SetX(Camera o, float amm)
        {
            Vector3 position = o.transform.position;
            position.x = amm;
            o.transform.position = position;
        }

        private void SetX(GameObject o, float amm)
        {
            Vector3 position = o.transform.position;
            position.x = amm;
            o.transform.position = position;
        }

        private void SetY(GameObject o, float amm)
        {
            Vector3 position = o.transform.position;
            position.y = amm;
            o.transform.position = position;
        }

        private void SetY(Camera o, float amm)
        {
            Vector3 position = o.transform.position;
            position.y = amm;
            o.transform.position = position;
        }
    }
}
