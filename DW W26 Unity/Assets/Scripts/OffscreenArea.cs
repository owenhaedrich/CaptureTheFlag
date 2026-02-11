using UnityEngine;

public class OffscreenArea : MonoBehaviour
{
    [SerializeField] GameObject offScreenIndicator;

    Collider2D offscreenArea;

    float offset = 0.5f; // Multiplier to determine how far off-screen the indicator should be placed.

    private void Awake()
    {
        offscreenArea = GetComponent<Collider2D>();
        if (offscreenArea == null)
        {
            Debug.LogError("OffscreenArea requires a Collider2D component.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            // Create an off-screen indicator for the player if they don't already have one.
            // Spawn at the edge the player is exiting from, with an offset to ensure it's fully off-screen.
            Vector2 indicatorSpawnPosition = new Vector2(transform.position.x + (transform.localScale.x / 2 + offset) * Mathf.Sign(player.transform.position.x - transform.position.x), player.transform.position.y);

            // Tint the off-screen indicator with the player's color.
            offScreenIndicator.GetComponent<SpriteRenderer>().color = player.PlayerColor;
            offScreenIndicator.GetComponent<SpriteRenderer>().flipX = Mathf.Sign(player.transform.position.x - transform.position.x) < 0;
            offScreenIndicator.GetComponent<OffscreenIndicator>().player = player;
            offScreenIndicator.GetComponent<OffscreenIndicator>().offscreenArea = offscreenArea;
            Instantiate(offScreenIndicator, indicatorSpawnPosition, Quaternion.identity);
        }
    }
}
