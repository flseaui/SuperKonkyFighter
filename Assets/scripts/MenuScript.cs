using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour {

	public Sprite titleSprite;
	public Sprite playButtonSprite;
	public RuntimeAnimatorController playButtonAnimator;

	//-------------------------------------------//

	public GameObject buttonPrefab;
	public GameObject spritePrefab;

	public GameObject background;

	public List<GameObject> buttons;
	public List<int> links;

	private int screen;

	void Start () {
        PlayerPrefs.SetInt("stage", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);

		startScreen(0);
	}
	
	public ButtonScript makeButton(Vector3[] points, string text, Color color)
	{
		GameObject button = Instantiate(buttonPrefab);

		button.GetComponent<ButtonScript>().menuScript = this;

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

		button.GetComponentInChildren<TextMesh>().text = text;

		Material mat = button.GetComponent<MeshRenderer>().material;
		mat.color = color;

		buttons.Add(button);

		return button.GetComponent<ButtonScript>();
	}

	public void makeSprite(int x, int y, int width, int height, Sprite sprite, RuntimeAnimatorController animator)
	{
		GameObject spriteObject = Instantiate(spritePrefab);

		RectTransform rect = spriteObject.GetComponent<RectTransform>();
		Vector3 position = rect.position;
		position.x = x;
		position.y = y;
		rect.position = position;

		Vector2 size = rect.sizeDelta;
		size.x = width;
		size.y = height;
		rect.sizeDelta = size;

		SpriteRenderer render = spriteObject.GetComponent<SpriteRenderer>();
		render.sprite = sprite;
		render.size = size;

		if (animator != null)
		{
			spriteObject.GetComponent<Animator>().runtimeAnimatorController = playButtonAnimator;
		}
		buttons.Add(spriteObject);
	}

	public void clearButtons()
	{
		foreach (GameObject i in buttons)
		{
			Destroy(i);
		}
		buttons.Clear();
	}

	void Update () {
    }

	public void startScreen(int no)
	{
		clearButtons();
		switch (no) {
			case 0:
				Color c = background.GetComponent<SpriteRenderer>().color = new Color(0.6705882352941176f, 0f, 0f);
				Color buttonColor = new Color(0.8f, 0f, 0f, 0.75f);
				makeButton(new Vector3[] {new Vector2(-6, -7) , new Vector2(6, -7) , new Vector2(6, -2) , new Vector2(-6, -2) }, "", buttonColor).hide();
				makeSprite(0, -5, 12, 4, playButtonSprite, playButtonAnimator);
				makeSprite(0, 3, 26, 11, titleSprite, null);
				break;
			case 1:
				buttonColor = new Color(0.8f, 0f, 0f, 0.75f);
				makeButton(new Vector3[] { new Vector2(-15, 0), new Vector2(-7, 0), new Vector2(-7, -8), new Vector2(-15, -8) }, "", buttonColor);
				makeButton(new Vector3[] { new Vector2(7, 0), new Vector2(15, 0), new Vector2(15, -8), new Vector2(7, -8) }, "", buttonColor);
				makeButton(new Vector3[] { new Vector2(-6, 0), new Vector2(6, 0), new Vector2(6, -4), new Vector2(-6, -4) }, "", buttonColor);

				makeButton(new Vector3[] { new Vector2(-15, 8), new Vector2(-10, 8), new Vector2(-7, 1), new Vector2(-11, 1) }, "", new Color(0.25f, 0.2f, 0.2f, 0.75f));
				makeButton(new Vector3[] { new Vector2(-10, 8), new Vector2(-5, 8), new Vector2(-3, 1), new Vector2(-7, 1) }, "", new Color(1f, 0.5f, 0.5f, 0.75f));
				makeButton(new Vector3[] { new Vector2(-5, 8), new Vector2(0, 8), new Vector2(0, 1), new Vector2(-3, 1) }, "", new Color(0.1f, 0.9f, 0.1f, 0.75f));
				makeButton(new Vector3[] { new Vector2(0, 8), new Vector2(5, 8), new Vector2(3, 1), new Vector2(0, 1) }, "", new Color(0.9f, 0.6f, 0.1f, 0.75f));
				makeButton(new Vector3[] { new Vector2(5, 8), new Vector2(10, 8), new Vector2(7, 1), new Vector2(3, 1) }, "", new Color(0.00f, 0.1f, 0.9f, 0.75f));
				makeButton(new Vector3[] { new Vector2(10, 8), new Vector2(15, 8), new Vector2(11, 1), new Vector2(7, 1) }, "", new Color(0.6f, 0.6f, 0.6f, 0.75f));
				break;
		}
	}

	public void beginGame()
	{
		SceneManager.LoadScene("SKF");
		int stage = Random.Range(1, 2);
		PlayerPrefs.SetInt("stage", stage);
	}
}

