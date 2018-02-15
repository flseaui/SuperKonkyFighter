using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour {

	public GameObject buttonPrefab;
	public Canvas canvas;

	public int screen;

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("background", 0);
        PlayerPrefs.SetInt("ground", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);

		GameObject playButton = Instantiate(buttonPrefab);
		//playButton.transform.SetParent(canvas.transform);
		Vector3 v = playButton.transform.position;
		v.z = 0;
		playButton.transform.position = v;
		v = playButton.transform.localScale;
		v.x = 1 / 45f;
		v.y = 1 / 45f;
		v.z = 1 / 45f;
		v= playButton.transform.localScale;
		playButton.GetComponent<Button>().onClick.AddListener(beginGame);
	}
	
	// Update is called once per frame
	void Update () {
		//if ()
		{
			
		}
		
			
		

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
		PlayerPrefs.SetInt("background", stage);
		PlayerPrefs.SetInt("ground", stage);
	}

}
