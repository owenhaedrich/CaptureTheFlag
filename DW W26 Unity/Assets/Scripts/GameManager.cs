using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public UIManager UI;

    public int redTeamScore { get; private set; } = 0;
    public int blueTeamScore { get; private set; } = 0;

    private void Start()
    {
        UI.UpdateScoreText(redTeamScore, blueTeamScore);
    }

    public void ScoreGoal (Team team)
    {
        switch (team)
        {
            case Team.Red:
                redTeamScore++;
                break;
            case Team.Blue:
                blueTeamScore++;
                break;
        }

        UI.UpdateScoreText(redTeamScore, blueTeamScore);
    }
}
