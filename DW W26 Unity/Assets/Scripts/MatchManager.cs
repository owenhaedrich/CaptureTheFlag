using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchManager : MonoBehaviour
{
    [SerializeField] TMP_Text CountdownText;
    [SerializeField] TMP_Text TimerText;
    [SerializeField] GameObject MenuOverlay;

    [SerializeField] float StartCountdownSeconds = 3f;
    [SerializeField] float MatchSeconds = 60f;

    bool matchRunning = false;
    bool waitingInMenu = true;
    float timeLeft = 0f;

    private void Start()
    {
        timeLeft = MatchSeconds;

        if (MenuOverlay != null) MenuOverlay.SetActive(true);

        Time.timeScale = 0f;
        matchRunning = false;
        waitingInMenu = true;
    }

    private void Update()
    {
        if (waitingInMenu)
        {
            if (AnyPlayerPressedStart())
            {
                waitingInMenu = false;
                if (MenuOverlay != null) MenuOverlay.SetActive(false);
                StartCoroutine(StartCountdown());
            }
            return;
        }

        if (!matchRunning) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;

        if (TimerText != null)
        {
            int seconds = Mathf.CeilToInt(timeLeft);
            TimerText.text = seconds.ToString();
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

        float t = StartCountdownSeconds;
        while (t > 0f)
        {
            if (CountdownText != null) CountdownText.text = Mathf.CeilToInt(t).ToString();
            yield return new WaitForSecondsRealtime(1f);
            t -= 1f;
        }

        if (CountdownText != null) CountdownText.text = "GO!";
        Time.timeScale = 1f;
        matchRunning = true;

        yield return new WaitForSeconds(1f);
        if (CountdownText != null) CountdownText.text = "";
    }

    System.Collections.IEnumerator RestartRoutine()
    {
        Time.timeScale = 0f;
        if (CountdownText != null) CountdownText.text = "TIME!";

        yield return new WaitForSecondsRealtime(15f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
