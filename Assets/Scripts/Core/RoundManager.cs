using System.Collections;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class RoundManager : MonoBehaviour
    {
        #region Singleton

        public static RoundManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of RoundManager found!");
                return;
            }
            Instance = this;
        }

        #endregion

        public GameObject RoundManagerObj;

        public IntVariable RoundCounter, P1Wins, P2Wins;
        public bool Stop;

        [SerializeField] private Image P1Win1, P1Win2, P2Win1, P2Win2;
        public TextMeshProUGUI RoundText;
        [SerializeField] private TextMeshProUGUI FightText;
        [SerializeField] private TextMeshProUGUI WinText;
        [SerializeField] private float RoundTimeMultiplier;
        [SerializeField] private float FightInTimeMultiplier;
        [SerializeField] private float FightOutTimeMultiplier;
        [SerializeField] private float WinTimeMultiplier;

        private void Start()
        {
            RoundText.color = Color.clear;
            FightText.color = Color.clear;
            NextRound();
        }

        private void NextRound()
        {
            if (PlayerPrefs.GetInt("loadedFromEditor") == 0)
            {
                RoundCounter.Value = 0;
                P1Wins.Value = 0;
                P2Wins.Value = 0;
            }

            InputManager.IsInputEnabled = false;
            if (RoundCounter.Value <= 0)
            {
                P1Wins.Value = 0;
                P2Wins.Value = 0;
                Stop = false;

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
                    AudioManager.Instance.PlayMusic((AudioManager.Music)Random.Range(2, 8));
                }
            }
            var limit = 2;
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

            P1Win1.enabled = false;
            P1Win2.enabled = false;
            if (P1Wins.Value > 0)
                P1Win1.enabled = true;
            if (P1Wins.Value > 1)
                P1Win2.enabled = true;
            if (P1Wins.Value >= limit)
                Stop = true;

            P2Win1.enabled = false;
            P2Win2.enabled = false;
            if (P2Wins.Value > 0)
                P2Win1.enabled = true;
            if (P2Wins.Value > 1)
                P2Win2.enabled = true;
            if (P2Wins.Value >= limit)
                Stop = true;

            if (!Stop)
            {
                ++RoundCounter.Value;
                if (RoundCounter.Value >= PlayerPrefs.GetInt("settingBestOf"))
                {
                    RoundText.SetText("final round");
                }
                else
                    RoundText.SetText("round " + RoundCounter.Value);
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
                RoundText.text = "player " + player + " wins";
            }

            RoundText.color = new Color(RoundText.color.r, RoundText.color.g, RoundText.color.b, 0);
            while (RoundText.color.a < 1.0f)
            {
                RoundText.color = new Color(RoundText.color.r, RoundText.color.g, RoundText.color.b, RoundText.color.a + (Time.deltaTime * RoundTimeMultiplier));

                yield return null;
            }
            if (toMenu)
            {
                SceneManager.LoadScene("Menu");
            }
            else
            {
                RoundText.color = Color.clear;
                StartCoroutine(FadeInFight());
            }
        }

        private IEnumerator FadeInWin()
        {
            WinText.color = new Color(WinText.color.r, WinText.color.g, WinText.color.b, 0);
            while (RoundText.color.a < 1.0f)
            {
                WinText.color = new Color(WinText.color.r, WinText.color.g, WinText.color.b, WinText.color.a + (Time.deltaTime * WinTimeMultiplier));

                yield return null;
            }
        }

        private IEnumerator FadeInFight()
        {
            FightText.color = new Color(FightText.color.r, FightText.color.g, FightText.color.b, 0);
            while (FightText.color.a < 1.0f)
            {
                FightText.color = new Color(FightText.color.r, FightText.color.g, FightText.color.b, FightText.color.a + (Time.deltaTime * FightInTimeMultiplier));

                yield return null;
            }

            yield return new WaitForSeconds(1);

            FightText.color = Color.clear;
            StartCoroutine(FadeOutFight());
        }

        private IEnumerator FadeOutFight()
        {
            FightText.color = new Color(FightText.color.r, FightText.color.g, FightText.color.b, 1);
            while (FightText.color.a > 0.0f)
            {
                FightText.color = new Color(FightText.color.r, FightText.color.g, FightText.color.b, FightText.color.a - (Time.deltaTime * FightOutTimeMultiplier));
                yield return null;
            }
            InputManager.IsInputEnabled = true;
            RoundManagerObj.SetActive(false);
        }
    }
}
