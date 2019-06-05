using System.Collections;
using Core;
using Misc;
using UnityEngine;

namespace UI
{
    public class TimerScript : MonoBehaviour
    {
        public IntVariable Time;
        public TMPro.TMP_Text TimerText;

        public int StartTime;

        private bool _started;

        void Start()
        {
            Time.Value = PlayerPrefs.GetInt("settingMatchTime");
        }
	
        IEnumerator TickDown()
        {
            while (true)
            {
                if (Time.Value > 0)
                    Time.Value--;
                else
                    StopCoroutine("TickDown");
                yield return new WaitForSeconds(1);
            }
        }

        void Update()
        {
            if (Time.Value < 0)
            {
                TimerText.text = "8";
                TimerText.transform.rotation = new Quaternion(0, 0, 90, Quaternion.identity.w);
            }
            else
                TimerText.text = Time.Value.ToString();
            if (!_started && InputManager.IsInputEnabled && Time.Value >= 0)
            {
                _started = true;
                StartCoroutine("TickDown");
            }
        }

    }
}
