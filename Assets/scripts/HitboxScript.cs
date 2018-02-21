using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
	private PlayerScript s;
	private PlayerScript os;

	int selfID;

	public bool hit;

	public bool initialFrame;
	public bool colliding;

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
                if (s.x() > tx)
                {
                    if (s.hVelocity > 0)
                    {
                        if (!s.air || os.air)
                        {
                            os.hVelocity += s.hVelocity / 2;
                            s.hVelocity -= s.hVelocity / 2;
                            colliding = true;
                        }
                    }
                    s.setX(tx);
                }
                else
                {
                    colliding = false;
                }
            }
            else
            {
                float tx = os.width * Mathf.Sqrt(1 - Mathf.Pow(((s.y() - os.y()) / os.height), 2)) + os.x();
                if (s.x() < tx)
                {
                    if (s.hVelocity < 0)
                    {
                        if (!s.air || os.air)
                        {
                            os.hVelocity += s.hVelocity / 2;
                            s.hVelocity -= s.hVelocity / 2;
                            colliding = true;
                        }
                    }
                    s.setX(tx);
                }
                else
                {
                    colliding = false;
                }
            }
        }
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		/*if (!col.CompareTag(this.tag) && !initialFrame && col.enabled)
		{
			initialFrame = true;
			Debug.Log("other tag: " + col.tag);
			Debug.Log("this tag: " + tag);
			hit = true;
		}*/
	}
}