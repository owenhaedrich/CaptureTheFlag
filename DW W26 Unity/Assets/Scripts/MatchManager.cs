using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchManager : MonoBehaviour
{
    [SerializeField] TMP_Text RedCountdownText;
    [SerializeField] TMP_Text RedTimerText;
    [SerializeField] GameObject RedMenuOverlay;

    [SerializeField] TMP_Text BlueCountdownText;
    [SerializeField] TMP_Text BlueTimerText;
    [SerializeField] GameObject BlueMenuOverlay;

    [Space(10)]
    [SerializeField] GameObject RedWinOverlay;
    [SerializeField] GameObject BlueWinOverlay;
    [SerializeField] GameManager gameManager;

    [SerializeField] float StartCountdownSeconds = 3f;
    [SerializeField] float MatchSeconds = 60f;

    bool matchRunning = false;
    bool waitingInMenu = true;
    float timeLeft = 0f;

    private void Start()
    {
        timeLeft = MatchSeconds;

        if (RedMenuOverlay != null)
        {
            RedMenuOverlay.SetActive(true);
        }
        if (BlueMenuOverlay != null)
        {
            BlueMenuOverlay.SetActive(true);
        }

        if (RedWinOverlay != null)
        {
            RedWinOverlay.SetActive(false);
        }
        if (BlueWinOverlay != null)
        {
            BlueWinOverlay.SetActive(false);
        }

        Time.timeScale = 0f;
        matchRunning = false;
        waitingInMenu = true;

        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.PlayMenu();
    }

    private void Update()
    {
        if (waitingInMenu)
        {
            if (AnyPlayerPressedStart())
            {
                waitingInMenu = false;
                if (RedMenuOverlay != null)
                {
                    RedMenuOverlay.SetActive(false);
                }
                if (BlueMenuOverlay != null)
                {
                    BlueMenuOverlay.SetActive(false);
                }
                StartCoroutine(StartCountdown());
            }
            return;
        }

        if (!matchRunning) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;

        if (RedTimerText != null)
        {
            int seconds = Mathf.CeilToInt(timeLeft);
            RedTimerText.text = seconds.ToString();
        }
        if (BlueTimerText != null)
        {
            int seconds = Mathf.CeilToInt(timeLeft);
            BlueTimerText.text = seconds.ToString();
        }

        if (timeLeft <= 0f)
        {
            matchRunning = false;
            StartCoroutine(RestartRoutine());
        }
    }

    bool AnyPlayerPressedStart()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].PressedBoostThisFrame())
                return true;
        }
        return false;
    }

    System.Collections.IEnumerator StartCountdown()
    {
    matchRunning = false;
    Time.timeScale = 0f;

    if (SoundManager.Instance != null)
        SoundManager.Instance.PlayRoundStart();

    float t = StartCountdownSeconds;
    while (t > 0f)
    {
        if (RedCountdownText != null) RedCountdownText.text = Mathf.CeilToInt(t).ToString();
        if (BlueCountdownText != null) BlueCountdownText.text = Mathf.CeilToInt(t).ToString();
        yield return new WaitForSecondsRealtime(1f);
        t -= 1f;
    }

    if (RedCountdownText != null) RedCountdownText.text = "GO!";
    if (BlueCountdownText != null) BlueCountdownText.text = "GO!";


    Time.timeScale = 1f;
    matchRunning = true;

    if (BackgroundMusic.Instance != null)
        BackgroundMusic.Instance.PlayBattle();

    yield return new WaitForSeconds(1f);
    if (RedCountdownText != null) RedCountdownText.text = "";
    if (BlueCountdownText != null) BlueCountdownText.text = "";
    }

    System.Collections.IEnumerator RestartRoutine()
    {
        Time.timeScale = 0f;
        if (RedCountdownText != null) RedCountdownText.text = "TIME!";
        if (BlueCountdownText != null) BlueCountdownText.text = "TIME!";

        if (BackgroundMusic.Instance != null)
            BackgroundMusic.Instance.PlayEnd();

        if (gameManager != null)
        {
            if (gameManager.redTeamScore > gameManager.blueTeamScore)
            {
                if (RedWinOverlay != null) RedWinOverlay.SetActive(true);
            }
            else if (gameManager.blueTeamScore > gameManager.redTeamScore)
            {
                if (BlueWinOverlay != null) BlueWinOverlay.SetActive(true);
            }
        }

        yield return new WaitForSecondsRealtime(15f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
