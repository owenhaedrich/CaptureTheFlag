using UnityEngine;

public class Flag : MonoBehaviour
{
    [field: SerializeField] public Team PlayerTeam { get; private set; }
    [field: SerializeField] public float magnetToPlayer { get; private set; }

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
            float distanceFactor = (moveTowardsDirection.magnitude / 50f) * magnetToPlayer;
            transform.position += (Vector3)moveTowardsDirection.normalized * heldSpeed * Time.fixedDeltaTime * distanceFactor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Flag {name} collided with {other.name}.");  
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (playerController.PlayerTeam != PlayerTeam && !held)
            {
                heldByPlayer = playerController;
                held = true;
            }
        }
    }
}
