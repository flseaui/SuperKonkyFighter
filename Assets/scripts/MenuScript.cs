using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	const int TITLE_SCREEN = 0;
	const int PLAYER_SELECT_SCREEN = 1;
	const int STAGE_SELECT_SCREEN = 2;

	//---------------------misc sprites-----------------//

	public Sprite titleSprite;
	public Sprite playButtonSprite;
	public RuntimeAnimatorController playButtonAnimator;
	public Sprite platformSprite;
	public Sprite konkySelect;
	public Sprite GreyshirtSelect;
	public Sprite PlayerSelectText;
	public Sprite StageSelectText;

	//---------------------background sprites------------------//

	public Sprite BackgroundNSprite;
	public Sprite backgroundRSprite;
	public Sprite background0Sprite;
	public Sprite background1Sprite;
	public Sprite background2Sprite;
	public Sprite background3Sprite;

	//----------------------screen specific key buttons----------------//

	private GameObject backgroundShowcase;
	private GameObject backgroundGoButton;
	private GameObject backgroundText;

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
	private int player1Select;
	private int player2Select;

	private int backgroundPass;

	void Start () {
        PlayerPrefs.SetInt("stage", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);

		startScreen(TITLE_SCREEN);
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
			spriteObject.GetComponent<Animator>().runtimeAnimatorController = playButtonAnimator;
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
	public GameObject makeText(float x, float y, string text)
	{
		GameObject ret = Instantiate(textPrefab);

		//set position
		Vector3 p = new Vector3(x, y, 0);
		ret.GetComponent<Transform>().position = p;

		//set text
		ret.GetComponent<TextMesh>().text = text;

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
				ret.transform.GetChild(0).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
				ret.transform.GetChild(1).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
				ret.transform.GetChild(2).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
				ret.transform.GetChild(3).GetComponent<TextMesh>().anchor = TextAnchor.UpperLeft;
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
				makeButton(new Vector3[] { new Vector2(-6, -7), new Vector2(6, -7), new Vector2(6, -2), new Vector2(-6, -2) }, new Color(0.8f, 0f, 0f, 0.75f), 1, new int[] { ButtonScript.FLAG_HIDDEN });//hidden play button
				//makeSprite(0, -5, 12, 4, playButtonSprite, playButtonAnimator);
				makeFancyText(0, -5, 2f, "play", 0);//play button (visible)
				makeSprite(0, 3, 26, 11, titleSprite);//title logo
				break;
			case PLAYER_SELECT_SCREEN:

				makeSprite(-11, -6, 8, 2, platformSprite, Color.red);
				makeSprite(11, -6, 8, 2, platformSprite, Color.red);
				makeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 2, new int[] { });

				makeButton(new Vector3[] { new Vector2(-15, 7), new Vector2(-10, 7), new Vector2( -7, 0), new Vector2(-11, 0) }, new Color(0.25f, 0.2f, 0.2f, 0.75f), -1, new int[] { });
				makeSprite(-10, 4, 8, 8, konkySelect);//konky sprite
				makeButton(new Vector3[] { new Vector2(-10, 7), new Vector2( -5, 7), new Vector2( -3, 0), new Vector2( -7, 0) }, new Color(1f, 0.5f, 0.5f, 0.75f), -1, new int[] { });
				makeSprite(-6, 4, 8, 8, GreyshirtSelect);//greyshirt sprite
				makeButton(new Vector3[] { new Vector2( -5, 7), new Vector2(  0, 7), new Vector2(  0, 0), new Vector2( -3, 0) }, new Color(0.1f, 0.9f, 0.1f, 0.75f), -1, new int[] { });
				makeButton(new Vector3[] { new Vector2(  0, 7), new Vector2(  5, 7), new Vector2(  3, 0), new Vector2(  0, 0) }, new Color(0.9f, 0.6f, 0.1f, 0.75f), -1, new int[] { });
				makeButton(new Vector3[] { new Vector2(  5, 7), new Vector2( 10, 7), new Vector2(  7, 0), new Vector2(  3, 0) }, new Color(0.00f, 0.1f, 0.9f, 0.75f), -1, new int[] { });
				makeButton(new Vector3[] { new Vector2( 10, 7), new Vector2( 15, 7), new Vector2( 11, 0), new Vector2(  7, 0) }, new Color(0.6f, 0.6f, 0.6f, 0.75f), -1, new int[] { });

				makeFancyText(-16f, 9f, 1.25f, "player select", 1);//screen description

				break;
			case STAGE_SELECT_SCREEN:

				makeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, new Color(0.25f, 0.2f, 0.2f, 0.75f), -1, new int[] { ButtonScript.FLAG_DUMMY });

				backgroundShowcase = makeSprite(0, 2, 32, 8, backgroundRSprite);
				backgroundShowcase.GetComponent<SpriteRenderer>().sortingOrder = 0 ;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundNSprite;

				backgroundGoButton = makeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, new Color(1f, 1f, 1f, 0f), 3, new int[] {ButtonScript.FLAG_DUMMY, ButtonScript.FLAG_SHADE});
				backgroundGoButton.GetComponent<MeshRenderer>().sortingOrder = 1;

				makeButton(new Vector3[] { new Vector2(-10, -2), new Vector2(-6, -2), new Vector2(-5, -6), new Vector2(-9, -6) }, new Color(0f, 0.8f, 0f, 0.75f), 4, new int[] {ButtonScript.FLAG_STICKY});
				makeButton(new Vector3[] { new Vector2(-6, -2), new Vector2(-2, -2), new Vector2(-1, -6), new Vector2(-5, -6) }, new Color(0.8f, 0.6f, 0f, 0.75f), 5, new int[] { ButtonScript.FLAG_STICKY });
				makeButton(new Vector3[] { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(1, -6), new Vector2(-1, -6) }, new Color(0.8f, 0.8f, 0.8f, 0.75f), 6, new int[] { ButtonScript.FLAG_STICKY });
				makeButton(new Vector3[] { new Vector2(2, -2), new Vector2(6, -2), new Vector2(5, -6), new Vector2(1, -6) }, new Color(0.4f, 0.0f, 0.2f, 0.75f), 7, new int[] { ButtonScript.FLAG_STICKY });
				makeButton(new Vector3[] { new Vector2(6, -2), new Vector2(10, -2), new Vector2(9, -6), new Vector2(5, -6) }, new Color(0.0f, 0f, 0.6f, 0.75f), 8, new int[] { ButtonScript.FLAG_STICKY });

				backgroundText = makeFancyText(0, -7.25f, 1.125f, "please select a stage", 0);

				makeFancyText(-16f, 9f, 1.25f, "stage select", 1);//screen description
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
				unstickAll();
				backgroundPass = 0;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background0Sprite;
				changeFancyText(backgroundText, "twlight hills");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 5:
				unstickAll();
				backgroundPass = 1;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background1Sprite;
				changeFancyText(backgroundText, "africa");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 6:
				unstickAll();
				backgroundPass = 2;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = backgroundRSprite;
				changeFancyText(backgroundText, "random");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 7:
				unstickAll();
				backgroundPass = 3;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background2Sprite;
				changeFancyText(backgroundText, "midnight park");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
				break;
			case 8:
				unstickAll();
				backgroundPass = 4;
				backgroundShowcase.GetComponent<SpriteRenderer>().sprite = background3Sprite;
				changeFancyText(backgroundText, "catacombs of carthus");
				backgroundGoButton.GetComponent<ButtonScript>().enable();
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
					b.unlock();
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
			backgroundPass = Random.Range(0,1);
		}
		PlayerPrefs.SetInt("stage", backgroundPass);
	}
}

