using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeScript : MonoBehaviour
{

    public Gradient gradient;
    public float strobeDuration = 2f;

    public SpriteRenderer sr;

    private void Start()
    {
        StartCoroutine("UpdateColor");
    }

    IEnumerator UpdateColor()
    {
        float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
        sr.color = gradient.Evaluate(t);
        
        yield return null;
    }

    private void Update()
    {
        StartCoroutine("UpdateColor");
    }
}
