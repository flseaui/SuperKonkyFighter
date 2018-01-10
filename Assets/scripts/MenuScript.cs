using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("background", 0);
        PlayerPrefs.SetInt("ground", 0);
    }
	
	// Update is called once per frame
	void OnGUI () {
        if (GUI.Button(new Rect(10, 30, 200, 60), "background1"))
        {
            SceneManager.LoadScene("SKF");

        }
        if (GUI.Button(new Rect(210, 30, 200, 60), "background2"))
        {
            SceneManager.LoadScene("SKF");
            PlayerPrefs.SetInt("background", 1);
            PlayerPrefs.SetInt("ground", 1);
        }
        if (GUI.Button(new Rect(410, 30, 200, 60), "background3"))
        {
            SceneManager.LoadScene("SKF");
            PlayerPrefs.SetInt("background", 2);
        }
        if (GUI.Button(new Rect(10, 230, 200, 60), "background4"))
        {
            SceneManager.LoadScene("SKF");
            PlayerPrefs.SetInt("background", 3);
        }
        if (GUI.Button(new Rect(10, 430, 200, 60), "background5"))
        {
            SceneManager.LoadScene("SKF");
            PlayerPrefs.SetInt("background", 4);
        }

    }
}
