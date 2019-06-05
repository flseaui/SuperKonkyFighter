using System.Collections;
using UnityEngine;

namespace UI
{
    public class SelectorColor : MonoBehaviour {

        public SpriteRenderer Sr;
        public Gradient Gradient;

        public float StrobeDuration = 2f;

        private void Update()
        {
            StartCoroutine("UpdateColor");
        }

        IEnumerator UpdateColor()
        {
            float t = Mathf.PingPong(Time.time / StrobeDuration, 1f);
            Sr.color = Gradient.Evaluate(t);

            yield return null;
        }
    }
}
