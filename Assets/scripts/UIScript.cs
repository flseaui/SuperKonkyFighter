using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public GameObject player;

    public Slider health1;
    public Slider health2;
    public Slider meter1;
    public Slider meter2;

    public GameObject back;

    // Use this for initialization
    void Start () {
        health1.value = 10000;
        health2.value = 10000;
        //meter1.value = 0.5f;
        // meter2.value = 0.5f;

        Vector3 position = back.transform.position;
        position.x = 0;
        back.transform.position = position;

        position = back.transform.position;
        position.y = 23.5f;
        back.transform.position = position;
    }
	
	// Update is called once per frame
	void Update () {
        
        
	}
}
