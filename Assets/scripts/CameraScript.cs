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

        player2 = Instantiate(playerPrefab);
        setX(player2, 16f);
        player2.GetComponent<PlayerScript>().flip(false);
    }

    void Update()
    {
       // setX(background, getX(self) * -0.6f);
        //setX(self, getX(player1));
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
        position.x += amm;
        o.transform.position = position;
    }
}
