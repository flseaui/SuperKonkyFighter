using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI fightText;
    [SerializeField] private float roundTimeMultiplier;
    [SerializeField] private float fightInTimeMultiplier;
    [SerializeField] private float fightOutTimeMultiplier;

    private int roundNum = 0;

    private void Start()
    {
        roundText.color = Color.clear;
        fightText.color = Color.clear;
        nextRound();
    }

    public void nextRound()
    {
        ++roundNum;
        roundText.SetText("round " + roundNum.ToString());
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
        roundManager.SetActive(false);
    }
}
