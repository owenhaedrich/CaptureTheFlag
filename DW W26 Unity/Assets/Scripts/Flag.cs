using UnityEngine;

public class Flag : MonoBehaviour
{
    [field: SerializeField] public Team Team { get; private set; }
    [field: SerializeField] public float MagnetToPlayer { get; private set; } = 50;

    [field: SerializeField] GameManager GameManager;

    bool held;
    PlayerController heldByPlayer;
    float heldSpeed = 5f;
    float dropReturnTime;
    float dropReturnTimer;

    private void FixedUpdate()
    {
        if (held)
        {
            Vector2 moveTowardsDirection = heldByPlayer.transform.position - transform.position;
            float distanceFactor = (moveTowardsDirection.magnitude / 50f) * MagnetToPlayer;
            transform.position += (Vector3)moveTowardsDirection.normalized * heldSpeed * Time.fixedDeltaTime * distanceFactor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Flag {name} collided with {other.name}.");  
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (playerController.PlayerTeam != Team && !held)
            {
                heldByPlayer = playerController;
                held = true;
            }
        }

        // Check if it's a goal and if so, score if held by the correct team.
        if (other.TryGetComponent(out Goal goal))
        {
            if (goal.Team != Team)
                GameManager.ScoreGoal(goal.Team);
        }
    }
}
