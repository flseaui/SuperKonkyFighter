using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
	public Canvas canvas;
	public UIScript uis;

	public GameObject playerPrefab;
    public GameObject ghost;
	public GameObject background;
	public GameObject ground;
	public Camera self;

	public GameObject player1;
    public PlayerScript p1s;
    public CollisionScript p1h;
	public GameObject player2;
	public PlayerScript p2s;
	public CollisionScript p2h;

	public Sprite background0;
	public Sprite background1;
	public Sprite background2;
	public Sprite background3;
	public Sprite background4;
	public Sprite background5;
	public Sprite background6;

	public Sprite ground0;
	public Sprite ground1;

	public Sprite[] Background;
	public Sprite[] Ground;

	public JoyScript JoyScript;

	public Button lightButton;
	public Button mediumButton;
	public Button heavyButton;

	private int megaKek;

	public float shakeX;
	public float shakeY;
    public bool lastP1Side;
    public bool lastP2Side;


	void Start()
	{
		uis = canvas.GetComponent<UIScript>();

		player1 = Instantiate(playerPrefab);

		setX(player1, -16f);
		p1s = player1.GetComponent<PlayerScript>();
        p1h = player1.GetComponentInChildren<CollisionScript>();
		p1s.facingRight = true;
		p1s.playerID = 1;
		p1s.JoyScript = JoyScript;
        //p1s.lightButton = lightButton;
        //p1s.mediumButton = mediumButton;
        //p1s.heavyButton = heavyButton;

        player2 = Instantiate(playerPrefab);
		setX(player2, 16f);
		p2s = player2.GetComponent<PlayerScript>();
        p2h = player2.GetComponentInChildren<CollisionScript>();
        p2s.facingRight = false;
		p2s.playerID = 2;

        p1s.otherPlayer = player2;
		p2s.otherPlayer = player1;

        ghost.GetComponent<BackGroundScript>().setScripts(p1s, p2s);

        Background = new Sprite[] { background0, background1, background2, background3, background4, background5, background6 };
		Ground = new Sprite[] { ground0, ground1 };

		background.GetComponent<SpriteRenderer>().sprite = Background[PlayerPrefs.GetInt("background", 0)];
		ground.GetComponent<SpriteRenderer>().sprite = Ground[PlayerPrefs.GetInt("ground", 0)];
	}

    void Update()
    {

        if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        
		float cx = (getX(player1) + getX(player2)) / 2f;
        if (cx > 42)
        {
            cx = 42;
        } else if (cx < -42)
        {
            cx = -42;
        }
        float cy = ((getY(player1) + 13 / 2 - 8) + (getY(player2) + 13 / 2 - 8)) / 2f + 8;
        if (cy < 12)
        {
            cy = 12;
        }
        //setY(background, cy * 0.5f + 8);
        //setY(self, cy);
        setX(background, cx * 0.5f);
        setX(self, cx);

        if (getX(player1) < getX(player2) - 1)
        {
            p1s.playerSide = true;
            p2s.playerSide = false;
        }
        else if (getX(player1) > getX(player2) + 1)
        {
            p1s.playerSide = false;
            p2s.playerSide = true;
        }

        uis.health1.maxValue = p1s.maxHealth;
        uis.health1.minValue = 0;
        uis.health2.maxValue = p2s.maxHealth;
        uis.health2.minValue = 0;
        uis.health1.value = p1s.health;
        uis.meter1.value = p1s.meterCharge;
		uis.health2.value = p2s.health;
        uis.meter2.value = p2s.meterCharge;

        if (ghost.GetComponent<BackGroundScript>().shake)
		{
            shakeX = Random.Range(-0.75f, 0.75f);
			shakeY = Random.Range(-0.75f, 0.75f);
			setX(self, cx + shakeX);
			setY(self, 12 + shakeY);
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

    private float getY(GameObject o)
    {
        return o.transform.position.y;
    }
    private float getY(Camera o)
    {
        return o.transform.position.y;
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

    private void setY(GameObject o, float amm)
    {
        Vector3 position = o.transform.position;
        position.y = amm;
        o.transform.position = position;
    }

    private void setY(Camera o, float amm)
	{
		Vector3 position = o.transform.position;
		position.y = amm;
		o.transform.position = position;
	}
}
