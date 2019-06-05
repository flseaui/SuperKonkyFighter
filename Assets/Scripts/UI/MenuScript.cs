using System.Collections.Generic;
using Core;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MenuScript : MonoBehaviour
    {

        Color _konkyRed = new Color(0.9f, 0, 0, 1);
        const int TitleScreen = 0;
        const int PlayerSelectScreen = 1;
        const int StageSelectScreen = 2;
        const int SettingsScreen = 3;

        private readonly int[] _times = { 99, 120, 60, -1 };
        private readonly int[] _bestofs = { 3, 5, 7, 9 };

        //---------------------misc sprites-----------------//

        public Sprite TitleSprite;
        public Sprite PlayButtonSprite;
        public Sprite PlatformSprite;
        public Sprite KonkySelect;
        public Sprite GreyshirtSelect;
        public Sprite DkSelect;
        public Sprite ShrulkSelect;
        public Sprite PlayerSelectText;
        public Sprite StageSelectText;

        public Sprite KonkyGlobe;
        public Sprite GreyshirtGlobe;
        public RuntimeAnimatorController KonkyGlobeAnim;
        public RuntimeAnimatorController GreyshirtGlobeAnim;
        public RuntimeAnimatorController DkGlobeAnim;
        public RuntimeAnimatorController ShrulkGlobeAnim;

        public Sprite BkgMask;

        //---------------------background sprites------------------//

        public Sprite BackgroundNSprite;
        public Sprite BackgroundRSprite;
        public Sprite Background0Sprite;
        public Sprite Background1Sprite;
        public Sprite Background2Sprite;
        public Sprite Background3Sprite;

        public Sprite Background0Button;
        public Sprite Background1Button;
        public Sprite Background2Button;
        public Sprite Background3Button;
        public Sprite BackgroundRButton;

        //----------------------screen specific key buttons----------------//

        private GameObject _backgroundFrame;
        private GameObject _backgroundShowcase;
        private GameObject _backgroundGoButton;
        private GameObject _backgroundText;

        private GameObject _globe1;
        private GameObject _globe2;
        private GameObject _player1;
        private GameObject _player2;
        private GameObject _characterGoButton;
        private GameObject _characterGoText;
        private GameObject _globeButton1;
        private GameObject _globeButton2;
        private GameObject _cpuButton1;
        private GameObject _cpuText1;
        private GameObject _cpuButton2;
        private GameObject _cpuText2;
        public GameObject[] CharacterButtons;

        private GameObject[] _stageSprites;

        private GameObject _onlineButton;
        private GameObject _onlineText;
        private GameObject _hitboxButton;
        private GameObject _hitboxText;
        private GameObject _matchTimeButton;
        private GameObject _matchTimeText;
        private GameObject _bestOfButton;
        private GameObject _bestOfText;
        private GameObject _controller1Button;
        private GameObject _controller1Text;
        private GameObject _controller2Button;
        private GameObject _controller2Text;

        //-----------------------------prefabs----------------------------//

        public GameObject ButtonPrefab;
        public GameObject SpritePrefab;
        public GameObject TextPrefab;
        public GameObject TextHolderPrefab;

        //--------------------------------------------------------------//

        public GameObject Background;

        public List<GameObject> MenuObjects;
        public List<int> Links;

        private int _screen;
        //--------------------prefs----------------------------------------//

        private int _stageSelect;
        public int Player1C;
        public int Player2C;
        private int _player1Ai;
        private int _player2Ai;

        //settings
        private bool _onlineMode;
        private bool _showHitboxes;
        private int _matchTime;
        private int _bestOf;
        private int _controller1Id = 3;
        private int _controller2Id = 3;

        //win screen
        private int _player1W;
        private int _player2W;

        //-----------------------------------------------------------------//

        private int _backgroundPass;

        private int _globeSelect;

        public IntVariable RoundCounter, Player1Wins, Player2Wins;
        bool _cameFromSettings = false;
    

        void Start()
        {
            RoundCounter.Value = 0;

            ComponentScript.Init(transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, transform.GetChild(2).gameObject, transform.GetChild(3).gameObject, transform.GetChild(4).gameObject, transform.GetChild(5).gameObject);

            //set default settings
            _onlineMode = false;

            StartScreen(PlayerPrefs.GetInt("menu_state"));
        }

        void OnApplicationQuit()
        {
            RoundCounter.Value = 0;
            Player1Wins.Value = 0;
            Player2Wins.Value = 0;
            PlayerPrefs.DeleteAll();
        }

        public GameObject MakeButton(Vector3[] points, Color color, int triggerId, int[] flags)
        {
            GameObject button = Instantiate(ButtonPrefab);

            button.GetComponent<ComponentScript>().MenuScript = this;

            button.GetComponent<ComponentScript>().TriggerId = triggerId;

            Vector2[] l = button.GetComponent<PolygonCollider2D>().points;
            for (int i = 0; i < points.Length; ++i)
            {
                l[i] = points[i];
            }
            button.GetComponent<PolygonCollider2D>().points = l;

            Mesh m = button.GetComponent<MeshFilter>().mesh;
            m = new Mesh();
            m.vertices = points;
            int[] meshTri = m.triangles;
            meshTri = new int[] { 0, 1, 2, 2, 3, 0 };
            m.triangles = meshTri;
            button.GetComponent<MeshFilter>().mesh = m;

            Material mat = button.GetComponent<MeshRenderer>().material;
            mat.color = color;

            MenuObjects.Add(button);

            button.GetComponent<ComponentScript>().DefaultColor = color;
            button.GetComponent<ComponentScript>().Setup(flags, points);

            return button;
        }

        //do not use
        private GameObject SubSprite(float x, float y, int width, int height, Sprite sprite, RuntimeAnimatorController animator, Color color)
        {
            GameObject spriteObject = Instantiate(SpritePrefab);

            RectTransform rect = spriteObject.GetComponent<RectTransform>();
            Vector3 position = rect.position;
            position.x = x;
            position.y = y;
            position.z = -2;
            rect.position = position;

            Vector2 size = rect.sizeDelta;
            size.x = width;
            size.y = height;
            rect.sizeDelta = size;

            SpriteRenderer render = spriteObject.GetComponent<SpriteRenderer>();
            render.sprite = sprite;
            render.color = color;
            render.size = size;

            if (animator != null)
            {
                spriteObject.GetComponent<Animator>().runtimeAnimatorController = animator;
            }
            MenuObjects.Add(spriteObject);

            return spriteObject;
        }

        public GameObject MakeSprite(float x, float y, int w, int h, Sprite s)
        {
            return SubSprite(x, y, w, h, s, null, Color.white);
        }

        public GameObject MakeSprite(float x, float y, int w, int h, Sprite s, RuntimeAnimatorController a)
        {
            return SubSprite(x, y, w, h, s, a, Color.white);
        }

        public GameObject MakeSprite(float x, float y, int w, int h, Sprite s, Color c)
        {
            return SubSprite(x, y, w, h, s, null, c);
        }

        //just plain text, no movement
        public GameObject MakeText(float x, float y, float scale, string text, int allignMode)
        {
            GameObject ret = Instantiate(TextPrefab);

            //set position
            Vector3 p = new Vector3(x, y, 0);
            ret.GetComponent<Transform>().position = p;
            Vector3 s = new Vector3(scale, scale, 1);
            ret.GetComponent<Transform>().localScale = s;

            //set text
            ret.GetComponent<TextMesh>().text = text;

            //set allignment
            switch (allignMode)
            {
                case 0:
                    ret.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    break;
                case 1:
                    ret.GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                    break;
                case 2:
                    ret.GetComponent<TextMesh>().anchor = TextAnchor.LowerLeft;
                    break;
            }

            //add the text object to the list
            MenuObjects.Add(ret);

            return ret;
        }

        //make text that has an animation, moves up and down with different colors
        public GameObject MakeFancyText(float x, float y, float scale, string text, int allignMode)
        {
            GameObject ret = Instantiate(TextHolderPrefab);

            //set position and scale
            Vector3 p = new Vector3(x, y, -8);
            ret.GetComponent<Transform>().position = p;
            Vector3 s = new Vector3(scale, scale, 1);
            ret.GetComponent<Transform>().localScale = s;

            //set all the text to the same thing
            ret.transform.GetChild(0).GetComponent<TextMesh>().text = text;
            ret.transform.GetChild(1).GetComponent<TextMesh>().text = text;
            ret.transform.GetChild(2).GetComponent<TextMesh>().text = text;
            ret.transform.GetChild(3).GetComponent<TextMesh>().text = text;

            //set the allignment of all the texts
            switch (allignMode)
            {
                case 0:
                    ret.transform.GetChild(0).GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    ret.transform.GetChild(1).GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    ret.transform.GetChild(2).GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    ret.transform.GetChild(3).GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    break;
                case 1:
                    ret.transform.GetChild(0).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                    ret.transform.GetChild(1).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                    ret.transform.GetChild(2).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                    ret.transform.GetChild(3).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
                    break;
                case 2:
                    ret.transform.GetChild(0).GetComponent<TextMesh>().anchor = TextAnchor.LowerLeft;
                    ret.transform.GetChild(1).GetComponent<TextMesh>().anchor = TextAnchor.LowerLeft;
                    ret.transform.GetChild(2).GetComponent<TextMesh>().anchor = TextAnchor.LowerLeft;
                    ret.transform.GetChild(3).GetComponent<TextMesh>().anchor = TextAnchor.LowerLeft;
                    break;
            }

            //add the text object to the list
            MenuObjects.Add(ret);

            return ret;
        }

        //change the text of animated text objects
        private void ChangeFancyText(GameObject textObject, string text)
        {

            textObject.transform.GetChild(0).GetComponent<TextMesh>().text = text;
            textObject.transform.GetChild(1).GetComponent<TextMesh>().text = text;
            textObject.transform.GetChild(2).GetComponent<TextMesh>().text = text;
            textObject.transform.GetChild(3).GetComponent<TextMesh>().text = text;
        }

        private void ChangeText(GameObject textObject, string text)
        {
            textObject.GetComponent<TextMesh>().text = text;
        }

        //destroy all menu objects created by this script
        public void MenuClear()
        {
            foreach (GameObject i in MenuObjects)
            {
                //destroy the gameObject in unity
                Destroy(i);
            }
            //clear the list
            MenuObjects.Clear();
        }

        //switch to pre designated arrangements of menu objects and create them
        public void StartScreen(int no)
        {
            MenuClear();
            switch (no)
            {
                case TitleScreen:
                    if (!_cameFromSettings)
                        AudioManager.Instance.PlayMusic(AudioManager.Music.MenuTheme);
                    else
                        _cameFromSettings = false;

                    _controller1Id = 1;
                    _controller2Id = 2;
                    InputManager.C1Id = _controller1Id;
                    InputManager.C2Id = _controller2Id;
                

                    _player1Ai = 0;
                    _player2Ai = 0;
                    Player1C = -1;
                    Player2C = -1;
                    _globeSelect = 0;

                    //reset player wins
                    PlayerPrefs.SetInt("player1w", 0);
                    PlayerPrefs.SetInt("player2w", 0);

                    MakeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 1, new int[] { ComponentScript.FlagHidden });//hidden play button
                    MakeFancyText(0, -3, 2f, "play", 0);//play button text

                    MakeButton(new Vector3[] { new Vector2(-6, -5), new Vector2(6, -5), new Vector2(6, -9), new Vector2(-6, -9) }, new Color(0.8f, 0f, 0f, 0.75f), 15, new int[] { ComponentScript.FlagHidden });//hidden play button
                    MakeFancyText(0, -7, 1.5f, "settings", 0);//settings button text

                    MakeSprite(0, 3, 26, 11, TitleSprite);//title logo
                    break;
                case PlayerSelectScreen:
                    AudioManager.Instance.PlayMusic(AudioManager.Music.VsTheme);

                    Player1C = -1;
                    Player2C = -1;

                    Player1Wins.Value = 0;
                    Player2Wins.Value = 0;

                    _globe1 = MakeSprite(-11, -6, 8, 2, PlatformSprite, Color.red);
                    _globe1.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    _globeButton1 = MakeButton(new Vector3[] { new Vector2(-11, -4.75f), new Vector2(-6f, -6f), new Vector2(-11, -7.25f), new Vector2(-16f, -6) }, new Color(0f, 0f, 0f, 0f), 9, new int[] { ComponentScript.FlagNoline });//
                    _player1 = MakeSprite(-11, -6, 10, 10, KonkyGlobe, KonkyGlobeAnim);
                    _player1.GetComponent<SpriteRenderer>().sortingOrder = 90;
                    _cpuButton1 = MakeButton(new Vector3[] { new Vector2(-10.5f, -7f), new Vector2(-6.5f, -7f), new Vector2(-6.5f, -8.5f), new Vector2(-10.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 13, new int[] { });
                    _cpuText1 = MakeFancyText(-8.5f, -7.75f, 0.5f, "", 0);
                    //makeText(-14, 0, 0.75f, "wins\n" + PlayerPrefs.GetInt("player1w"), 0);


                    _globe2 = MakeSprite(11, -6, 8, 2, PlatformSprite, Color.red);
                    _globe2.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    _globeButton2 = MakeButton(new Vector3[] { new Vector2(11, -4.75f), new Vector2(6f, -6f), new Vector2(11, -7.25f), new Vector2(16f, -6) }, new Color(0f, 0f, 0f, 0f), 10, new int[] { ComponentScript.FlagNoline });// 
                    _player2 = MakeSprite(11, -6, 10, 10, KonkyGlobe, KonkyGlobeAnim);
                    _player2.GetComponent<SpriteRenderer>().sortingOrder = 90; _player2.GetComponent<SpriteRenderer>().flipX = true;
                    _cpuButton2 = MakeButton(new Vector3[] { new Vector2(6.5f, -7f), new Vector2(10.5f, -7f), new Vector2(10.5f, -8.5f), new Vector2(6.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 14, new int[] { });
                    _cpuText2 = MakeFancyText(8.5f, -7.75f, 0.5f, "", 0);
                    //makeText(14, 0, 0.75f, "wins\n" + PlayerPrefs.GetInt("player2w"), 0);

                    _characterGoButton = MakeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 2, new int[] { ComponentScript.FlagHidden, ComponentScript.FlagDummy });
                    _characterGoText = MakeFancyText(0, -3, 2, "go", 0);
                    _characterGoText.SetActive(false);

                    CharacterButtons = new GameObject[6];

                    CharacterButtons[0] = MakeButton(new Vector3[] { new Vector2(-15, 7), new Vector2(-10, 7), new Vector2(-7, 0), new Vector2(-11, 0) }, new Color(0.25f, 0.2f, 0.2f, 0.75f), 11, new int[] { ComponentScript.FlagSticky });
                    MakeSprite(-10, 4, 8, 8, KonkySelect).GetComponent<SpriteRenderer>().sortingOrder = 5;//konky sprite
                    CharacterButtons[1] = MakeButton(new Vector3[] { new Vector2(-10, 7), new Vector2(-5, 7), new Vector2(-3, 0), new Vector2(-7, 0) }, new Color(1f, 0.5f, 0.5f, 0.75f), 12, new int[] { ComponentScript.FlagSticky });
                    MakeSprite(-6, 4, 8, 8, GreyshirtSelect).GetComponent<SpriteRenderer>().sortingOrder = 5;//greyshirt sprite
                    CharacterButtons[2] = MakeButton(new Vector3[] { new Vector2(-5, 7), new Vector2(0, 7), new Vector2(0, 0), new Vector2(-3, 0) }, new Color(0.1f, 0.9f, 0.1f, 0.75f), -1, new int[] { });
                    MakeSprite(-2, 4, 8, 8, DkSelect).GetComponent<SpriteRenderer>().sortingOrder = 5;//dk sprite
                    CharacterButtons[3] = MakeButton(new Vector3[] { new Vector2(0, 7), new Vector2(5, 7), new Vector2(3, 0), new Vector2(0, 0) }, new Color(0.9f, 0.6f, 0.1f, 0.75f), -3, new int[] { });
                    MakeSprite(2, 4, 8, 8, ShrulkSelect).GetComponent<SpriteRenderer>().sortingOrder = 5;//shrulk sprite

                    //makeButton(new Vector3[] { new Vector2(0, 7), new Vector2(5, 7), new Vector2(3, 0), new Vector2(0, 0) }, new Color(0.9f, 0.6f, 0.1f, 0.75f), -1, new int[] { });
                    MakeButton(new Vector3[] { new Vector2(5, 7), new Vector2(10, 7), new Vector2(7, 0), new Vector2(3, 0) }, new Color(0.00f, 0.1f, 0.9f, 0.75f), -1, new int[] { });
                    MakeButton(new Vector3[] { new Vector2(10, 7), new Vector2(15, 7), new Vector2(11, 0), new Vector2(7, 0) }, new Color(0.6f, 0.6f, 0.6f, 0.75f), -1, new int[] { });
                    // makeButton(new Vector3[] { new Vector2(15, 7), new Vector2(20, 7), new Vector2(15, 0), new Vector2(10, 0) }, new Color(0.6f, 0.6f, 0.6f, 0.75f), -1, new int[] { });

                    MakeText(-16f, 9f, 1.25f, "player select", 1);//screen description

                    MakeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 0, new int[] { ComponentScript.FlagHidden });
                    MakeFancyText(-16f, -9f, 1f, "back", 2);//back button

                    GlobeShift();
                    CharShift();
                    GlobeShift();

                    break;
                case StageSelectScreen:

                

                    _backgroundFrame = MakeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, _konkyRed, -1, new int[] { ComponentScript.FlagDummy, ComponentScript.FlagDecoration });
                    _backgroundFrame.GetComponent<MeshRenderer>().sortingOrder = 1;

                    _backgroundShowcase = MakeSprite(0, 2, 32, 8, BackgroundRSprite);
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundNSprite;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    SpriteMask ma = _backgroundShowcase.AddComponent(typeof(SpriteMask)) as SpriteMask;
                    ma.sprite = BkgMask;

                    _backgroundGoButton = MakeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, new Color(1f, 1f, 1f, 0f), 3, new int[] { ComponentScript.FlagDummy, ComponentScript.FlagShade, ComponentScript.FlagDecoration });
                    _backgroundGoButton.GetComponent<MeshRenderer>().sortingOrder = 1;

                    _stageSprites = new GameObject[5];

                    MakeButton(new Vector3[] { new Vector2(-10, -2), new Vector2(-6, -2), new Vector2(-5, -6), new Vector2(-9, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 4, new int[] { ComponentScript.FlagSticky, ComponentScript.FlagClearLock }).GetComponent<MeshRenderer>().sortingOrder = 1;
                    _stageSprites[0] = MakeSprite(-7.5f, -4, 5, 4, Background0Button);
                    _stageSprites[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
                    ChangeSpriteColor(_stageSprites[0], _konkyRed);
                    MakeButton(new Vector3[] { new Vector2(-6, -2), new Vector2(-2, -2), new Vector2(-1, -6), new Vector2(-5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 5, new int[] { ComponentScript.FlagSticky, ComponentScript.FlagClearLock }).GetComponent<MeshRenderer>().sortingOrder = 1;
                    _stageSprites[1] = MakeSprite(-3.5f, -4, 5, 4, Background1Button);
                    _stageSprites[1].GetComponent<SpriteRenderer>().sortingOrder = 1;
                    ChangeSpriteColor(_stageSprites[1], _konkyRed);
                    MakeButton(new Vector3[] { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(1, -6), new Vector2(-1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 6, new int[] { ComponentScript.FlagSticky, ComponentScript.FlagClearLock }).GetComponent<MeshRenderer>().sortingOrder = 1;
                    _stageSprites[2] = MakeSprite(0, -4, 5, 4, BackgroundRButton);
                    _stageSprites[2].GetComponent<SpriteRenderer>().sortingOrder = 1;
                    ChangeSpriteColor(_stageSprites[2], _konkyRed);
                    MakeButton(new Vector3[] { new Vector2(2, -2), new Vector2(6, -2), new Vector2(5, -6), new Vector2(1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 7, new int[] { ComponentScript.FlagSticky, ComponentScript.FlagClearLock }).GetComponent<MeshRenderer>().sortingOrder = 1;
                    _stageSprites[3] = MakeSprite(3.5f, -4, 5, 4, Background2Button);
                    _stageSprites[3].GetComponent<SpriteRenderer>().sortingOrder = 1;
                    ChangeSpriteColor(_stageSprites[3], _konkyRed);
                    MakeButton(new Vector3[] { new Vector2(6, -2), new Vector2(10, -2), new Vector2(9, -6), new Vector2(5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 8, new int[] { ComponentScript.FlagSticky, ComponentScript.FlagClearLock }).GetComponent<MeshRenderer>().sortingOrder = 1;
                    _stageSprites[4] = MakeSprite(7.5f, -4, 5, 4, Background3Button);
                    _stageSprites[4].GetComponent<SpriteRenderer>().sortingOrder = 1;
                    ChangeSpriteColor(_stageSprites[4], _konkyRed);

                    _backgroundText = MakeText(0, -7.25f, 1.125f, "select a stage", 0);//background text

                    MakeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 1, new int[] { ComponentScript.FlagHidden });
                    MakeFancyText(-16f, -9f, 1f, "back", 2);//back button

                    MakeText(-16f, 9f, 1.25f, "stage select", 1);//screen description
                    break;
                case SettingsScreen:
                    _controller1Id = InputManager.C1Id;
                    _controller2Id = InputManager.C2Id;
                    _cameFromSettings = true;
                    MakeText(-16f, 9f, 1.25f, "settings", 1);//screen description

                    //makeText(-14f, 6.5f, 1f, "multiplayer mode", 1);

                    //onlineButton = makeButton(new Vector3[] { new Vector2(-14, 5), new Vector2(-7, 5), new Vector2(-7, 3), new Vector2(-14, 3) }, new Color(0.8f, 0f, 0f, 0.75f), -1, new int[] { });
                    //onlineText = makeFancyText(-10.5f, 4, 1f, "local", 0);

                    MakeText(-14f, 6.5f, 1f, "hitboxes", 1);

                    _hitboxButton = MakeButton(new Vector3[] { new Vector2(-14, 5), new Vector2(-7, 5), new Vector2(-7, 3), new Vector2(-14, 3) }, new Color(0.8f, 0f, 0f, 0.75f), 16, new int[] { });
                    _hitboxText = MakeFancyText(-10.5f, 4, 1f, "hidden", 0);

                    MakeText(-14f, 2.5f, 1f, "match time", 1);

                    _matchTimeButton = MakeButton(new Vector3[] { new Vector2(-14, 1), new Vector2(-7, 1), new Vector2(-7, -1), new Vector2(-14, -1) }, new Color(0.8f, 0f, 0f, 0.75f), 17, new int[] { });
                    _matchTimeText = MakeFancyText(-10.5f, 0, 1f, "99", 0);

                    MakeText(-14f, -1.5f, 1f, "best of", 1);

                    _bestOfButton = MakeButton(new Vector3[] { new Vector2(-14, -3), new Vector2(-7, -3), new Vector2(-7, -5), new Vector2(-14, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 18, new int[] { });
                    _bestOfText = MakeFancyText(-10.5f, -4, 1f, "3", 0);

                    MakeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 0, new int[] { ComponentScript.FlagHidden });
                    MakeFancyText(-16f, -9f, 1f, "back", 2);//back button

                    MakeText(0f, 6.5f, 1f, "controller 1", 1);
                    _controller1Button = MakeButton(new Vector3[] { new Vector2(9f, 4), new Vector2(2, 4), new Vector2(2, 2), new Vector2(9f, 2) }, new Color(0.8f, 0f, 0f, 0.75f), 19, new int[] { });
                    _controller1Text = MakeFancyText(5.5f, 3, 1f, "1", 0);

                    MakeText(0f, 0f, 1f, "controller 2", 1);
                    _controller2Button = MakeButton(new Vector3[] { new Vector2(9f, -2.5f), new Vector2(2, -2.5f), new Vector2(2, -4.5f), new Vector2(9f, -4.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 20, new int[] { });
                    _controller2Text = MakeFancyText(5.5f, -3.5f, 1f, "2", 0);

                    SettingsShift();

                    break;
            }
        }

        public void TriggerEvent(int id)
        {
            Debug.Log(id);
            switch (id)
            {
                case 0:
                    StartScreen(TitleScreen);
                    break;
                case 1:
                    StartScreen(PlayerSelectScreen);
                    break;
                case 2:
                    StartScreen(StageSelectScreen);
                    break;
                case 3:
                    BeginGame();
                    break;
                case 4:
                    UnstickAll(false);
                    ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
                    ChangeSpriteColor(_stageSprites[0], Color.white);
                    _backgroundPass = 0;
                    _backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background0Sprite;
                    ChangeText(_backgroundText, "twlight hills");
                    _backgroundGoButton.GetComponent<ComponentScript>().Unstick();
                    break;
                case 5:
                    UnstickAll(false);
                    ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
                    ChangeSpriteColor(_stageSprites[1], Color.white);
                    _backgroundPass = 1;
                    _backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background1Sprite;
                    ChangeText(_backgroundText, "africa");
                    _backgroundGoButton.GetComponent<ComponentScript>().Unstick();
                    break;
                case 6:
                    UnstickAll(false);
                    ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
                    ChangeSpriteColor(_stageSprites[2], Color.white);
                    _backgroundPass = 2;
                    _backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundRSprite;
                    ChangeText(_backgroundText, "random");
                    _backgroundGoButton.GetComponent<ComponentScript>().Unstick();
                    break;
                case 7:
                    UnstickAll(false);
                    ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
                    ChangeSpriteColor(_stageSprites[3], Color.white);
                    _backgroundPass = 3;
                    _backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background2Sprite;
                    ChangeText(_backgroundText, "midnight park");
                    _backgroundGoButton.GetComponent<ComponentScript>().Unstick();
                    break;
                case 8:
                    UnstickAll(false);
                    ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
                    ChangeSpriteColor(_stageSprites[4], Color.white);
                    _backgroundPass = 4;
                    _backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                    _backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background3Sprite;
                    ChangeText(_backgroundText, "training grounds");
                    _backgroundGoButton.GetComponent<ComponentScript>().Unstick();
                    break;
                case 9:
               
                    break;
                case 10:
                
                    break;
                case 11:
               
                    break;
                case 12:
               
                    break;
                case 13:
                    _player1Ai = (_player1Ai == 0) ? 1 : 0;
                    GlobeShift();
                    break;
                case 14:
                    _player2Ai = (_player2Ai == 0) ? 1 : 0;
                    GlobeShift();
                    break;
                case 15:
                    StartScreen(SettingsScreen);
                    break;
                case 16:
                    _showHitboxes = !_showHitboxes;
                    SettingsShift();
                    break;
                case 17:
                    ++_matchTime;
                    _matchTime %= _times.Length;
                    SettingsShift();
                    break;
                case 18:
                    ++_bestOf;
                    _bestOf %= _bestofs.Length;
                    SettingsShift();
                    break;
                case 19:
                    _controller1Id++;
                    if (_controller1Id > 3)
                        if (_controller2Id > 1)
                            _controller1Id = 1;
                        else
                            _controller1Id = 2;

                    if (_controller1Id == _controller2Id && _controller1Id < 3)
                        TriggerEvent(19);
                    
                    SettingsShift();
                    break;
                case 20:
                    _controller2Id++;
                    if (_controller2Id > 3)
                        if (_controller1Id > 1)
                            _controller2Id = 1;
                        else
                            _controller2Id = 2;

                    if (_controller1Id == _controller2Id && _controller1Id < 3)
                        TriggerEvent(20);

                    SettingsShift();
                    break;
            }
        }

        public void UpdateChar(int state, bool player)
        {
            if (player)
                Player2C = state;
            else
                Player1C = state;
            CharShift();
            GlobeShift();
        }

        public void UpdateSelection(int state1, int state2)
        {
            CharacterButtons[0].GetComponent<ComponentScript>().Unstick();
            CharacterButtons[1].GetComponent<ComponentScript>().Unstick();
            CharacterButtons[2].GetComponent<ComponentScript>().Unstick();
            CharacterButtons[3].GetComponent<ComponentScript>().Unstick();

            if (state1 == state2)
            {
                CharacterButtons[state1].GetComponent<ComponentScript>().SetGlow(3);
            }
            else
            {
                CharacterButtons[state1].GetComponent<ComponentScript>().SetGlow(1);
                CharacterButtons[state2].GetComponent<ComponentScript>().SetGlow(2);
            }

            CharacterButtons[state1].GetComponent<ComponentScript>().Stick();
            CharacterButtons[state2].GetComponent<ComponentScript>().Stick();
            /*switch (state)
        {
            // 0  0
            case 0:
                characterButtons[0].GetComponent<ComponentScript>().setGlow(3);
                characterButtons[0].GetComponent<ComponentScript>().stick();
                break;

            // 0  1
            case 1:
                characterButtons[1].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[0].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                characterButtons[0].GetComponent<ComponentScript>().stick();
                break;

            // 0  2
            case 2:
                characterButtons[2].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[0].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[2].GetComponent<ComponentScript>().stick();
                characterButtons[0].GetComponent<ComponentScript>().stick();
                break;

            // 1  0
            case 3:
                characterButtons[0].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[1].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                characterButtons[0].GetComponent<ComponentScript>().stick();
                break;

            // 1  1
            case 4:
                characterButtons[1].GetComponent<ComponentScript>().setGlow(3);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                break;

            // 1  2
            case 5:
                characterButtons[2].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[1].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                characterButtons[2].GetComponent<ComponentScript>().stick();
                break;

            // 2  0
            case 6:
                characterButtons[2].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[0].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[2].GetComponent<ComponentScript>().stick();
                characterButtons[0].GetComponent<ComponentScript>().stick();
                break;

            // 2  1
            case 7:
                characterButtons[2].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[1].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                characterButtons[2].GetComponent<ComponentScript>().stick();
                break;

            // 2  2
            case 8:
                characterButtons[2].GetComponent<ComponentScript>().setGlow(3);
                characterButtons[2].GetComponent<ComponentScript>().stick();
                break;
        }*/
            CharShift();
            GlobeShift();

        }

        public void UnstickAll(bool player)
        {
            foreach (GameObject i in MenuObjects)
            {
                try
                {
                    ComponentScript b = i.GetComponent<ComponentScript>();
                    if (b.Sticky)
                    {
                        //b.setPlayer(player);
                    
                        b.Unstick();
                    }
                }
                catch
                {

                }
            }
        }

        public void BeginGame()
        {
            SceneManager.LoadScene("SKF");
            if (_backgroundPass == 2)
            {
                _backgroundPass = Random.Range(0, 3);
            }
            PlayerPrefs.SetInt("stage", _backgroundPass);
            PlayerPrefs.SetInt("player1c", Player1C);
            PlayerPrefs.SetInt("player2c", Player2C);
            PlayerPrefs.SetInt("player1ai", _player1Ai);
            PlayerPrefs.SetInt("player2ai", _player2Ai);

            PlayerPrefs.SetInt("settingShowHitboxes", _showHitboxes ? 0 : 1);
            PlayerPrefs.SetInt("settingMatchTime", _times[_matchTime]);
            PlayerPrefs.SetInt("settingBestOf", _bestofs[_bestOf]);
        }

        private void GlobeShift()
        {

            _globeButton1.GetComponent<ComponentScript>().SetGlow(0);
            _globeButton2.GetComponent<ComponentScript>().SetGlow(1);
            if (_globeSelect == 0)
            {
                if (Player1C != -1)
                {
                    //characterButtons[player1c].GetComponent<ComponentScript>().stick();
                }
                _globeButton1.GetComponent<ComponentScript>().Stick();
                _globeButton2.GetComponent<ComponentScript>().Unstick();
            }
            else
            {
                if (Player2C != -1)
                {
                    //characterButtons[player2c].GetComponent<ComponentScript>().stick();
                }
                _globeButton1.GetComponent<ComponentScript>().Unstick();
                _globeButton2.GetComponent<ComponentScript>().Stick();
            }

            if (_player1Ai == 1)
            {
                ChangeSpriteColor(_globe1, Color.grey);
                _cpuButton1.GetComponent<ComponentScript>().SetColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
                ChangeFancyText(_cpuText1, "cpu1");
            }
            else
            {
                ChangeSpriteColor(_globe1, Color.red);
                _cpuButton1.GetComponent<ComponentScript>().SetColor(new Color(1f, 0f, 0f, 0.8f));
                ChangeFancyText(_cpuText1, "player1");
            }
            if (_player2Ai == 1)
            {
                ChangeSpriteColor(_globe2, Color.grey);
                _cpuButton2.GetComponent<ComponentScript>().SetColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
                ChangeFancyText(_cpuText2, "cpu2");
            }
            else
            {
                ChangeSpriteColor(_globe2, Color.red);
                _cpuButton2.GetComponent<ComponentScript>().SetColor(new Color(1f, 0f, 0f, 0.8f));
                ChangeFancyText(_cpuText2, "player2");
            }
        }

        private void CharShift()
        {
            bool showGo = true;
            switch (Player1C)
            {
                case -1:
                    ChangeSpriteColor(_player1, Color.clear);
                    showGo = false;
                    break;
                case 0:
                    _player1.GetComponent<Animator>().runtimeAnimatorController = KonkyGlobeAnim;
                    ChangeSpriteColor(_player1, Color.white);
                    break;
                case 1:
                    _player1.GetComponent<Animator>().runtimeAnimatorController = GreyshirtGlobeAnim;
                    ChangeSpriteColor(_player1, Color.white);
                    break;
                case 2:
                    _player1.GetComponent<Animator>().runtimeAnimatorController = DkGlobeAnim;
                    ChangeSpriteColor(_player1, Color.white);
                    break;
                case 3:
                    _player1.GetComponent<Animator>().runtimeAnimatorController = ShrulkGlobeAnim;
                    ChangeSpriteColor(_player1, Color.white);
                    break;
                default:
                    ChangeSpriteColor(_player1, Color.clear);
                    showGo = false;
                    break;
            }
            switch (Player2C)
            {
                case -1:
                    ChangeSpriteColor(_player2, Color.clear);
                    showGo = false;
                    break;
                case 0:
                    _player2.GetComponent<Animator>().runtimeAnimatorController = KonkyGlobeAnim;
                    ChangeSpriteColor(_player2, Color.white);
                    break;
                case 1:
                    _player2.GetComponent<Animator>().runtimeAnimatorController = GreyshirtGlobeAnim;
                    ChangeSpriteColor(_player2, Color.white);
                    break;
                case 2:
                    _player2.GetComponent<Animator>().runtimeAnimatorController = DkGlobeAnim;
                    ChangeSpriteColor(_player2, Color.white);
                    break;
                case 3:
                    _player2.GetComponent<Animator>().runtimeAnimatorController = ShrulkGlobeAnim;
                    ChangeSpriteColor(_player2, Color.white);
                    break;
                default:
                    ChangeSpriteColor(_player2, Color.clear);
                    showGo = false;
                    break;
            }
            if (showGo)
            {
                _characterGoButton.GetComponent<ComponentScript>().Unstick();
                _characterGoText.SetActive(true);
            }
        }

        /*
	private bool showHitboxes;
	private int matchTime;
	private int bestOf;
	 **/

        private void SettingsShift()
        {
            if (_showHitboxes)
                ChangeFancyText(_hitboxText, "shown");
            else
                ChangeFancyText(_hitboxText, "hidden");

            int tempTime = _times[_matchTime];
            if (tempTime == -1)
                ChangeFancyText(_matchTimeText, "inf");
            else
                ChangeFancyText(_matchTimeText, tempTime.ToString());

            ChangeFancyText(_bestOfText, _bestofs[_bestOf].ToString());
            if (_controller1Id >= 3)
                ChangeFancyText(_controller1Text, "key");
            else
                ChangeFancyText(_controller1Text, _controller1Id.ToString());

            if (_controller2Id >= 3)
                ChangeFancyText(_controller2Text, "key");
            else
                ChangeFancyText(_controller2Text, _controller2Id.ToString());
            
            InputManager.C1Id = _controller1Id;
            InputManager.C2Id = _controller2Id;
        }

        private void ChangeSpriteColor(GameObject o, Color c)
        {
            o.GetComponent<SpriteRenderer>().color = c;
        }
    }
}

