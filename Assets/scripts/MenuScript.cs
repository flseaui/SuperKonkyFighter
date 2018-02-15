using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("background", 0);
        PlayerPrefs.SetInt("ground", 0);
		PlayerPrefs.SetInt("character1", 0);
		PlayerPrefs.SetInt("character2", 0);
	}
	
	// Update is called once per frame
	void OnGUI () {

		if (GUI.Button(new Rect(200, 240, 200, 60), "play"))
		{
			SceneManager.LoadScene("SKF");
			int stage = Random.Range(1,2);
			PlayerPrefs.SetInt("background", stage);
			PlayerPrefs.SetInt("ground", stage);
		}

    }
}
