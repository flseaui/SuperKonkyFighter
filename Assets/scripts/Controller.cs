using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

    private static Global instance = null;

    public static Global Instance
    {
        get
        {
            return instance;
        }
    }

    public static bool INP1_UP;
    public static bool INP1_LEFT;
    public static bool INP1_DOWN;
    public static bool INP1_RIGHT;

    void Awake () {
        instance = this;
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            INP1_UP = true;
        }
        else
        {
            INP1_UP = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            INP1_LEFT = true;
        }
        else
        {
            INP1_LEFT = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            INP1_DOWN = true;
        }
        else
        {
            INP1_DOWN = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            INP1_RIGHT = true;
        }
        else
        {
            INP1_RIGHT = false;
        }
    }
}
