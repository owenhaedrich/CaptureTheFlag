using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_Text RedTeamScore;
    [SerializeField] public TMP_Text BlueTeamScore;

    public void UpdateScoreText(int score1, int score2)
    {
        RedTeamScore.text = $"Red Team: {score1}";
        BlueTeamScore.text = $"Blue Team: {score2}";
    }
}
