using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RoundManager : MonoBehaviour
{
    #region Singleton

    public static RoundManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of RoundManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject roundManager;

    public IntVariable roundCounter, p1Wins, p2Wins;
    public bool stop = false;

    [SerializeField] private Image p1Win1, p1Win2, p2Win1, p2Win2;
     public TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI fightText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private float roundTimeMultiplier;
    [SerializeField] private float fightInTimeMultiplier;
    [SerializeField] private float fightOutTimeMultiplier;
    [SerializeField] private float winTimeMultiplier;

    private void Start()
    {
        roundText.color = Color.clear;
        fightText.color = Color.clear;
        nextRound();
    }

    public void nextRound()
    {
        if (PlayerPrefs.GetInt("loadedFromEditor") == 0)
        {
            roundCounter.value = 0;
            p1Wins.value = 0;
            p2Wins.value = 0;
        }

        InputManager.isInputEnabled = false;
        if (roundCounter.value <= 0)
        {
            p1Wins.value = 0;
            p2Wins.value = 0;
            stop = false;

            PlayerPrefs.SetInt("P1Meter", 0);
            PlayerPrefs.SetInt("P2Meter", 0);

            if (AudioManager.Instance != null)
            {
                switch(PlayerPrefs.GetInt("stage"))
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
                AudioManager.Instance.PlayMusic((AudioManager.Music)UnityEngine.Random.Range(2, 8));
            }
        }
        int limit = 2;
        switch (PlayerPrefs.GetInt("settingBestOf"))
        {
            case 3:
                limit = 2;
                break;
            case 5:
                limit = 3;
                break;
            case 7:
                limit = 4;
                break;
            case 9:
                limit = 5;
                break;
        }

        p1Win1.enabled = false;
        p1Win2.enabled = false;
        if (p1Wins.value > 0)
            p1Win1.enabled = true;
        if (p1Wins.value > 1)
            p1Win2.enabled = true;
        if (p1Wins.value >= limit)
            stop = true;

        p2Win1.enabled = false;
        p2Win2.enabled = false;
        if (p2Wins.value > 0)
            p2Win1.enabled = true;
        if (p2Wins.value > 1)
            p2Win2.enabled = true;
        if (p2Wins.value >= limit)
            stop = true;

        if (!stop)
        {
            ++roundCounter.value;
            if (roundCounter.value >= PlayerPrefs.GetInt("settingBestOf"))
            {
                roundText.SetText("final round");
            }
            else
                roundText.SetText("round " + roundCounter.value.ToString());
            StartCoroutine(FadeInRound(false, 0));
        }
        else
        {
            StartCoroutine(FadeInWin());
        }
    }

    public IEnumerator FadeInRound(bool toMenu, int player)
    {
        if (toMenu)
        {
            roundText.text = "player " + player + " wins";
        }

        roundText.color = new Color(roundText.color.r, roundText.color.g, roundText.color.b, 0);
        while (roundText.color.a < 1.0f)
        {
            roundText.color = new Color(roundText.color.r, roundText.color.g, roundText.color.b, roundText.color.a + (Time.deltaTime * roundTimeMultiplier));

            yield return null;
        }
        if (toMenu)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            roundText.color = Color.clear;
            StartCoroutine(FadeInFight());
        }
    }

    public IEnumerator FadeInWin()
    {
        winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, 0);
        while (roundText.color.a < 1.0f)
        {
            winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, winText.color.a + (Time.deltaTime * winTimeMultiplier));

            yield return null;
        }
    }

    private IEnumerator FadeInFight()
    {
        fightText.color = new Color(fightText.color.r, fightText.color.g, fightText.color.b, 0);
        while (fightText.color.a < 1.0f)
        {
            fightText.color = new Color(fightText.color.r, fightText.color.g, fightText.color.b, fightText.color.a + (Time.deltaTime * fightInTimeMultiplier));

            yield return null;
        }

        yield return new WaitForSeconds(1);

        fightText.color = Color.clear;
        StartCoroutine(FadeOutFight());
    }

    private IEnumerator FadeOutFight()
    {
        fightText.color = new Color(fightText.color.r, fightText.color.g, fightText.color.b, 1);
        while (fightText.color.a > 0.0f)
        {
            fightText.color = new Color(fightText.color.r, fightText.color.g, fightText.color.b, fightText.color.a - (Time.deltaTime * fightOutTimeMultiplier));
            yield return null;
        }
        InputManager.isInputEnabled = true;
        roundManager.SetActive(false);
    }
}
