using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public IntVariable time;
    public TMPro.TMP_Text timerText;

    public int startTime;

    private bool started;

	void Start()
    {
        this.transform.rotation.Set(0, 0, 90, Quaternion.identity.w);
        time.value = PlayerPrefs.GetInt("settingMatchTime");
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
        if(time.value < 0)
            timerText.text = "8";
        else
            timerText.text = time.value.ToString();
        if (!started && InputManager.isInputEnabled && time.value >= 0)
        {
            started = true;
            StartCoroutine("tickDown");
        }
    }

}
