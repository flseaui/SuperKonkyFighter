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

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("stage", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);

		startScreen(0);
	}
	
	//20 by 34 yo
	public void makeButton(Vector3[] points, string text, Color color)
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
				makeButton(new Vector3[] {new Vector2(-6, -3) , new Vector2(6, -3) , new Vector2(6, 3) , new Vector2(-6, 3) }, "play", buttonColor);
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

