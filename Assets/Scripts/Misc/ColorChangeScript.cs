using System.Collections;
using UnityEngine;

namespace Misc
{
    public class ColorChangeScript : MonoBehaviour
    {
        public SpriteRenderer Sr;
        public Gradient Gradient;

        public float StrobeDuration = 2f;

        private void Update()
        {
            // if on training grounds
            if (PlayerPrefs.GetInt("stage") == 4)
                StartCoroutine(nameof(UpdateColor));
        }

        private IEnumerator UpdateColor()
        {
            var t = Mathf.PingPong(Time.time / StrobeDuration, 1f);
            Sr.color = Gradient.Evaluate(t);
        
            yield return null;
        }
    }
}
