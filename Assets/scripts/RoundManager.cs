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

    [SerializeField] private Image p1Win1, p1Win2, p2Win1, p2Win2;
    [SerializeField] private TextMeshProUGUI roundText;
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
        InputManager.isInputEnabled = false;
        if (roundCounter.value <= 0)
        {
            p1Wins.value = 0;
            p2Wins.value = 0;
        }
        p1Win1.enabled = false;
        p1Win2.enabled = false;
        if (p1Wins.value > 0)
            p1Win1.enabled = true;
        if (p1Wins.value > 1)
        {
            p1Win2.enabled = true;
            StartCoroutine(FadeInWin());
        }

        p2Win1.enabled = false;
        p2Win2.enabled = false;
        if (p2Wins.value > 0)
            p2Win1.enabled = true;
        if (p2Wins.value > 1)
        {
            p2Win2.enabled = true;
            StartCoroutine(FadeInWin());
        }

        ++roundCounter.value;
        roundText.SetText("round " + roundCounter.value.ToString());
        StartCoroutine(FadeInRound());
    }

    private IEnumerator FadeInRound()
    {
        roundText.color = new Color(roundText.color.r, roundText.color.g, roundText.color.b, 0);
        while (roundText.color.a < 1.0f)
        {
            roundText.color = new Color(roundText.color.r, roundText.color.g, roundText.color.b, roundText.color.a + (Time.deltaTime * roundTimeMultiplier));

            yield return null;
        }
        roundText.color = Color.clear;
        StartCoroutine(FadeInFight());
    }

    private IEnumerator FadeInWin()
    {
        winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, 0);
        while (roundText.color.a < 1.0f)
        {
            winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, winText.color.a + (Time.deltaTime * winTimeMultiplier));

            yield return null;
        }
        SceneManager.LoadScene("Menu");
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
