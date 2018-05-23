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
    public Sprite ground2;
    public Sprite ground3;

    public Sprite[] Background;
	public Sprite[] Ground;

    public Sprite konkyPortrait, greyshirtPortrait;

	public JoyScript JoyScript;

	public Button lightButton;
	public Button mediumButton;
	public Button heavyButton;

    public Transform cameraLeft, cameraRight;
    public Transform cameraFarLeft, cameraFarRight;
    public Transform cameraLeftPos, cameraRightPos;
    public Transform leftEdge, rightEdge, topEdge, bottomEdge;

    public IntVariable time, p1Wins, p2Wins, roundCounter;

    float vertExtent, horzExtent;

	private int megaKek;

	public float shakeX;
	public float shakeY;
    public bool lastP1Side;
    public bool lastP2Side;

    public float magnitude, roughness, fadeIn, fadeOut;

    private bool justShook, justWon = false;
    private Vector3 preShakePos;

    public Image playerPortrait1, playerPortrait2;

    void OnApplicationQuit()
    {
        roundCounter.value = 0;
        p1Wins.value = 0;
        p2Wins.value = 0;
        PlayerPrefs.DeleteKey("player1w");
        PlayerPrefs.DeleteKey("player2w");
        PlayerPrefs.GetInt("loadedFromEditor");
    }

    void Start()
	{
        cameraLeftPos.position = cameraLeft.position;
        cameraRightPos.position = cameraRight.position;
        uis = canvas.GetComponent<UIScript>();

		player1 = Instantiate(playerPrefab);

		setX(player1, -16f);
		p1s = player1.GetComponent<PlayerScript>();
        p1h = player1.GetComponentInChildren<CollisionScript>();
		p1s.facingRight = true;
		p1s.playerID = 1;
		p1s.JoyScript = JoyScript;
        p1s.cameraLeft = cameraLeftPos;
        p1s.cameraRight = cameraRightPos;
        //p1s.lightButton = lightButton;
        //p1s.mediumButton = mediumButton;
        //p1s.heavyButton = heavyButton;

        player2 = Instantiate(playerPrefab);
		setX(player2, 16f);
		p2s = player2.GetComponent<PlayerScript>();
        p2h = player2.GetComponentInChildren<CollisionScript>();
        p2s.facingRight = false;
		p2s.playerID = 2;
        p2s.cameraLeft = cameraLeftPos;
        p2s.cameraRight = cameraRightPos;

        p1s.otherPlayer = player2;
		p2s.otherPlayer = player1;

        ghost.GetComponent<BackGroundScript>().setScripts(p1s, p2s);

        Background = new Sprite[] { background0, background1, background2, background3 };
		Ground = new Sprite[] { ground0, ground1, ground2, ground3 };

        GetComponentInParent<Follow>().setTargets(player1.transform, player2.transform);

        vertExtent = GetComponentInParent<Follow>().vertExtent;
        horzExtent = GetComponentInParent<Follow>().horzExtent;

        int stage = PlayerPrefs.GetInt("stage");

        background.GetComponent<SpriteRenderer>().sprite = Background[stage < 2 ? stage : stage - 1];
        ground.GetComponent<SpriteRenderer>().sprite = Ground[stage < 2 ? stage : stage - 1];

        switch (PlayerPrefs.GetInt("player1c"))
        {
            case 0:
                playerPortrait1.sprite = konkyPortrait;
                if (PlayerPrefs.GetInt("player1c") == PlayerPrefs.GetInt("player2c"))
                {
                    p2s.GetComponent<SpriteRenderer>().color = Color.red;
                    playerPortrait2.color = Color.red;
                }
                break;
            case 1:
                playerPortrait1.sprite = greyshirtPortrait;
                if (PlayerPrefs.GetInt("player1c") == PlayerPrefs.GetInt("player2c"))
                {
                    p2s.GetComponent<SpriteRenderer>().color = Color.cyan;
                    playerPortrait2.color = Color.cyan;
                }
                break;
        }
        switch (PlayerPrefs.GetInt("player2c"))
        {
            case 0:
                playerPortrait2.sprite = konkyPortrait;
                break;
            case 1:
                playerPortrait2.sprite = greyshirtPortrait;
                break;
        }

        Debug.LogFormat("STAGE {0}", stage);
    }

    void Update()
    {
        /*
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        } 
        */

        if (getX(player1) < getX(player2) - 1)
        {
            p1s.playerSide = true;
            p2s.playerSide = false;

            if (getX(player1) < cameraLeft.position.x)
                cameraLeftPos.position = cameraLeft.position;
            else
                cameraLeftPos.position = cameraFarLeft.position;

            if (getX(player2) > cameraRight.position.x)
                cameraRightPos.position = cameraRight.position;
            else
                cameraRightPos.position = cameraFarRight.position;
        }
        else if (getX(player1) > getX(player2) + 1)
        {
            p1s.playerSide = false;
            p2s.playerSide = true;

            if (getX(player2) < cameraLeft.position.x)
                cameraLeftPos.position = cameraLeft.position;
            else
                cameraLeftPos.position = cameraFarLeft.position;

            if (getX(player1) > cameraRight.position.x)
                cameraRightPos.position = cameraRight.position;
            else
                cameraRightPos.position = cameraFarRight.position;
        }

        p1s.cameraLeft.position = cameraLeftPos.position;
        p1s.cameraRight.position = cameraRightPos.position;

        p2s.cameraLeft.position = cameraLeftPos.position;
        p2s.cameraRight.position = cameraRightPos.position;

        uis.health1.maxValue = p1s.maxHealth;
        uis.health1.minValue = 0;
        uis.health1p.maxValue = p1s.maxHealth;
        uis.health1p.minValue = 0;
        uis.health2.maxValue = p2s.maxHealth;
        uis.health2.minValue = 0;
        uis.health2p.maxValue = p2s.maxHealth;
        uis.health2p.minValue = 0;
        uis.meter1.maxValue = p1s.maxMeter;
        uis.meter1.minValue = 0;
        uis.meter1p.maxValue = p1s.maxMeter;
        uis.meter1p.minValue = 0;
        uis.meter2.maxValue = p2s.maxMeter;
        uis.meter2.minValue = 0;
        uis.meter2p.maxValue = p2s.maxMeter;
        uis.meter2p.minValue = 0;
        uis.health1.value = p1s.health;
        uis.health1p.value = p1s.health + p1s.healthStore;
        uis.meter1.value = p1s.meterCharge;
        uis.meter1p.value = p1s.meterCharge + p1s.meterStore;
		uis.health2.value = p2s.health;
        uis.health2p.value = p2s.health + p2s.healthStore;
        uis.meter2.value = p2s.meterCharge;
        uis.meter2p.value = p2s.meterCharge + p2s.meterStore;

        if (p1s.health <= 0)
        {
            //RoundManager.instance.nextRound();
            //PlayerPrefs.SetInt("menu_state", 1);
            if (!justWon)
            {
                justWon = true;
                p2Wins.value++;
                Invoke("nextRound", 2);
            }
        }
        else if (p2s.health <= 0)
        {
            if (!justWon)
            {
                justWon = true;
                p1Wins.value++;
                Invoke("nextRound", 2);
            }
        }
        else if (time.value == 0)
        {
            if (!justWon)
            {
                justWon = true;
                if (p1s.health > p2s.health)
                    p1Wins.value++;
                else
                    p2Wins.value++;
                Invoke("nextRound", 2);
            }
        }

        if (ghost.GetComponent<BackGroundScript>().shake)
		{
            horzExtent = 26.66667f;

            if (justShook == false)
            {
                preShakePos = transform.position;

                p1s.camLeft = preShakePos.x - horzExtent;
                p2s.camLeft = preShakePos.x - horzExtent;

                p1s.camRight = preShakePos.x + horzExtent;
                p2s.camRight = preShakePos.x + horzExtent;
                
                justShook = true;
            }

            shakeX = Random.Range(-0.75f, 0.75f);
			shakeY = Random.Range(-0.75f, 0.75f);

            float camX = transform.parent.transform.position.x + shakeX;

            if (camX - (horzExtent) < leftEdge.position.x)
                camX = leftEdge.position.x + (horzExtent);
            else if (camX + (horzExtent) > rightEdge.position.x)
                camX = rightEdge.position.x - (horzExtent);

            Debug.Log(horzExtent);
            //Debug.LogFormat("x:{0}, width: {1}, leftEdge: {2}, rightEdge: {3}", camX, horzExtent, leftEdge.position.x, rightEdge.position.x);

            if (camX + horzExtent > rightEdge.position.x)
                camX = rightEdge.position.x - horzExtent;
            else if (camX - horzExtent < leftEdge.position.x)
                camX = leftEdge.position.x - horzExtent;

            setX(self, camX);
			setY(self, preShakePos.y + shakeY);
            
		}
        else if (justShook)
        {
            transform.position = preShakePos;
            justShook = false;
        }
	}

    private void nextRound()
    {
        PlayerPrefs.SetInt("loadedFromEditor", 1);
        int limit = 2;
        switch (PlayerPrefs.GetInt("settingBestOf"))
        {
            case 3:
                limit = 3;
                break;
            case 5:
                limit = 4;
                break;
            case 7:
                limit = 5;
                break;
            case 9:   
                limit = 6;
                break;
        }
        if (RoundManager.instance.roundCounter.value < limit)
            SceneManager.LoadScene("SKF");
        else
        {
            StartCoroutine(RoundManager.instance.FadeInWin());
            SceneManager.LoadScene("Menu");
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
