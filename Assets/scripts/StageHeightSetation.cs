using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHeightSetation : MonoBehaviour {

	void Start () {
        
        if (PlayerPrefs.GetInt("stage") <= 2)
            transform.position = new Vector3(0, 35.2f, 65);
        else if (PlayerPrefs.GetInt("stage") == 4)
            transform.position = new Vector3(0, 50, 65);
        else
            transform.position = new Vector3(0, 35.2f, 65);
    }

}
