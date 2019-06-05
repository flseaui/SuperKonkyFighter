using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIScript : MonoBehaviour {

        public Slider Health1;
        public Slider Health2;
        public Slider Health1P;
        public Slider Health2P;
        public Slider Meter1;
        public Slider Meter2;
        public Slider Meter1P;
        public Slider Meter2P;

        public GameObject Back;

        // Use this for initialization
        void Start ()
        {
            Vector3 position = Back.transform.position;
            position.x = 0;
            Back.transform.position = position;

            position = Back.transform.position;
            position.y = 23.5f;
            Back.transform.position = position;
        }
    }
}
