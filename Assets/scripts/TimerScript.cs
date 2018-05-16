using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public IntVariable time;
    public TMPro.TMP_Text timerText;

    public int startTime;

	void Start()
    {
        time.value = PlayerPrefs.GetInt("settingMatchTime");
        StartCoroutine("tickDown");
	}
	
	IEnumerator tickDown()
    {
        while (true)
        {
            if (time.value > 0)
                time.value--;
            else
                StopCoroutine("tickDown");
            yield return new WaitForSeconds(1);
        }
	}

    void Update()
    {
        timerText.text = time.value.ToString();
    }

}
