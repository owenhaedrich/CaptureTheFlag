using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchManager : MonoBehaviour
{
    [SerializeField] TMP_Text CountdownText;
    [SerializeField] TMP_Text TimerText;

    [SerializeField] float StartCountdownSeconds = 3f;
    [SerializeField] float MatchSeconds = 60f;

    bool matchRunning = false;
    float timeLeft = 0f;

    private void Start()
    {
        timeLeft = MatchSeconds;
        StartCoroutine(StartCountdown());
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

    private void Update()
    {
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

    System.Collections.IEnumerator RestartRoutine()
    {
        Time.timeScale = 0f;
        if (CountdownText != null) CountdownText.text = "TIME!";

        yield return new WaitForSecondsRealtime(15f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
