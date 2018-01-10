using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxScript : MonoBehaviour
{
	public bool hit;
	public bool collide;
	private PlayerScript s;
	private PlayerScript os;
	public LineRenderer line;

	// Use this for initialization
	void Start()
	{
		hit = false;
		collide = false;
		s = GetComponentInParent<PlayerScript>();
		os = s.otherPlayer.GetComponent<PlayerScript>();
		line.positionCount = 3;
		line.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (s.y() <= (os.height + os.y()) && s.y() >= os.y())
		{
			if (s.x() < os.x())
			{
				float tx = (os.x() - (s.width / 2) + ((os.width / 2) * (((s.y()-os.y()) / os.height) - 1)));
				//float tx = os.x() - (s.width / 2) + ((os.width / 2) * (((s.y() - os.y()) / os.height) - 1));
				if (s.x() > tx)
				{
					s.setX(tx);
				}
			}
			else
			{
				float tx = (os.x() + (s.width / 2) - ((os.width / 2) * (((s.y()-os.y()) / os.height) - 1)));
				//float tx = (os.x() + (s.width / 2) - ((os.width / 2) * (((s.y() - os.y()) / os.height) - 1)));
				if (s.x() < tx)
				{
					s.setX(tx);
				}
			}

			line.startColor = Color.red;
		}

		//line.SetPosition(0, new Vector3((os.x() - (s.width / 2) + (os.width / 2) - 1), os.y(), 0));
		//line.SetPosition(1, new Vector3((os.x() - (s.width / 2) + ((os.width / 2) - 1)), os.y() + os.height, 0));

		//line.SetPosition(2, new Vector3((os.x() + (s.width / 2) - ((os.width / 2) - 1)), os.y() + os.height, 0));
	}

	private void OnPostRender()
	{
		Debug.Log("kekeke");
	}

	private void OnTriggerStay2D(Collider2D col)
	{
		if (hit)
		{
			hit = false;
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (hit)
		{
			hit = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.enabled)
		{
			hit = true;
		}
	}
}