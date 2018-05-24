using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    Color KONKY_RED = new Color(0.9f, 0, 0, 1);
    const int TITLE_SCREEN = 0;
    const int PLAYER_SELECT_SCREEN = 1;
    const int STAGE_SELECT_SCREEN = 2;
    const int SETTINGS_SCREEN = 3;

    private readonly int[] TIMES = { 99, 120, 60, -1 };
    private readonly int[] BESTOFS = { 3, 5, 7, 9 };

    //---------------------misc sprites-----------------//

    public Sprite titleSprite;
    public Sprite playButtonSprite;
    public Sprite platformSprite;
    public Sprite konkySelect;
    public Sprite GreyshirtSelect;
    public Sprite PlayerSelectText;
    public Sprite StageSelectText;

    public Sprite konkyGlobe;
    public Sprite greyshirtGlobe;
    public RuntimeAnimatorController konkyGlobeAnim;
    public RuntimeAnimatorController GreyshirtGlobeAnim;

    public Sprite bkgMask;

    //---------------------background sprites------------------//

    public Sprite BackgroundNSprite;
    public Sprite backgroundRSprite;
    public Sprite background0Sprite;
    public Sprite background1Sprite;
    public Sprite background2Sprite;
    public Sprite background3Sprite;

    public Sprite background0Button;
    public Sprite background1Button;
    public Sprite background2Button;
    public Sprite background3Button;
    public Sprite backgroundRButton;

    //----------------------screen specific key buttons----------------//

    private GameObject backgroundFrame;
    private GameObject backgroundShowcase;
    private GameObject backgroundGoButton;
    private GameObject backgroundText;

    private GameObject globe1;
    private GameObject globe2;
    private GameObject player1;
    private GameObject player2;
    private GameObject characterGoButton;
    private GameObject characterGoText;
    private GameObject globeButton1;
    private GameObject globeButton2;
    private GameObject cpuButton1;
    private GameObject cpuText1;
    private GameObject cpuButton2;
    private GameObject cpuText2;
    public GameObject[] characterButtons;

    private GameObject[] stageSprites;

    private GameObject onlineButton;
    private GameObject onlineText;
    private GameObject hitboxButton;
    private GameObject hitboxText;
    private GameObject matchTimeButton;
    private GameObject matchTimeText;
    private GameObject bestOfButton;
    private GameObject bestOfText;
    private GameObject controller1Button;
    private GameObject controller1Text;
    private GameObject controller2Button;
    private GameObject controller2Text;

    //-----------------------------prefabs----------------------------//

    public GameObject buttonPrefab;
    public GameObject spritePrefab;
    public GameObject textPrefab;
    public GameObject textHolderPrefab;

    //--------------------------------------------------------------//

    public GameObject background;

    public List<GameObject> menuObjects;
    public List<int> links;

    private int screen;
    //--------------------prefs----------------------------------------//

    private int stageSelect;
    public int player1c;
    public int player2c;
    private int player1ai;
    private int player2ai;

    //settings
    private bool onlineMode;
    private bool showHitboxes;
    private int matchTime;
    private int bestOf;
    private int controller1Id = 3;
    private int controller2Id = 3;

    //win screen
    private int player1w;
    private int player2w;

    //-----------------------------------------------------------------//

    private int backgroundPass;

    private int globeSelect;

    public IntVariable roundCounter, player1Wins, player2Wins;
    bool cameFromSettings = false;
    

    void Start()
    {
        roundCounter.value = 0;

        ComponentScript.init(transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, transform.GetChild(2).gameObject, transform.GetChild(3).gameObject, transform.GetChild(4).gameObject, transform.GetChild(5).gameObject);

        //set default settings
        onlineMode = false;

        startScreen(PlayerPrefs.GetInt("menu_state"));
    }

    void OnApplicationQuit()
    {
        roundCounter.value = 0;
        player1Wins.value = 0;
        player2Wins.value = 0;
        PlayerPrefs.DeleteAll();
    }

    public GameObject makeButton(Vector3[] points, Color color, int triggerID, int[] flags)
    {
        GameObject button = Instantiate(buttonPrefab);

        button.GetComponent<ComponentScript>().menuScript = this;

        button.GetComponent<ComponentScript>().triggerID = triggerID;

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

        menuObjects.Add(button);

        button.GetComponent<ComponentScript>().defaultColor = color;
        button.GetComponent<ComponentScript>().setup(flags, points);

        return button;
    }

    //do not use
    private GameObject subSprite(float x, float y, int width, int height, Sprite sprite, RuntimeAnimatorController animator, Color color)
    {
        GameObject spriteObject = Instantiate(spritePrefab);

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
        menuObjects.Add(spriteObject);

        return spriteObject;
    }

    public GameObject makeSprite(float x, float y, int w, int h, Sprite s)
    {
        return subSprite(x, y, w, h, s, null, Color.white);
    }

    public GameObject makeSprite(float x, float y, int w, int h, Sprite s, RuntimeAnimatorController a)
    {
        return subSprite(x, y, w, h, s, a, Color.white);
    }

    public GameObject makeSprite(float x, float y, int w, int h, Sprite s, Color c)
    {
        return subSprite(x, y, w, h, s, null, c);
    }

    //just plain text, no movement
    public GameObject makeText(float x, float y, float scale, string text, int allignMode)
    {
        GameObject ret = Instantiate(textPrefab);

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
        menuObjects.Add(ret);

        return ret;
    }

    //make text that has an animation, moves up and down with different colors
    public GameObject makeFancyText(float x, float y, float scale, string text, int allignMode)
    {
        GameObject ret = Instantiate(textHolderPrefab);

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
        menuObjects.Add(ret);

        return ret;
    }

    //change the text of animated text objects
    private void changeFancyText(GameObject textObject, string text)
    {

        textObject.transform.GetChild(0).GetComponent<TextMesh>().text = text;
        textObject.transform.GetChild(1).GetComponent<TextMesh>().text = text;
        textObject.transform.GetChild(2).GetComponent<TextMesh>().text = text;
        textObject.transform.GetChild(3).GetComponent<TextMesh>().text = text;
    }

    private void changeText(GameObject textObject, string text)
    {
        textObject.GetComponent<TextMesh>().text = text;
    }

    //destroy all menu objects created by this script
    public void menuClear()
    {
        foreach (GameObject i in menuObjects)
        {
            //destroy the gameObject in unity
            Destroy(i);
        }
        //clear the list
        menuObjects.Clear();
    }

    //switch to pre designated arrangements of menu objects and create them
    public void startScreen(int no)
    {
        menuClear();
        switch (no)
        {
            case TITLE_SCREEN:
                if (!cameFromSettings)
                    AudioManager.Instance.PlayMusic(AudioManager.Music.MENU_THEME);
                else
                    cameFromSettings = false;

                player1ai = 0;
                player2ai = 0;
                player1c = -1;
                player2c = -1;
                globeSelect = 0;

                //reset player wins
                PlayerPrefs.SetInt("player1w", 0);
                PlayerPrefs.SetInt("player2w", 0);

                makeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 1, new int[] { ComponentScript.FLAG_HIDDEN });//hidden play button
                makeFancyText(0, -3, 2f, "play", 0);//play button text

                makeButton(new Vector3[] { new Vector2(-6, -5), new Vector2(6, -5), new Vector2(6, -9), new Vector2(-6, -9) }, new Color(0.8f, 0f, 0f, 0.75f), 15, new int[] { ComponentScript.FLAG_HIDDEN });//hidden play button
                makeFancyText(0, -7, 1.5f, "settings", 0);//settings button text

                makeSprite(0, 3, 26, 11, titleSprite);//title logo
                break;
            case PLAYER_SELECT_SCREEN:
                AudioManager.Instance.PlayMusic(AudioManager.Music.VS_THEME);

                player1c = -1;
                player2c = -1;

                player1Wins.value = 0;
                player2Wins.value = 0;

                globe1 = makeSprite(-11, -6, 8, 2, platformSprite, Color.red);
                globe1.GetComponent<SpriteRenderer>().sortingOrder = 7;
                globeButton1 = makeButton(new Vector3[] { new Vector2(-11, -4.75f), new Vector2(-6f, -6f), new Vector2(-11, -7.25f), new Vector2(-16f, -6) }, new Color(0f, 0f, 0f, 0f), 9, new int[] { ComponentScript.FLAG_NOLINE });//
                player1 = makeSprite(-11, -6, 10, 10, konkyGlobe, konkyGlobeAnim);
                player1.GetComponent<SpriteRenderer>().sortingOrder = 90;
                cpuButton1 = makeButton(new Vector3[] { new Vector2(-10.5f, -7f), new Vector2(-6.5f, -7f), new Vector2(-6.5f, -8.5f), new Vector2(-10.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 13, new int[] { });
                cpuText1 = makeFancyText(-8.5f, -7.75f, 0.5f, "", 0);
                makeText(-14, 0, 0.75f, "wins\n" + PlayerPrefs.GetInt("player1w"), 0);


                globe2 = makeSprite(11, -6, 8, 2, platformSprite, Color.red);
                globe2.GetComponent<SpriteRenderer>().sortingOrder = 7;
                globeButton2 = makeButton(new Vector3[] { new Vector2(11, -4.75f), new Vector2(6f, -6f), new Vector2(11, -7.25f), new Vector2(16f, -6) }, new Color(0f, 0f, 0f, 0f), 10, new int[] { ComponentScript.FLAG_NOLINE });// 
                player2 = makeSprite(11, -6, 10, 10, konkyGlobe, konkyGlobeAnim);
                player2.GetComponent<SpriteRenderer>().sortingOrder = 90; player2.GetComponent<SpriteRenderer>().flipX = true;
                cpuButton2 = makeButton(new Vector3[] { new Vector2(6.5f, -7f), new Vector2(10.5f, -7f), new Vector2(10.5f, -8.5f), new Vector2(6.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 14, new int[] { });
                cpuText2 = makeFancyText(8.5f, -7.75f, 0.5f, "", 0);
                makeText(14, 0, 0.75f, "wins\n" + PlayerPrefs.GetInt("player2w"), 0);

                characterGoButton = makeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 2, new int[] { ComponentScript.FLAG_HIDDEN, ComponentScript.FLAG_DUMMY });
                characterGoText = makeFancyText(0, -3, 2, "go", 0);
                characterGoText.SetActive(false);

                characterButtons = new GameObject[6];

                characterButtons[0] = makeButton(new Vector3[] { new Vector2(-15, 7), new Vector2(-10, 7), new Vector2(-7, 0), new Vector2(-11, 0) }, new Color(0.25f, 0.2f, 0.2f, 0.75f), 11, new int[] { ComponentScript.FLAG_STICKY });
                makeSprite(-10, 4, 8, 8, konkySelect).GetComponent<SpriteRenderer>().sortingOrder = 5;//konky sprite
                characterButtons[1] = makeButton(new Vector3[] { new Vector2(-10, 7), new Vector2(-5, 7), new Vector2(-3, 0), new Vector2(-7, 0) }, new Color(1f, 0.5f, 0.5f, 0.75f), 12, new int[] { ComponentScript.FLAG_STICKY });
                makeSprite(-6, 4, 8, 8, GreyshirtSelect).GetComponent<SpriteRenderer>().sortingOrder = 5;//greyshirt sprite
                makeButton(new Vector3[] { new Vector2(-5, 7), new Vector2(0, 7), new Vector2(0, 0), new Vector2(-3, 0) }, new Color(0.1f, 0.9f, 0.1f, 0.75f), -1, new int[] { });
                makeButton(new Vector3[] { new Vector2(0, 7), new Vector2(5, 7), new Vector2(3, 0), new Vector2(0, 0) }, new Color(0.9f, 0.6f, 0.1f, 0.75f), -1, new int[] { });
                makeButton(new Vector3[] { new Vector2(5, 7), new Vector2(10, 7), new Vector2(7, 0), new Vector2(3, 0) }, new Color(0.00f, 0.1f, 0.9f, 0.75f), -1, new int[] { });
                makeButton(new Vector3[] { new Vector2(10, 7), new Vector2(15, 7), new Vector2(11, 0), new Vector2(7, 0) }, new Color(0.6f, 0.6f, 0.6f, 0.75f), -1, new int[] { });

                makeText(-16f, 9f, 1.25f, "player select", 1);//screen description

                makeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 0, new int[] { ComponentScript.FLAG_HIDDEN });
                makeFancyText(-16f, -9f, 1f, "back", 2);//back button

                globeShift();
                charShift();
                globeShift();

                break;
            case STAGE_SELECT_SCREEN:

                

                backgroundFrame = makeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, KONKY_RED, -1, new int[] { ComponentScript.FLAG_DUMMY, ComponentScript.FLAG_DECORATION });
                backgroundFrame.GetComponent<MeshRenderer>().sortingOrder = 1;

                backgroundShowcase = makeSprite(0, 2, 32, 18, backgroundRSprite);
                backgroundShowcase.GetComponent<SpriteRenderer>().sortingOrder = 0;
                backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundNSprite;
                backgroundShowcase.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                SpriteMask ma = backgroundShowcase.AddComponent(typeof(SpriteMask)) as SpriteMask;
                ma.sprite = bkgMask;

                backgroundGoButton = makeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, new Color(1f, 1f, 1f, 0f), 3, new int[] { ComponentScript.FLAG_DUMMY, ComponentScript.FLAG_SHADE, ComponentScript.FLAG_DECORATION });
                backgroundGoButton.GetComponent<MeshRenderer>().sortingOrder = 1;

                stageSprites = new GameObject[5];

                makeButton(new Vector3[] { new Vector2(-10, -2), new Vector2(-6, -2), new Vector2(-5, -6), new Vector2(-9, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 4, new int[] { ComponentScript.FLAG_STICKY, ComponentScript.FLAG_CLEAR_LOCK }).GetComponent<MeshRenderer>().sortingOrder = 1;
                stageSprites[0] = makeSprite(-7.5f, -4, 5, 4, background0Button);
                stageSprites[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
                changeSpriteColor(stageSprites[0], KONKY_RED);
                makeButton(new Vector3[] { new Vector2(-6, -2), new Vector2(-2, -2), new Vector2(-1, -6), new Vector2(-5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 5, new int[] { ComponentScript.FLAG_STICKY, ComponentScript.FLAG_CLEAR_LOCK }).GetComponent<MeshRenderer>().sortingOrder = 1;
                stageSprites[1] = makeSprite(-3.5f, -4, 5, 4, background1Button);
                stageSprites[1].GetComponent<SpriteRenderer>().sortingOrder = 1;
                changeSpriteColor(stageSprites[1], KONKY_RED);
                makeButton(new Vector3[] { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(1, -6), new Vector2(-1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 6, new int[] { ComponentScript.FLAG_STICKY, ComponentScript.FLAG_CLEAR_LOCK }).GetComponent<MeshRenderer>().sortingOrder = 1;
                stageSprites[2] = makeSprite(0, -4, 5, 4, backgroundRButton);
                stageSprites[2].GetComponent<SpriteRenderer>().sortingOrder = 1;
                changeSpriteColor(stageSprites[2], KONKY_RED);
                makeButton(new Vector3[] { new Vector2(2, -2), new Vector2(6, -2), new Vector2(5, -6), new Vector2(1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 7, new int[] { ComponentScript.FLAG_STICKY, ComponentScript.FLAG_CLEAR_LOCK }).GetComponent<MeshRenderer>().sortingOrder = 1;
                stageSprites[3] = makeSprite(3.5f, -4, 5, 4, background2Button);
                stageSprites[3].GetComponent<SpriteRenderer>().sortingOrder = 1;
                changeSpriteColor(stageSprites[3], KONKY_RED);
                makeButton(new Vector3[] { new Vector2(6, -2), new Vector2(10, -2), new Vector2(9, -6), new Vector2(5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 8, new int[] { ComponentScript.FLAG_STICKY, ComponentScript.FLAG_CLEAR_LOCK }).GetComponent<MeshRenderer>().sortingOrder = 1;
                stageSprites[4] = makeSprite(7.5f, -4, 5, 4, background3Button);
                stageSprites[4].GetComponent<SpriteRenderer>().sortingOrder = 1;
                changeSpriteColor(stageSprites[4], KONKY_RED);

                backgroundText = makeText(0, -7.25f, 1.125f, "select a stage", 0);//background text

                makeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 1, new int[] { ComponentScript.FLAG_HIDDEN });
                makeFancyText(-16f, -9f, 1f, "back", 2);//back button

                makeText(-16f, 9f, 1.25f, "stage select", 1);//screen description
                break;
            case SETTINGS_SCREEN:
                controller1Id = InputManager.c1id;
                controller2Id = InputManager.c2id;
                cameFromSettings = true;
                makeText(-16f, 9f, 1.25f, "settings", 1);//screen description

                //makeText(-14f, 6.5f, 1f, "multiplayer mode", 1);

                //onlineButton = makeButton(new Vector3[] { new Vector2(-14, 5), new Vector2(-7, 5), new Vector2(-7, 3), new Vector2(-14, 3) }, new Color(0.8f, 0f, 0f, 0.75f), -1, new int[] { });
                //onlineText = makeFancyText(-10.5f, 4, 1f, "local", 0);

                makeText(-14f, 6.5f, 1f, "hitboxes", 1);

                hitboxButton = makeButton(new Vector3[] { new Vector2(-14, 5), new Vector2(-7, 5), new Vector2(-7, 3), new Vector2(-14, 3) }, new Color(0.8f, 0f, 0f, 0.75f), 16, new int[] { });
                hitboxText = makeFancyText(-10.5f, 4, 1f, "hidden", 0);

                makeText(-14f, 2.5f, 1f, "match time", 1);

                matchTimeButton = makeButton(new Vector3[] { new Vector2(-14, 1), new Vector2(-7, 1), new Vector2(-7, -1), new Vector2(-14, -1) }, new Color(0.8f, 0f, 0f, 0.75f), 17, new int[] { });
                matchTimeText = makeFancyText(-10.5f, 0, 1f, "99", 0);

                makeText(-14f, -1.5f, 1f, "best of", 1);

                bestOfButton = makeButton(new Vector3[] { new Vector2(-14, -3), new Vector2(-7, -3), new Vector2(-7, -5), new Vector2(-14, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 18, new int[] { });
                bestOfText = makeFancyText(-10.5f, -4, 1f, "3", 0);

                makeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 0, new int[] { ComponentScript.FLAG_HIDDEN });
                makeFancyText(-16f, -9f, 1f, "back", 2);//back button

                makeText(0f, 6.5f, 1f, "controller 1", 1);
                controller1Button = makeButton(new Vector3[] { new Vector2(9f, 4), new Vector2(2, 4), new Vector2(2, 2), new Vector2(9f, 2) }, new Color(0.8f, 0f, 0f, 0.75f), 19, new int[] { });
                controller1Text = makeFancyText(5.5f, 3, 1f, "1", 0);

                makeText(0f, 0f, 1f, "controller 2", 1);
                controller2Button = makeButton(new Vector3[] { new Vector2(9f, -2.5f), new Vector2(2, -2.5f), new Vector2(2, -4.5f), new Vector2(9f, -4.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 20, new int[] { });
                controller2Text = makeFancyText(5.5f, -3.5f, 1f, "2", 0);

                settingsShift();

                break;
        }
    }

    public void triggerEvent(int id)
    {
        Debug.Log(id);
        switch (id)
        {
            case 0:
                startScreen(TITLE_SCREEN);
                break;
            case 1:
                startScreen(PLAYER_SELECT_SCREEN);
                break;
            case 2:
                startScreen(STAGE_SELECT_SCREEN);
                break;
            case 3:
                beginGame();
                break;
            case 4:
                unstickAll(false);
                changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
                changeSpriteColor(stageSprites[0], Color.white);
                backgroundPass = 0;
                backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background0Sprite;
                changeText(backgroundText, "twlight hills");
                backgroundGoButton.GetComponent<ComponentScript>().unstick();
                break;
            case 5:
                unstickAll(false);
                changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
                changeSpriteColor(stageSprites[1], Color.white);
                backgroundPass = 1;
                backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background1Sprite;
                changeText(backgroundText, "africa");
                backgroundGoButton.GetComponent<ComponentScript>().unstick();
                break;
            case 6:
                unstickAll(false);
                changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
                changeSpriteColor(stageSprites[2], Color.white);
                backgroundPass = 2;
                backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                backgroundShowcase.GetComponent<SpriteRenderer>().sprite = backgroundRSprite;
                changeText(backgroundText, "random");
                backgroundGoButton.GetComponent<ComponentScript>().unstick();
                break;
            case 7:
                unstickAll(false);
                changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
                changeSpriteColor(stageSprites[3], Color.white);
                backgroundPass = 3;
                backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background2Sprite;
                changeText(backgroundText, "midnight park");
                backgroundGoButton.GetComponent<ComponentScript>().unstick();
                break;
            case 8:
                unstickAll(false);
                changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
                changeSpriteColor(stageSprites[4], Color.white);
                backgroundPass = 4;
                backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
                backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background3Sprite;
                changeText(backgroundText, "training grounds");
                backgroundGoButton.GetComponent<ComponentScript>().unstick();
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
                player1ai = (player1ai == 0) ? 1 : 0;
                globeShift();
                break;
            case 14:
                player2ai = (player2ai == 0) ? 1 : 0;
                globeShift();
                break;
            case 15:
                startScreen(SETTINGS_SCREEN);
                break;
            case 16:
                showHitboxes = !showHitboxes;
                settingsShift();
                break;
            case 17:
                ++matchTime;
                matchTime %= TIMES.Length;
                settingsShift();
                break;
            case 18:
                ++bestOf;
                bestOf %= BESTOFS.Length;
                settingsShift();
                break;
            case 19:
                controller1Id++;
                if (controller1Id > 3)
                    controller1Id = 1;
                if (controller1Id == 1 && controller2Id == 1)
                    
                settingsShift();
                break;
            case 20:
                controller2Id++;
                if (controller2Id > 3)
                    controller2Id = 1;
                if (controller1Id == controller2Id && controller1Id != 3)
                    triggerEvent(19);
                settingsShift();
                break;
        }
    }

    public void updateChar(int state, bool player)
    {
        if (player)
            player2c = state;
        else
            player1c = state;
            charShift();
        globeShift();
    }

    public void updateSelection(int state)
    {
        characterButtons[0].GetComponent<ComponentScript>().unstick();
        characterButtons[1].GetComponent<ComponentScript>().unstick();
        switch (state)
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

            // 1  0
            case 2:
                characterButtons[0].GetComponent<ComponentScript>().setGlow(1);
                characterButtons[1].GetComponent<ComponentScript>().setGlow(0);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                characterButtons[0].GetComponent<ComponentScript>().stick();
                break;

            // 1  1
            case 3:
                characterButtons[1].GetComponent<ComponentScript>().setGlow(3);
                characterButtons[1].GetComponent<ComponentScript>().stick();
                break;
        }
        charShift();
        globeShift();

    }

    public void unstickAll(bool player)
    {
        foreach (GameObject i in menuObjects)
        {
            try
            {
                ComponentScript b = i.GetComponent<ComponentScript>();
                if (b.sticky)
                {
                    //b.setPlayer(player);
                    
                    b.unstick();
                }
            }
            catch
            {

            }
        }
    }

    public void beginGame()
    {
        SceneManager.LoadScene("SKF");
        if (backgroundPass == 2)
        {
            backgroundPass = Random.Range(0, 3);
        }
        PlayerPrefs.SetInt("stage", backgroundPass);
        PlayerPrefs.SetInt("player1c", player1c);
        PlayerPrefs.SetInt("player2c", player2c);
        PlayerPrefs.SetInt("player1ai", player1ai);
        PlayerPrefs.SetInt("player2ai", player2ai);

        PlayerPrefs.SetInt("settingShowHitboxes", showHitboxes ? 0 : 1);
        PlayerPrefs.SetInt("settingMatchTime", TIMES[matchTime]);
        PlayerPrefs.SetInt("settingBestOf", BESTOFS[bestOf]);
    }

    private void globeShift()
    {

        globeButton1.GetComponent<ComponentScript>().setGlow(0);
        globeButton2.GetComponent<ComponentScript>().setGlow(1);
        if (globeSelect == 0)
        {
            if (player1c != -1)
            {
                //characterButtons[player1c].GetComponent<ComponentScript>().stick();
            }
            globeButton1.GetComponent<ComponentScript>().stick();
            globeButton2.GetComponent<ComponentScript>().unstick();
        }
        else
        {
            if (player2c != -1)
            {
                //characterButtons[player2c].GetComponent<ComponentScript>().stick();
            }
            globeButton1.GetComponent<ComponentScript>().unstick();
            globeButton2.GetComponent<ComponentScript>().stick();
        }

        if (player1ai == 1)
        {
            changeSpriteColor(globe1, Color.grey);
            cpuButton1.GetComponent<ComponentScript>().setColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
            changeFancyText(cpuText1, "cpu1");
        }
        else
        {
            changeSpriteColor(globe1, Color.red);
            cpuButton1.GetComponent<ComponentScript>().setColor(new Color(1f, 0f, 0f, 0.8f));
            changeFancyText(cpuText1, "player1");
        }
        if (player2ai == 1)
        {
            changeSpriteColor(globe2, Color.grey);
            cpuButton2.GetComponent<ComponentScript>().setColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
            changeFancyText(cpuText2, "cpu2");
        }
        else
        {
            changeSpriteColor(globe2, Color.red);
            cpuButton2.GetComponent<ComponentScript>().setColor(new Color(1f, 0f, 0f, 0.8f));
            changeFancyText(cpuText2, "player2");
        }
    }

    private void charShift()
    {
        bool showGo = true;
        switch (player1c)
        {
            case -1:
                changeSpriteColor(player1, Color.clear);
                showGo = false;
                break;
            case 0:
                player1.GetComponent<Animator>().runtimeAnimatorController = konkyGlobeAnim;
                changeSpriteColor(player1, Color.white);
                break;
            case 1:
                player1.GetComponent<Animator>().runtimeAnimatorController = GreyshirtGlobeAnim;
                changeSpriteColor(player1, Color.white);
                break;
            default:
                changeSpriteColor(player1, Color.clear);
                showGo = false;
                break;
        }
        switch (player2c)
        {
            case -1:
                changeSpriteColor(player2, Color.clear);
                showGo = false;
                break;
            case 0:
                player2.GetComponent<Animator>().runtimeAnimatorController = konkyGlobeAnim;
                changeSpriteColor(player2, Color.white);
                break;
            case 1:
                player2.GetComponent<Animator>().runtimeAnimatorController = GreyshirtGlobeAnim;
                changeSpriteColor(player2, Color.white);
                break;
            default:
                changeSpriteColor(player2, Color.clear);
                showGo = false;
                break;
        }
        if (showGo)
        {
            characterGoButton.GetComponent<ComponentScript>().unstick();
            characterGoText.SetActive(true);
        }
    }

    /*
	private bool showHitboxes;
	private int matchTime;
	private int bestOf;
	 **/

    private void settingsShift()
    {
        if (showHitboxes)
            changeFancyText(hitboxText, "shown");
        else
            changeFancyText(hitboxText, "hidden");

        int tempTime = TIMES[matchTime];
        if (tempTime == -1)
            changeFancyText(matchTimeText, "inf");
        else
            changeFancyText(matchTimeText, tempTime.ToString());

        changeFancyText(bestOfText, BESTOFS[bestOf].ToString());
        if (controller1Id >= 3)
            changeFancyText(controller1Text, "key");
        else
            changeFancyText(controller1Text, controller1Id.ToString());

        if (controller2Id >= 3)
            changeFancyText(controller2Text, "key");
        else
            changeFancyText(controller2Text, controller2Id.ToString());
            
        InputManager.c1id = controller1Id;
        InputManager.c2id = controller2Id;
    }

    private void changeSpriteColor(GameObject o, Color c)
    {
        o.GetComponent<SpriteRenderer>().color = c;
    }
}

