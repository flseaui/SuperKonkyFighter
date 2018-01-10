using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{


    public GameObject playerPrefab;
    public GameObject background;
	public GameObject ground;
    public Camera self;

    public GameObject player1;
    public GameObject player2;

    public Slider p1Heath;
    public Slider p2Heath;
    public Slider p1Power;
    public Slider p2Power;

	public Sprite background0;
	public Sprite background1;
	public Sprite background2;
	public Sprite background3;
	public Sprite background4;
	public Sprite background5;
	public Sprite background6;

	public Sprite ground0;
	public Sprite ground1;

	public bool history;

    public Sprite[] Background;
    public Sprite[] Ground;

    void Start()
    {
        player1 = Instantiate(playerPrefab);
        setX(player1, -16f);
		player1.GetComponent<PlayerScript>().facingRight = true;
		player1.GetComponent<PlayerScript>().playerID = 1;

        player2 = Instantiate(playerPrefab);
        setX(player2, 16f);
		player2.GetComponent<PlayerScript>().facingRight = false;
        player2.GetComponent<PlayerScript>().playerID = 2;

        player1.GetComponent<PlayerScript>().otherPlayer = player2;
        player2.GetComponent<PlayerScript>().otherPlayer = player1;

        history = true;

        Background = new Sprite[]{background0,background1,background2,background3,background4,background5,background6};
        Ground = new Sprite[] { ground0, ground1 };

		background.GetComponent<SpriteRenderer>().sprite = Background[PlayerPrefs.GetInt("background", 0)];
		ground.GetComponent<SpriteRenderer>().sprite = Ground[PlayerPrefs.GetInt("ground",0)];
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

		bool now = getX(player1) < getX(player2);

		if (now != history) {
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

        history = getX(player1) < getX(player2);

        if (player1.GetComponent<PlayerScript>().hitbox.GetComponent<hitboxScript>().hit)
        {
            p1Heath.GetComponent<Slider>().value -= 50;
        }

        if (player2.GetComponent<PlayerScript>().hitbox.GetComponent<hitboxScript>().hit)
        {
            p2Heath.GetComponent<Slider>().value -= 50;
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
