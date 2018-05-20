using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public Slider health1;
    public Slider health2;
    public Slider meter1;
    public Slider meter2;
    public Slider meter1p;
    public Slider meter2p;

    public GameObject back;

    // Use this for initialization
    void Start ()
    {
        Vector3 position = back.transform.position;
        position.x = 0;
        back.transform.position = position;

        position = back.transform.position;
        position.y = 23.5f;
        back.transform.position = position;
    }
}
