using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour {

	public GameObject buttonPrefab;
	public Canvas canvas;

	public int screen;

	public List<GameObject> buttons;
	public List<int> links;

	public GameObject background;

	Color buttonColor;

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("stage", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);

		startScreen(0);
	}
	
	//20 by 34 yo
	public void makeButton(Vector3[] points, string text)
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

		//button.GetComponent<SpriteRenderer>().color = buttonColor;

		buttons.Add(button);
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
				Color c = background.GetComponent<SpriteRenderer>().color = new Color(0.101960784f, 0f, 0f);
				buttonColor = new Color(1f, 0.3f, 0.3f);
				makeButton(new Vector3[] {new Vector2(-6, -3) , new Vector2(6, -3) , new Vector2(6, 3) , new Vector2(-6, 3) }, "play");
				break;
			case 1:
				makeButton(new Vector3[] { new Vector2(-15, 0), new Vector2(-7, 0), new Vector2(-7, -8), new Vector2(-15, -8) }, "");
				makeButton(new Vector3[] { new Vector2(7, 0), new Vector2(15, 0), new Vector2(15, -8), new Vector2(7, -8) }, "");
				makeButton(new Vector3[] { new Vector2(-6, 0), new Vector2(6, 0), new Vector2(6, -4), new Vector2(-6, -4) }, "");
				makeButton(new Vector3[] { new Vector2(-15, 8), new Vector2(-10, 8), new Vector2(-7, 1), new Vector2(-11, 1) }, "");
				makeButton(new Vector3[] { new Vector2(-10, 8), new Vector2(-5, 8), new Vector2(-3, 1), new Vector2(-7, 1) }, "");
				makeButton(new Vector3[] { new Vector2(-5, 8), new Vector2(0, 8), new Vector2(0, 1), new Vector2(-3, 1) }, "");
				break;
		}
	}

	public void beginGame()
	{
		SceneManager.LoadScene("SKF");
		int stage = Random.Range(1, 2);
		PlayerPrefs.SetInt("stage", stage);
	}

	private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
		Color[] rpixels = result.GetPixels(0);
		float incX = (1.0f / (float)targetWidth);
		float incY = (1.0f / (float)targetHeight);
		for (int px = 0; px < rpixels.Length; px++)
		{
			rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
		}
		result.SetPixels(rpixels, 0);
		result.Apply();

		source = result;

		return result;
	}
}

