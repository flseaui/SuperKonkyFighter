using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
	public class OldMenuScript : MonoBehaviour {

		Color _konkyRed = new Color(0.9f, 0, 0, 1);
		const int TitleScreen = 0;
		const int PlayerSelectScreen = 1;
		const int StageSelectScreen = 2;

		//---------------------misc sprites-----------------//

		public Sprite TitleSprite;
		public Sprite PlayButtonSprite;
		public Sprite PlatformSprite;
		public Sprite KonkySelect;
		public Sprite GreyshirtSelect;
		public Sprite PlayerSelectText;
		public Sprite StageSelectText;

		public Sprite KonkyGlobe;
		public Sprite GreyshirtGlobe;
		public RuntimeAnimatorController KonkyGlobeAnim;
		public RuntimeAnimatorController GreyshirtGlobeAnim;

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
		private GameObject[] _characterButtons;

		private GameObject[] _stageSprites;

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

		private int _stageSelect;
		private int _player1C;
		private int _player2C;
		private int _player1Ai;
		private int _player2Ai;

		private int _backgroundPass;

		private int _globeSelect;

		void Start () {
			StartScreen(PlayerPrefs.GetInt("menu_state"));
		}

		void OnApplicationQuit()
		{
			PlayerPrefs.DeleteAll();
		}

		public GameObject MakeButton(Vector3[] points, Color color, int triggerId, int[] flags)
		{
			GameObject button = Instantiate(ButtonPrefab);

			button.GetComponent<ButtonScript>().MenuScript = this;

			button.GetComponent<ButtonScript>().TriggerId = triggerId;

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

			MenuObjects.Add(button);

			button.GetComponent<ButtonScript>().DefaultColor = color;
			button.GetComponent<ButtonScript>().StartFlags(flags);

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
			switch (no) {
				case TitleScreen:

					_player1Ai = 0;
					_player2Ai = 0;
					_player1C = -1;
					_player2C = -1;
					_globeSelect = 0;

					MakeButton(new Vector3[] { new Vector2(-6, -7), new Vector2(6, -7), new Vector2(6, -2), new Vector2(-6, -2) }, new Color(0.8f, 0f, 0f, 0.75f), 1, new int[] { ButtonScript.FlagHidden });//hidden play button
					MakeFancyText(0, -5, 2f, "play", 0);//play button (visible)
					MakeSprite(0, 3, 26, 11, TitleSprite);//title logo
					break;
				case PlayerSelectScreen:

					_globe1 = MakeSprite(-11, -6, 8, 2, PlatformSprite, Color.red);
					_globeButton1 = MakeButton(new Vector3[] { new Vector2(-11, -4.75f), new Vector2(-6f, -6f), new Vector2(-11, -7.25f), new Vector2(-16f, -6) }, new Color(0f, 0f, 0f, 0f), 9, new int[] { ButtonScript.FlagNoline });
					_player1 = MakeSprite(-11,-6,10,10, KonkyGlobe, KonkyGlobeAnim);
					_player1.GetComponent<SpriteRenderer>().sortingOrder = 90;
					_cpuButton1 = MakeButton(new Vector3[] { new Vector2(-10.5f, -7f), new Vector2(-6.5f, -7f), new Vector2(-6.5f, -8.5f), new Vector2(-10.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 13, new int[] {});
					_cpuText1 = MakeFancyText(-8.5f, -7.75f, 0.5f, "", 0);

					_globe2 = MakeSprite(11, -6, 8, 2, PlatformSprite, Color.red);
					_globeButton2 = MakeButton(new Vector3[] { new Vector2(11, -4.75f), new Vector2(6f, -6f), new Vector2(11, -7.25f), new Vector2(16f, -6) }, new Color(0f, 0f, 0f, 0f), 10, new int[] { ButtonScript.FlagNoline });
					_player2 = MakeSprite(11, -6, 10, 10, KonkyGlobe, GreyshirtGlobeAnim);
					_player2.GetComponent<SpriteRenderer>().sortingOrder = 90;
					_player2.GetComponent<SpriteRenderer>().flipX = true;
					_cpuButton2 = MakeButton(new Vector3[] { new Vector2(6.5f, -7f), new Vector2(10.5f, -7f), new Vector2(10.5f, -8.5f), new Vector2(6.5f, -8.5f) }, new Color(0.8f, 0f, 0f, 0.75f), 14, new int[] { });
					_cpuText2 = MakeFancyText(8.5f, -7.75f, 0.5f, "", 0);

					_characterGoButton = MakeButton(new Vector3[] { new Vector2(-6, -1), new Vector2(6, -1), new Vector2(6, -5), new Vector2(-6, -5) }, new Color(0.8f, 0f, 0f, 0.75f), 2, new int[] { ButtonScript.FlagHidden, ButtonScript.FlagDummy });
					_characterGoText = MakeFancyText(0, -3, 2, "go", 0);
					_characterGoText.SetActive(false);

					_characterButtons = new GameObject[6];

					_characterButtons[0] = MakeButton(new Vector3[] { new Vector2(-15, 7), new Vector2(-10, 7), new Vector2( -7, 0), new Vector2(-11, 0) }, new Color(0.25f, 0.2f, 0.2f, 0.75f), 11, new int[] { ButtonScript.FlagSticky });
					MakeSprite(-10, 4, 8, 8, KonkySelect);//konky sprite
					_characterButtons[1] = MakeButton(new Vector3[] { new Vector2(-10, 7), new Vector2( -5, 7), new Vector2( -3, 0), new Vector2( -7, 0) }, new Color(1f, 0.5f, 0.5f, 0.75f), 12, new int[] { ButtonScript.FlagSticky });
					MakeSprite(-6, 4, 8, 8, GreyshirtSelect);//greyshirt sprite
					MakeButton(new Vector3[] { new Vector2( -5, 7), new Vector2(  0, 7), new Vector2(  0, 0), new Vector2( -3, 0) }, new Color(0.1f, 0.9f, 0.1f, 0.75f), -1, new int[] { });
					MakeButton(new Vector3[] { new Vector2(  0, 7), new Vector2(  5, 7), new Vector2(  3, 0), new Vector2(  0, 0) }, new Color(0.9f, 0.6f, 0.1f, 0.75f), -1, new int[] { });
					MakeButton(new Vector3[] { new Vector2(  5, 7), new Vector2( 10, 7), new Vector2(  7, 0), new Vector2(  3, 0) }, new Color(0.00f, 0.1f, 0.9f, 0.75f), -1, new int[] { });
					MakeButton(new Vector3[] { new Vector2( 10, 7), new Vector2( 15, 7), new Vector2( 11, 0), new Vector2(  7, 0) }, new Color(0.6f, 0.6f, 0.6f, 0.75f), -1, new int[] { });

					MakeText(-16f, 9f, 1.25f, "player select", 1);//screen description

					MakeButton(new Vector3[] { new Vector2(-16, -7), new Vector2(-11, -7), new Vector2(-11, -9), new Vector2(-16, -9) }, new Color(), 0, new int[] { ButtonScript.FlagHidden });
					MakeFancyText(-16f, -9f, 1f, "back", 2);//back button

					GlobeShift();
					CharShift();

					break;
				case StageSelectScreen:

					_backgroundFrame = MakeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, _konkyRed, -1, new int[] { ButtonScript.FlagDummy });
					_backgroundFrame.GetComponent<MeshRenderer>().sortingOrder = 1;

					_backgroundShowcase = MakeSprite(0, 2, 32, 8, BackgroundRSprite);
					_backgroundShowcase.GetComponent<SpriteRenderer>().sortingOrder = 0 ;
					_backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundNSprite;

					_backgroundGoButton = MakeButton(new Vector3[] { new Vector2(-16, 6), new Vector2(16, 6), new Vector2(16, -2), new Vector2(-16, -2) }, new Color(1f, 1f, 1f, 0f), 3, new int[] {ButtonScript.FlagDummy, ButtonScript.FlagShade});
					_backgroundGoButton.GetComponent<MeshRenderer>().sortingOrder = 1;

					_stageSprites = new GameObject[5];

					MakeButton(new Vector3[] { new Vector2(-10, -2), new Vector2(-6, -2), new Vector2(-5, -6), new Vector2(-9, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 4, new int[] {ButtonScript.FlagSticky, ButtonScript.FlagLockClear}).GetComponent<MeshRenderer>().sortingOrder = 1;
					_stageSprites[0] = MakeSprite(-7.5f, -4, 5, 4, Background0Button);
					_stageSprites[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
					ChangeSpriteColor(_stageSprites[0], _konkyRed);
					MakeButton(new Vector3[] { new Vector2(-6, -2), new Vector2(-2, -2), new Vector2(-1, -6), new Vector2(-5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 5, new int[] { ButtonScript.FlagSticky, ButtonScript.FlagLockClear }).GetComponent<MeshRenderer>().sortingOrder = 1;
					_stageSprites[1] = MakeSprite(-3.5f, -4, 5, 4, Background1Button);
					_stageSprites[1].GetComponent<SpriteRenderer>().sortingOrder = 1;
					ChangeSpriteColor(_stageSprites[1], _konkyRed);
					MakeButton(new Vector3[] { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(1, -6), new Vector2(-1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 6, new int[] { ButtonScript.FlagSticky, ButtonScript.FlagLockClear }).GetComponent<MeshRenderer>().sortingOrder = 1;
					_stageSprites[2] = MakeSprite(0, -4, 5, 4, BackgroundRButton);
					_stageSprites[2].GetComponent<SpriteRenderer>().sortingOrder = 1;
					ChangeSpriteColor(_stageSprites[2], _konkyRed);
					MakeButton(new Vector3[] { new Vector2(2, -2), new Vector2(6, -2), new Vector2(5, -6), new Vector2(1, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 7, new int[] { ButtonScript.FlagSticky, ButtonScript.FlagLockClear }).GetComponent<MeshRenderer>().sortingOrder = 1;
					_stageSprites[3] = MakeSprite(3.5f, -4, 5, 4, Background2Button);
					_stageSprites[3].GetComponent<SpriteRenderer>().sortingOrder = 1;
					ChangeSpriteColor(_stageSprites[3], _konkyRed);
					MakeButton(new Vector3[] { new Vector2(6, -2), new Vector2(10, -2), new Vector2(9, -6), new Vector2(5, -6) }, new Color(0.8f, 0.0f, 0f, 0f), 8, new int[] { ButtonScript.FlagSticky, ButtonScript.FlagLockClear }).GetComponent<MeshRenderer>().sortingOrder = 1;
					_stageSprites[4] = MakeSprite(7.5f, -4, 5, 4, Background3Button);
					_stageSprites[4].GetComponent<SpriteRenderer>().sortingOrder = 1;
					ChangeSpriteColor(_stageSprites[4], _konkyRed);

					_backgroundText = MakeText(0, -7.25f, 1.125f, "select a stage", 0);//background text

					MakeButton(new Vector3[] { new Vector2(-16,-7), new Vector2(-11,-7), new Vector2(-11,-9), new Vector2(-16,-9) }, new Color(), 1, new int[] { ButtonScript.FlagHidden });
					MakeFancyText(-16f, -9f, 1f, "back", 2);//back button

					MakeText(-16f, 9f, 1.25f, "stage select", 1);//screen description
					break;

			}
		}

		public void TriggerEvent(int id)
		{
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
					UnstickAll();
					ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
					ChangeSpriteColor(_stageSprites[0], Color.white);
					_backgroundPass = 0;
					_backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
					_backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background0Sprite;
					ChangeText(_backgroundText, "twlight hills");
					_backgroundGoButton.GetComponent<ButtonScript>().Enable();
					break;
				case 5:
					UnstickAll();
					ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
					ChangeSpriteColor(_stageSprites[1], Color.white);
					_backgroundPass = 1;
					_backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
					_backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background1Sprite;
					ChangeText(_backgroundText, "africa");
					_backgroundGoButton.GetComponent<ButtonScript>().Enable();
					break;
				case 6:
					UnstickAll();
					ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
					ChangeSpriteColor(_stageSprites[2], Color.white);
					_backgroundPass = 2;
					_backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
					_backgroundShowcase.GetComponent<SpriteRenderer>().sprite = BackgroundRSprite;
					ChangeText(_backgroundText, "random");
					_backgroundGoButton.GetComponent<ButtonScript>().Enable();
					break;
				case 7:
					UnstickAll();
					ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
					ChangeSpriteColor(_stageSprites[3], Color.white);
					_backgroundPass = 3;
					_backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
					_backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background2Sprite;
					ChangeText(_backgroundText, "midnight park");
					_backgroundGoButton.GetComponent<ButtonScript>().Enable();
					break;
				case 8:
					UnstickAll();
					ChangeSpriteColor(_stageSprites[_backgroundPass], _konkyRed);
					ChangeSpriteColor(_stageSprites[4], Color.white);
					_backgroundPass = 4;
					_backgroundFrame.GetComponent<MeshRenderer>().material.color = Color.clear;
					_backgroundShowcase.GetComponent<SpriteRenderer>().sprite = Background3Sprite;
					ChangeText(_backgroundText, "training grounds");
					_backgroundGoButton.GetComponent<ButtonScript>().Enable();
					break;
				case 9:
					_globeSelect = 0;
					UnstickAll();
					GlobeShift();
					break;
				case 10:
					_globeSelect = 1;
					UnstickAll();
					GlobeShift();
					break;
				case 11:
					UnstickAll();
					if (_globeSelect == 0)
					{
						_player1C = 0;
					}
					else
					{
						_player2C = 0;
					}
					CharShift();
					break;
				case 12:
					UnstickAll();
					if (_globeSelect == 0)
					{
						_player1C = 1;
					}
					else
					{
						_player2C = 1;
					}
					CharShift();
					break;
				case 13:
					_player1Ai = (_player1Ai == 0) ? 1 : 0;
					GlobeShift();
					break;
				case 14:
					_player2Ai = (_player2Ai == 0) ? 1 : 0;
					GlobeShift();
					break;
			}
		}

		private void UnstickAll()
		{
			foreach (GameObject i in MenuObjects)
			{
				try
				{
					ButtonScript b = i.GetComponent<ButtonScript>();
					if (b.Sticky) {
						b.Unstick();
					}
				}
				catch
				{

				}
			}
		}

		private void BeginGame()
		{
			SceneManager.LoadScene("SKF");
			if (_backgroundPass == 2)
			{
				_backgroundPass = Random.Range(0,3);
			}
			PlayerPrefs.SetInt("stage", _backgroundPass);
			PlayerPrefs.SetInt("player1c", _player1C);
			PlayerPrefs.SetInt("player2c", _player2C);
			PlayerPrefs.SetInt("player1ai", _player1Ai);
			PlayerPrefs.SetInt("player2ai", _player2Ai);
		}

		private void GlobeShift()
		{
			if (_globeSelect == 0)
			{
				if (_player1C != -1)
				{
					//Debug.Log("p1c: " + player1c + " | length: " + characterButtons.Length);
					_characterButtons[_player1C].GetComponent<ButtonScript>().Stick();
				}
				_globeButton1.GetComponent<ButtonScript>().Stick();
				_globeButton2.GetComponent<ButtonScript>().Unstick();
			}
			else
			{
				if (_player2C != -1)
				{
					//Debug.Log("p2c: "+player2c+" | length: "+characterButtons.Length);
					_characterButtons[_player2C].GetComponent<ButtonScript>().Stick();
				}
				_globeButton1.GetComponent<ButtonScript>().Unstick();
				_globeButton2.GetComponent<ButtonScript>().Stick();
			}

			if (_player1Ai == 1)
			{
				ChangeSpriteColor(_globe1, Color.grey);
				_cpuButton1.GetComponent<ButtonScript>().SetColor(new Color(0.5f,0.5f,0.5f,0.8f));
				ChangeFancyText(_cpuText1, "cpu1");
			}
			else
			{
				ChangeSpriteColor(_globe1, Color.red);
				_cpuButton1.GetComponent<ButtonScript>().SetColor(new Color(1f, 0f, 0f, 0.8f));
				ChangeFancyText(_cpuText1, "player1");
			}
			if (_player2Ai == 1)
			{
				ChangeSpriteColor(_globe2, Color.grey);
				_cpuButton2.GetComponent<ButtonScript>().SetColor(new Color(0.5f, 0.5f, 0.5f, 0.8f));
				ChangeFancyText(_cpuText2, "cpu2");
			}
			else
			{
				ChangeSpriteColor(_globe2, Color.red);
				_cpuButton2.GetComponent<ButtonScript>().SetColor(new Color(1f, 0f, 0f, 0.8f));
				ChangeFancyText(_cpuText2, "player2");
			}
		}

		private void CharShift()
		{
			bool showGo = true;
			switch (_player1C) {
				case 0:
					_player1.GetComponent<Animator>().runtimeAnimatorController = KonkyGlobeAnim;
					ChangeSpriteColor(_player1, Color.white);
					break;
				case 1:
					_player1.GetComponent<Animator>().runtimeAnimatorController = GreyshirtGlobeAnim;
					ChangeSpriteColor(_player1, Color.white);
					break;
				default:
					ChangeSpriteColor(_player1, Color.clear);
					showGo = false;
					break;
			}
			switch (_player2C)
			{
				case 0:
					_player2.GetComponent<Animator>().runtimeAnimatorController = KonkyGlobeAnim;
					ChangeSpriteColor(_player2, Color.white);
					break;
				case 1:
					_player2.GetComponent<Animator>().runtimeAnimatorController = GreyshirtGlobeAnim;
					ChangeSpriteColor(_player2, Color.white);
					break;
				default:
					ChangeSpriteColor(_player2, Color.clear);
					showGo = false;
					break;
			}
			if (showGo)
			{
				_characterGoButton.GetComponent<ButtonScript>().Enable();
				_characterGoText.SetActive(true);
			}
		}

		private void ChangeSpriteColor(GameObject o, Color c)
		{
			o.GetComponent<SpriteRenderer>().color = c;
		}
	}
}

