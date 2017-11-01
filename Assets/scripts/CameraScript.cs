/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


    public GameObject player1;
    public GameObject background;
    public Camera cameraa;

	void Start () {
	}

	void Update () {
        setX(background, getX(cameraa)*-0.6f);
        setX(cameraa, getX(player1));
    }

    private float getX(Behavior o)
    {
        return o.transform.position.x;
    }

    private float getX(GameObject o)
    {
        return o.transform.position.x;
    }

    private void moveX(Camera o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x += amm;
    }

    private void setX(Camera o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x = amm;
        o.transform.position = position;
    }

    private void setX(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x += amm;
        o.transform.position = position;
    }
}*/
