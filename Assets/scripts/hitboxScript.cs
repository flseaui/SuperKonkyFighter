using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour
{
	private PlayerScript s;
	private PlayerScript os;

	int selfID;

    public bool hit;

	// Use this for initialization
	void Start()
	{
		s = GetComponentInParent<PlayerScript>();
		os = s.otherPlayer.GetComponent<PlayerScript>();
	}

	// Update is called once per frame
	void Update()
	{
		if (s.y() <= (os.height + os.y()) && s.y() >= os.y())
		{
			if (s.x() < os.x())
			{
				float tx = -os.width * Mathf.Sqrt(1 - Mathf.Pow(((s.y() - os.y()) / os.height), 2)) + os.x();

				//float tx = os.x() - (s.width / 2) + ((os.width / 2) * (((s.y() - os.y()) / os.height) - 1));
				if (s.x() > tx)
				{
					if (s.hVelocity > 0)
					{
						if(!s.air){
							os.hVelocity += s.hVelocity / 2;
							s.hVelocity -= s.hVelocity / 2;
						}
						else if(os.air)
						{
							os.hVelocity += s.hVelocity / 2;
							s.hVelocity -= s.hVelocity / 2;
						}
					}
					s.setX(tx);
				}
			}
			else
			{
				float tx = os.width * Mathf.Sqrt(1 - Mathf.Pow(((s.y() - os.y()) / os.height), 2)) + os.x();

				//float tx = (os.x() + (s.width / 2) - ((os.width / 2) * (((s.y() - os.y()) / os.height) - 1)));
				if (s.x() < tx)
				{
					if (s.hVelocity < 0)
					{
						if (!s.air)
						{
							os.hVelocity += s.hVelocity / 2;
							s.hVelocity -= s.hVelocity / 2;
						}
						else if (os.air)
						{
							os.hVelocity += s.hVelocity / 2;
							s.hVelocity -= s.hVelocity / 2;
						}
					}
					s.setX(tx);
				}
			}

		}

		//line.SetPosition(0, new Vector3((os.x() - (s.width / 2) + (os.width / 2) - 1), os.y(), 0));
		//line.SetPosition(1, new Vector3((os.x() - (s.width / 2) + ((os.width / 2) - 1)), os.y() + os.height, 0));

		//line.SetPosition(2, new Vector3((os.x() + (s.width / 2) - ((os.width / 2) - 1)), os.y() + os.height, 0));
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		
		if (!col.CompareTag(this.tag)) {
			Debug.Log("other tag: " + col.tag);
			Debug.Log("this tag: " + tag);
			hit = true;
			if (col.enabled && col.GetComponentInParent<PlayerScript>().playerID != selfID)
			{
				if (s.state == 4 && !s.action)
				{
                    //s.block();
				}
				else
				{
					s.damage(os.damagePass);
                    
                    if(s.type == 0)
                    {
						s.stun((int)s.level(2));
                    }
                    else
                    {
						s.stun((int)s.level(1));
					}
				}
			}
		}
	}
}