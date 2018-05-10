using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCounterScript : MonoBehaviour {

    public IntVariable comboCounterText;
    
    public TextMeshProUGUI timerText;

    private Color originalColor;

    public int alpha;
    private int lastValue;

    void Start () {
        originalColor = timerText.color;
        timerText.color = Color.clear;
        comboCounterText.value = 0;
        alpha = 255;
        lastValue = 0;
    }

    private IEnumerator fade()
    {
        
        for (float t = 0.01f; t < 3; t += Time.deltaTime)
        {
            timerText.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / 3));
            yield return null;
        }
    }


    void Update () {
        if (comboCounterText.value != lastValue)
        {
            timerText.text = comboCounterText.value.ToString();
            lastValue = comboCounterText.value;
            timerText.color = originalColor;
            StartCoroutine("fade");
        }
    }
}
