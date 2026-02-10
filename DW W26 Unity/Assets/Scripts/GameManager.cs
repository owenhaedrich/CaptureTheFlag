using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public UIManager UI;

    int redTeamScore = 0;
    int blueTeamScore = 0;

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
