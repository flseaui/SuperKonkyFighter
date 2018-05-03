using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeScript : MonoBehaviour
{
    public SpriteRenderer sr;
    public Gradient gradient;

    public float strobeDuration = 2f;

    private void Update()
    {
        // if on training grounds
        if (PlayerPrefs.GetInt("stage") == 4)
            StartCoroutine("UpdateColor");
    }

    IEnumerator UpdateColor()
    {
        float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
        sr.color = gradient.Evaluate(t);
        
        yield return null;
    }
}
