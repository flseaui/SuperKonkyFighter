using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour {

    public PlayerScript p1s;
    public PlayerScript p2s;

    // Use this for initialization
    void Start () { }
	
    public void setScripts(PlayerScript p1s, PlayerScript p2s)
    {
        this.p1s = p1s;
        this.p2s = p2s;
    }

	// Update is called once per frame
	void Update () {

        if (p1s.updateEnd != 0 && p2s.updateEnd != 0)
        {
            if (p1s.updateEnd == 2)
            {
                p1s.updateEnd = 0;
                p1s.UpdateEnd();
            }
            if (p2s.updateEnd == 2)
            {
                p2s.updateEnd = 0;
                p2s.UpdateEnd();
            }

        }

    }
}
