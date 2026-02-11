using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    float lerpSpeed = 5f; // Speed at which the indicator moves to follow the player's y position.

    float fadeDelay = 0.5f; // Time in seconds before the indicator starts to fade out when the player stops moving while off-screen.
    float fadeDuration = 1f; // Time in seconds for the indicator to fully fade out after the fade delay.

    float fadeDelayTimer = 0f; // Timer to track how long the player has been stationary while off-screen before starting to fade out.
    float fadeTimer = 0f; // Timer to fully fade out.

    public PlayerController player;

    public Collider2D offscreenArea;

    // Track the player's y position and update the indicator's position accordingly.
    private void Update()
    {
        if (player != null)
        {
            Vector2 newPosition = new Vector2(transform.position.x, player.transform.position.y);
            transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);

            // Get smaller as the player gets further away from the indicator
            float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
            float scale = 1 - ((distance / (offscreenArea.bounds.size.x)) * 0.3f);
            transform.localScale = new Vector3(scale, scale, 1f);

            // Handle fading out the indicator if the player is stationary while off-screen.
            if (player.Rigidbody2D.linearVelocity.magnitude < 0.1f)
            {
                fadeDelayTimer += Time.deltaTime;
                if (fadeDelayTimer >= fadeDelay)
                {
                    fadeTimer += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
                    Color color = GetComponent<SpriteRenderer>().color;
                    color.a = alpha;
                    GetComponent<SpriteRenderer>().color = color;
                }
            }
            else
            {
                // Reset timers and ensure the indicator is fully visible if the player starts moving again.
                fadeDelayTimer = 0f;
                fadeTimer = 0f;
                Color color = GetComponent<SpriteRenderer>().color;
                color.a = 1f;
                GetComponent<SpriteRenderer>().color = color;
            }

            // Destroy the indicator if the player is not colliding with the offscreen area anymore (i.e. they are back on-screen).
            if (!offscreenArea.IsTouching(player.PlayerCollider))
            {
                Destroy(gameObject);
            }

        }
    }

}
