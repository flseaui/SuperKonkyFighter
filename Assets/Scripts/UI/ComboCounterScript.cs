using System.Collections;
using Misc;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ComboCounterScript : MonoBehaviour {

        public IntVariable ComboCounterText;
    
        public TextMeshProUGUI TimerText;

        private Color _originalColor;

        public int Alpha;
        private int _lastValue;

        void Start () {
            _originalColor = TimerText.color;
            TimerText.color = Color.clear;
            ComboCounterText.Value = 0;
            Alpha = 255;
            _lastValue = 0;
        }

        private IEnumerator Fade()
        {
            for (float t = 0.01f; t < 2; t += Time.deltaTime)
            {
                TimerText.color = Color.Lerp(_originalColor, Color.clear, Mathf.Min(1, t / 2));
                yield return null;
            }
        }


        void Update() {
            if (ComboCounterText.Value > 1)
            {
                if (ComboCounterText.Value != _lastValue)
                {
                    TimerText.text = ComboCounterText.Value.ToString();
                    _lastValue = ComboCounterText.Value;
                    TimerText.color = _originalColor;
                    StopCoroutine("Fade");
                    StartCoroutine("Fade");
                }
            }
            else
                TimerText.color = Color.clear;
        }
    }
}
