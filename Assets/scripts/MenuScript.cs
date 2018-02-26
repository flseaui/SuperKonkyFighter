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

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("stage", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);

		makeButton(10, 2, 0, 0, "feck");
	}
	
	//20 by 34 yo
	public void makeButton(int width, int height, int x, int y, string text)
	{
		GameObject button = Instantiate(buttonPrefab);
		Vector3 v = button.transform.position;
		v.x = x;
		v.y = y;
		button.transform.position = v;
		v = button.transform.localScale;
		v.x = width;
		v.y = height;
		button.transform.localScale = v;
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
		switch (no) {
			case 0:
				//GameObject playbutton = Instantiate(buttonPrefab);
				break;
			default:
				//GameObject playbutton = Instantiate(buttonPrefab);
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

