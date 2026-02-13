using UnityEngine;

public class Flag : MonoBehaviour
{
    [field: SerializeField] public Team Team { get; private set; }
    [field: SerializeField] public float MagnetToPlayer { get; private set; } = 50;

    [field: SerializeField] GameManager GameManager;

    Vector2 homePosition;

    bool held;
    PlayerController heldByPlayer;
    float heldSpeed = 5f;

    [SerializeField] float dropReturnTime = 6f;
    float dropReturnTimer;

    float homeReturnSpeed = 50f;
    float homeReturnDistanceFactor = 2f;
    bool returningHome;

    private void Start()
    {
        homePosition = transform.position;
    }

    public void Drop()
    {
        if (!held) return;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayFlagDropped();

        if (heldByPlayer != null)
            heldByPlayer.ClearCarriedFlag(this);

        held = false;
        heldByPlayer = null;
        dropReturnTimer = 0f;
    }

    private void ResetFlag()
    {
        returningHome = true;

        if (heldByPlayer != null)
            heldByPlayer.ClearCarriedFlag(this);

        held = false;
        heldByPlayer = null;
        dropReturnTimer = 0;
    }

    private void FixedUpdate()
    {
        if (returningHome)
        {
            Vector2 moveTowardsHome = homePosition - (Vector2)transform.position;
            if (moveTowardsHome.magnitude < 0.01f)
            {
                transform.position = (Vector3)homePosition;
                returningHome = false;
            }
            else
            {
                float distanceFactor = (moveTowardsHome.magnitude / 50f) * homeReturnDistanceFactor;
                transform.position += (Vector3)moveTowardsHome.normalized * homeReturnSpeed * distanceFactor * Time.fixedDeltaTime;
            }
        }
        else if (held)
        {
            Vector2 moveTowardsDirection = heldByPlayer.transform.position - transform.position;
            float distanceFactor = (moveTowardsDirection.magnitude / 50f) * MagnetToPlayer;
            transform.position += (Vector3)moveTowardsDirection.normalized * heldSpeed * Time.fixedDeltaTime * distanceFactor;
        }
        else if (transform.position != (Vector3)homePosition)
        {
            dropReturnTimer += Time.fixedDeltaTime;
            if (dropReturnTimer >= dropReturnTime)
            {
                ResetFlag();
                dropReturnTimer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (returningHome)
            return;

        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (playerController.PlayerTeam != Team && !held)
            {
                heldByPlayer = playerController;
                held = true;
                heldByPlayer.SetCarriedFlag(this);

                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayFlagSnatched();
            }
        }

        if (other.TryGetComponent(out Goal goal))
        {
            if (goal.Team != Team)
            {
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayFlagCaptured();

                GameManager.ScoreGoal(goal.Team);
                ResetFlag();
            }
        }
    }
}