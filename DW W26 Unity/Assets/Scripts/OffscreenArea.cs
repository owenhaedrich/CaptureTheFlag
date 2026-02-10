using UnityEngine;

public class OffscreenArea : MonoBehaviour
{
    [SerializeField] SpriteRenderer offScreenIndicatorRed;
    [SerializeField] SpriteRenderer offScreenIndicatorBlue;

    // Shows the vertical position of the player on the off-screen indicator when they are off-screen.
    // Displayed along the edge, red indicators on the left, blue indicators on the right.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            // Get the player's position relative to the off-screen area
            Vector3 playerPosition = player.transform.position;
            float offScreenAreaHeight = transform.localScale.y;

            // Calculate the normalized position (0 to 1) based on the off-screen area's height
            float normalizedPosition = Mathf.InverseLerp(-offScreenAreaHeight / 2, offScreenAreaHeight / 2, playerPosition.y);

            // Update the off-screen indicators' positions
            offScreenIndicatorRed.transform.localPosition = new Vector3(-0.5f, normalizedPosition, 0);
            offScreenIndicatorBlue.transform.localPosition = new Vector3(0.5f, normalizedPosition, 0);
        }
    }
}
