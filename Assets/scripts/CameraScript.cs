using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{


    public GameObject playerPrefab;
    public GameObject background;
    public Camera self;

    public GameObject player1;
    public GameObject player2;

    void Start()
    {
        player1 = Instantiate(playerPrefab);
        setX(player1, -16f);
        player1.GetComponent<PlayerScript>().flip(true);
        player1.GetComponent<PlayerScript>().playerID = 1;

        player2 = Instantiate(playerPrefab);
        setX(player2, 16f);
        player2.GetComponent<PlayerScript>().flip(false);
        player2.GetComponent<PlayerScript>().playerID = 2;
    }

    void Update()
    {
        float cx = (getX(player1) + getX(player2)) / 2f;
        if (cx > 42)
        {
            cx = 42;
        }else if (cx < -42)
        {
            cx = -42;
        }
        setX(background, cx * 0.5f);
        setX(self, cx);

        if (getX(player1) < getX(player2))
        {
            player1.GetComponent<PlayerScript>().flip(true);
            player2.GetComponent<PlayerScript>().flip(false);
        }
        else
        {
            player1.GetComponent<PlayerScript>().flip(false);
            player2.GetComponent<PlayerScript>().flip(true);
        }
    }

    private float getX(GameObject o)
    {
        return o.transform.position.x;
    }
    private float getX(Camera o)
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
        position.x = amm;
        o.transform.position = position;
    }
}
