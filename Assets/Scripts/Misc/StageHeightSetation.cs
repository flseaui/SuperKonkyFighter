using UnityEngine;

namespace Misc
{
    public class StageHeightSetation : MonoBehaviour {
        private void Start () {
        
            if (PlayerPrefs.GetInt("stage") <= 2)
                transform.position = new Vector3(0, 35.2f, 65);
            else if (PlayerPrefs.GetInt("stage") == 4)
                transform.position = new Vector3(0, 50, 65);
            else
                transform.position = new Vector3(0, 35.2f, 65);
        }

    }
}
