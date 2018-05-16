using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OldMenuScript : MonoBehaviour {

	Color KONKY_RED = new Color(0.9f, 0, 0, 1);
	const int TITLE_SCREEN = 0;
	const int PLAYER_SELECT_SCREEN = 1;
	const int STAGE_SELECT_SCREEN = 2;

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
	private GameObject[] characterButtons;

	private GameObject[] stageSprites;

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

	private int stageSelect;
	private int player1c;
	private int player2c;
	private int player1ai;
	private int player2ai;

	private int backgroundPass;

	private int globeSelect;

	void Start () {
        startScreen(PlayerPrefs.GetInt("menu_state"));
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    public GameObject makeButton(Vector3[] points, Color color, int triggerID, int[] flags)
	{
		GameObject button = Instantiate(buttonPrefab);

		button.GetComponent<ButtonScript>().menuScript = this;

		button.GetComponent<ButtonScript>().triggerID = triggerID;

		Vector2[] l = button.GetComponent<PolygonCollider2D>().points;
		for (int i =0; i < points.Length; ++i)
		{
			l[i] = points[i];
		}
		button.GetComponent<PolygonCollider2D>().points = l;

		LineRenderer line = button.GetComponent<LineRenderer>();
		line.SetPosition(0, points[0]);
		line.SetPosition(1, points[1]);
		line.SetPosition(2, points[2]);
		line.SetPosition(3, points[3]);

		Mesh m = button.GetComponent<MeshFilter>().mesh;
		m = new Mesh();
		m.vertices = points;
		int[] meshTri = m.triangles;
		meshTri = new int[] {0,1,2,2,3,0 };
		m.triangles = meshTri;
		button.GetComponent<MeshFilter>().mesh = m;

		Material mat = button.GetComponent<MeshRenderer>().material;
		mat.color = color;

		menuObjects.Add(button);

		button.GetComponent<ButtonScript>().defaultColor = color;
		button.GetComponent<ButtonScript>().startFlags(flags);

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
		switch (no) {
			case TITLE_SCREEN:

				player1ai = 0;
				player2ai = 0;
				player1c = -1;
				player2c = -1;
				globeSelect = 0;

				makeButton(new Vector3[] { new Vector2(-6, -7), new Vector2(6, -7), new Vector2(6, -2), new Vector2(-6, -2) }, new Color(0.8f, 0f, 0f, 0.75f), 1, new int[] { ButtonScript.FLAG_HIDDEN });//hidden play button
				makeFancyText(0, -5, 2f, "play", 0);//play button (visible)
				makeSprite(0, 3, 26, 11, titleSprite);//title logo
				break;
			case PLAYER_SELECT_SCREEN:

				globe1 = makeSprite(-11, -6, 8, 2, platformSprite, Color.red);
				globeButton1 = makeButton(new Vector3[] { new Vector2(-11, -4.75f), new Vector2(-6f, -6f), new Vector2(-11, -7.25f), new Vector2(-16f, -6) }, new Color(0f, 0f, 0f, 0f), 9, new int[] { ButtonScript.FLAG_NOLINE });
				player1 = makeSprite(-11,-6,10,10, konkyGlobe, konkyGlobeAnim);
				player1.GetComponent<SpriteRenderer>().sortingOrder = 90;
				cpuButton1 = makeButton(new Vector3[] { new Vector2(-10.5f, -7f), new Vector2(-6.5f, -7f), new Vector2(-6.5f, -8.5f), new Vector2(-10.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 13, new int[] {});
				cpuText1 = makeFancyText(-8.5f, -7.75f, 0.5f, "", 0);

				globe2 = makeSprite(11, -6, 8, 2, platformSprite, Color.red);
				globeButton2 = makeButton(new Vector3[] { new Vector2(11, -4.75f), new Vector2(6f, -6f), new Vector2(11, -7.25f), new Vector2(16f, -6) }, new Color(0f, 0f, 0f, 0f), 10, new int[] { ButtonScript.FLAG_NOLINE });
				player2 = makeSprite(11, -6, 10, 10, konkyGlobe, GreyshirtGlobeAnim);
				player2.GetComponent<SpriteRenderer>().sortingOrder = 90;
				player2.GetComponent<SpriteRenderer>().flipX = true;
				cpuButton2 = makeButton(new Vector3[] { new Vector2(6.5f, -7f), new Vector2(10.5f, -7f), new Vector2(10.5f, -8.5f), new Vector2(6.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 14, new int[] { });
				cpuText2 = makeFancyText(8.5f, -7.75f, 0.5f, "", 0);

				characterGoButton = makeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 2, new int[] { ButtonScript.FLAG_HIDDEN, ButtonScript.FLAG_DUMMY });
				characterGoText = makeFancyText(0, -3, 2, "go", 0);
				characterGoText.SetActive(false);

				characterButtons = new GameObject[6];

				characterButtons[0] = makeButton(new Vector3[] { new Vector2(-15, 7), new Vector2(-10, 7), new Vector2( -7, 0), new Vector2(-11, 0) }, new Color(0.25f, 0.2f, 0.2f, 0.75f), 11, new int[] { ButtonScript.FLAG_STICKY });
				makeSprite(-10, 4, 8, 8, konkySelect);//konky sprite
				characterButtons[1] = makeButton(new Vector3[] { new Vector2(-10, 7), new Vector2( -5, 7), new Vector2( -3, 0), new Vector2( -7, 0) }, new Color(1f, 0.5f, 0.5f, 0.75f), 12, new int[] { ButtonScript.FLAG_STICKY });
				makeSprite(-6, 4, 8, 8, GreyshirtSelect);//greyshirt sprite
				makeButton(new Vector3[] { new Vector2( -5, 7), new Vector2(  0, 7), new Vector2(  0, 0), new Vector2( -3, 0) }, new Color(0.1f, 0.9f, 0.1f, 0.75f), -1, new int[] { });
				makeButton(new Vector3[] { new Vector2(  0, 7), new Vector2(  5, 7), new Vector2(  3, 0), new Vector2(  0, 0) }, new Color(0.9f, 0.6f, 0.1f, 0.75f), -1, new int[] { });
				makeButton(new Vector3[] { new Vector2(  5, 7), new Vector2( 10, 7), new Vector2(  7, 0), new Vector2(  3, 0) }, new Color(0.00f, 0.1f, 0.9f, 0.75f), -1, new int[] { });
				makeButton(new Vector3[] { new Vector2( 10, 7), new Vector2( 15, 7), new Vector2( 11, 0), new Vector2(  7, 0) }, new Color(0.6f, 0.6f, 0.6f, 0.75f), -1, new int[] { });

				makeText(-16f, 9f, 1.25f, "player select", 1);//screen description

				makeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 0, new int[] { ButtonScript.FLAG_HIDDEN });
				makeFancyText(-16f, -9f, 1f, "back", 2);//back button

				globeShift();
				charShift();

				break;
			case STAGE_SELECT_SCREEN:

				backgroundFrame = makeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, KONKY_RED, -1, new int[] { ButtonScript.FLAG_DUMMY });
				backgroundFrame.GetComponent<MeshRenderer>().sortingOrder = 1;

				backgroundShowcase = makeSprite(0, 2, 32, 8, backgroundRSprite);
				backgroundShowcase.GetComponent<SpriteRenderer>().sortingOrder = 0 ;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundNSprite;

				backgroundGoButton = makeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, new Color(1f, 1f, 1f, 0f), 3, new int[] {ButtonScript.FLAG_DUMMY, ButtonScript.FLAG_SHADE});
				backgroundGoButton.GetComponent<MeshRenderer>().sortingOrder = 1;

				stageSprites = new GameObject[5];

				makeButton(new Vector3[] { new Vector2(-10, -2), new Vector2(-6, -2), new Vector2(-5, -6), new Vector2(-9, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 4, new int[] {ButtonScript.FLAG_STICKY, ButtonScript.FLAG_LOCK_CLEAR}).GetComponent<MeshRenderer>().sortingOrder = 1;
				stageSprites[0] = makeSprite(-7.5f, -4, 5, 4, background0Button);
				stageSprites[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
				changeSpriteColor(stageSprites[0], KONKY_RED);
				makeButton(new Vector3[] { new Vector2(-6, -2), new Vector2(-2, -2), new Vector2(-1, -6), new Vector2(-5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 5, new int[] { ButtonScript.FLAG_STICKY, ButtonScript.FLAG_LOCK_CLEAR }).GetComponent<MeshRenderer>().sortingOrder = 1;
				stageSprites[1] = makeSprite(-3.5f, -4, 5, 4, background1Button);
				stageSprites[1].GetComponent<SpriteRenderer>().sortingOrder = 1;
				changeSpriteColor(stageSprites[1], KONKY_RED);
				makeButton(new Vector3[] { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(1, -6), new Vector2(-1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 6, new int[] { ButtonScript.FLAG_STICKY, ButtonScript.FLAG_LOCK_CLEAR }).GetComponent<MeshRenderer>().sortingOrder = 1;
				stageSprites[2] = makeSprite(0, -4, 5, 4, backgroundRButton);
				stageSprites[2].GetComponent<SpriteRenderer>().sortingOrder = 1;
				changeSpriteColor(stageSprites[2], KONKY_RED);
				makeButton(new Vector3[] { new Vector2(2, -2), new Vector2(6, -2), new Vector2(5, -6), new Vector2(1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 7, new int[] { ButtonScript.FLAG_STICKY, ButtonScript.FLAG_LOCK_CLEAR }).GetComponent<MeshRenderer>().sortingOrder = 1;
				stageSprites[3] = makeSprite(3.5f, -4, 5, 4, background2Button);
				stageSprites[3].GetComponent<SpriteRenderer>().sortingOrder = 1;
				changeSpriteColor(stageSprites[3], KONKY_RED);
				makeButton(new Vector3[] { new Vector2(6, -2), new Vector2(10, -2), new Vector2(9, -6), new Vector2(5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 8, new int[] { ButtonScript.FLAG_STICKY, ButtonScript.FLAG_LOCK_CLEAR }).GetComponent<MeshRenderer>().sortingOrder = 1;
				stageSprites[4] = makeSprite(7.5f, -4, 5, 4, background3Button);
				stageSprites[4].GetComponent<SpriteRenderer>().sortingOrder = 1;
				changeSpriteColor(stageSprites[4], KONKY_RED);

				backgroundText = makeText(0, -7.25f, 1.125f, "select a stage", 0);//background text

				makeButton(new Vector3[] { new Vector2(-16,-7), new Vector2(-11,-7), new Vector2(-11,-9), new Vector2(-16,-9) }, new Color(), 1, new int[] { ButtonScript.FLAG_HIDDEN });
				makeFancyText(-16f, -9f, 1f, "back", 2);//back button

				makeText(-16f, 9f, 1.25f, "stage select", 1);//screen description
				break;

		}
	}

	public void triggerEvent(int id)
	{
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
				unstickAll();
				changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
				changeSpriteColor(stageSprites[0], Color.white);
				backgroundPass = 0;
				backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background0Sprite;
				changeText(backgroundText, "twlight hills");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 5:
				unstickAll();
				changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
				changeSpriteColor(stageSprites[1], Color.white);
				backgroundPass = 1;
				backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background1Sprite;
				changeText(backgroundText, "africa");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 6:
				unstickAll();
				changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
				changeSpriteColor(stageSprites[2], Color.white);
				backgroundPass = 2;
				backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = backgroundRSprite;
				changeText(backgroundText, "random");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 7:
				unstickAll();
				changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
				changeSpriteColor(stageSprites[3], Color.white);
				backgroundPass = 3;
				backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background2Sprite;
				changeText(backgroundText, "midnight park");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 8:
				unstickAll();
				changeSpriteColor(stageSprites[backgroundPass], KONKY_RED);
				changeSpriteColor(stageSprites[4], Color.white);
				backgroundPass = 4;
				backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background3Sprite;
				changeText(backgroundText, "training grounds");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 9:
				globeSelect = 0;
				unstickAll();
				globeShift();
				break;
			case 10:
				globeSelect = 1;
				unstickAll();
				globeShift();
				break;
			case 11:
				unstickAll();
				if (globeSelect == 0)
				{
					player1c = 0;
				}
				else
				{
					player2c = 0;
				}
				charShift();
				break;
			case 12:
				unstickAll();
				if (globeSelect == 0)
				{
					player1c = 1;
				}
				else
				{
					player2c = 1;
				}
				charShift();
				break;
			case 13:
				player1ai = (player1ai == 0) ? 1 : 0;
				globeShift();
				break;
			case 14:
				player2ai = (player2ai == 0) ? 1 : 0;
				globeShift();
				break;
		}
	}

	private void unstickAll()
	{
		foreach (GameObject i in menuObjects)
		{
			try
			{
				ButtonScript b = i.GetComponent<ButtonScript>();
				if (b.sticky) {
					b.unstick();
				}
			}
			catch
			{

			}
		}
	}

	private void beginGame()
	{
		SceneManager.LoadScene("SKF");
		if (backgroundPass == 2)
		{
			backgroundPass = Random.Range(0,3);
		}
		PlayerPrefs.SetInt("stage", backgroundPass);
		PlayerPrefs.SetInt("player1c", player1c);
		PlayerPrefs.SetInt("player2c", player2c);
		PlayerPrefs.SetInt("player1ai", player1ai);
		PlayerPrefs.SetInt("player2ai", player2ai);
	}

	private void globeShift()
	{
		if (globeSelect == 0)
		{
			if (player1c != -1)
			{
				//Debug.Log("p1c: " + player1c + " | length: " + characterButtons.Length);
				characterButtons[player1c].GetComponent<ButtonScript>().stick();
			}
			globeButton1.GetComponent<ButtonScript>().stick();
			globeButton2.GetComponent<ButtonScript>().unstick();
		}
		else
		{
			if (player2c != -1)
			{
				//Debug.Log("p2c: "+player2c+" | length: "+characterButtons.Length);
				characterButtons[player2c].GetComponent<ButtonScript>().stick();
			}
			globeButton1.GetComponent<ButtonScript>().unstick();
			globeButton2.GetComponent<ButtonScript>().stick();
		}

		if (player1ai == 1)
		{
			changeSpriteColor(globe1, Color.grey);
			cpuButton1.GetComponent<ButtonScript>().setColor(new Color(0.5f,0.5f,0.5f,0.8f));
			changeFancyText(cpuText1, "cpu1");
		}
		else
		{
			changeSpriteColor(globe1, Color.red);
			cpuButton1.GetComponent<ButtonScript>().setColor(new Color(1f, 0f, 0f, 0.8f));
			changeFancyText(cpuText1, "player1");
		}
		if (player2ai == 1)
		{
			changeSpriteColor(globe2, Color.grey);
			cpuButton2.GetComponent<ButtonScript>().setColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
			changeFancyText(cpuText2, "cpu2");
		}
		else
		{
			changeSpriteColor(globe2, Color.red);
			cpuButton2.GetComponent<ButtonScript>().setColor(new Color(1f, 0f, 0f, 0.8f));
			changeFancyText(cpuText2, "player2");
		}
	}

	private void charShift()
	{
		bool showGo = true;
		switch (player1c) {
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
			characterGoButton.GetComponent<ButtonScript>().enable();
			characterGoText.SetActive(true);
		}
	}

	private void changeSpriteColor(GameObject o, Color c)
	{
		o.GetComponent<SpriteRenderer>().color = c;
	}
}

