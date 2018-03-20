using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public bool history;

	public Sprite[] Background;
	public Sprite[] Ground;

	public JoyScript JoyScript;

	public Button lightButton;
	public Button mediumButton;
	public Button heavyButton;

	private int megaKek;

	public int shakeX;
	public int shakeY;
	public bool shake;

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

		history = true;

        ghost.GetComponent<BackGroundScript>().setScripts(p1s, p2s);

        Background = new Sprite[] { background0, background1, background2, background3, background4, background5, background6 };
		Ground = new Sprite[] { ground0, ground1 };

		background.GetComponent<SpriteRenderer>().sprite = Background[PlayerPrefs.GetInt("background", 0)];
		ground.GetComponent<SpriteRenderer>().sprite = Ground[PlayerPrefs.GetInt("ground", 0)];
	}

    void Update()
    {

     /*   if (p1h.hit)
        {
			megaKek = 1;

			p1h.hit = false;

            Action currentAction = p2s.behaviors.getAction(p2s.currentAction);

            int damage = currentAction.damage[p2s.damageCounter];
            float aKnockback = currentAction.aStrength;
            float gKnockback = currentAction.gStrength;
            int aAngle = currentAction.aAngle;
            int gAngle = currentAction.gAngle;
            int tier = currentAction.tier;

            if (p1s.basicState == 4 && p1s.currentAction == 0 || p1s.currentAction == 100000)
			{
				p1s.block((int)p2s.level(3));

                if (p1s.air)
                {
                    p2s.meter += p1s.damage(damage / 10, aKnockback / 2, aAngle, tier, true);
                    p1s.meter += 2;
                }
                else
                {
                    p2s.meter += p1s.damage(damage / 10, gKnockback / 2, gAngle, tier, true);
                    p1s.meter += 2;
                }
				
			}
			else
			{
                if (p1s.air)
                {
                    p2s.meter += p1s.damage(damage, aKnockback, aAngle, tier, false);
                    p1s.meter += 2;
                }
                else
                {
                    p2s.meter += p1s.damage(damage, gKnockback, gAngle, tier, false);
                    p1s.meter += 2;
                }
                
				if (p1s.type == 0)
				{
					p1s.stun((int)p1s.level(2));
				}
				else
				{
					p1s.stun((int)p1s.level(1));
				}
			}
		}

        if (p2h.hit)
        {
			megaKek = 2;

			p2h.hit = false;

            Action currentAction = p1s.behaviors.getAction(p1s.currentAction);

            int damage = currentAction.damage[p1s.damageCounter];
            float aKnockback = currentAction.aStrength;
            float gKnockback = currentAction.gStrength;
            int aAngle = currentAction.aAngle;
            int gAngle = currentAction.gAngle;
            int tier = currentAction.tier;

            if (p2s.basicState == 4 && p2s.currentAction == 0 || p2s.currentAction == 10000)
			{

				p2s.block((int)p2s.level(3));
                if (p2s.air)
                {
                    p1s.meter += p2s.damage(damage / 10, aKnockback / 2, aAngle, tier, true);
                    p2s.meter += 2;
                }
                else
                {
                    p1s.meter += p2s.damage(damage / 10, gKnockback / 2, gAngle, tier, true);
                    p2s.meter += 2;
                }

            }
            else
            {
                if (p2s.air)
                {
                    p1s.meter += p2s.damage(damage, aKnockback, aAngle, tier, false);
                    p2s.meter += 2;
                }
                else
                {
                    p1s.meter += p2s.damage(damage, gKnockback, gAngle, tier, false);
                    p2s.meter += 2;
                }

                if (p2s.currentFrame == 0)
				{
					p2s.stun((int)p2s.level(2));
				}
				else
				{
					p2s.stun((int)p2s.level(1));
				}
			}
		}
        */
		float cx = (getX(player1) + getX(player2)) / 2f;
        if (cx > 42)
        {
            cx = 42;
        } else if (cx < -42)
        {
            cx = -42;
        }
        setX(background, cx * 0.5f);
        setX(self, cx);
 
		bool now = getX(player1) < getX(player2);

		if (now != history) {
			if (getX(player1) < getX(player2))
			{
                player1.GetComponent<PlayerScript>().flipFacing = true;
                player1.GetComponent<PlayerScript>().flip = true;
                player2.GetComponent<PlayerScript>().flipFacing = false;
                player2.GetComponent<PlayerScript>().flip = true;
            }
			else
			{
                player1.GetComponent<PlayerScript>().flipFacing = false;
                player1.GetComponent<PlayerScript>().flip = true;
                player2.GetComponent<PlayerScript>().flipFacing = true;
                player2.GetComponent<PlayerScript>().flip = true;
            }
		}

		history = getX(player1) < getX(player2);

		uis.health1.maxValue = p1s.maxHealth;
		uis.health1.minValue = 0;
		uis.health2.maxValue = p2s.maxHealth;
		uis.health2.minValue = 0;
		uis.health1.value = p1s.health;
        uis.meter1.value = p1s.meter;
		uis.health2.value = p2s.health;
        uis.meter2.value = p2s.meter;

        if (shake)
		{            
			shakeX = Random.Range(-1,1);
			shakeY = Random.Range(-1, 1);
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

	private void setY(Camera o, float amm)
	{
		Vector3 position = o.transform.position;
		position.y = amm;
		o.transform.position = position;
	}
}
