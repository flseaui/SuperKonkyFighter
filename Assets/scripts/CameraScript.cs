using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


    public GameObject player1;
    public GameObject background;

	void Start () {
        
	}

	void Update () {
        setX(background, getX()*-0.6f);
        setX(this, getX())
    }

    private float getX(GameObject o)
    {
        Vector3 position = o.transform.position;
        return position.x;
    }

    private void moveX(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x += amm;
        this.transform.position = position;
    }

    private void moveY(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.y += amm;
        this.transform.position = position;
    }

    private void setX(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.x += amm;
        this.transform.position = position;
    }

    private void setY(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.y += amm;
        this.transform.position = position;
    }
}
