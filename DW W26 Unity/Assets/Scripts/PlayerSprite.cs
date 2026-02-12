using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] Sprite[] playerSpritesInOrder; //Six player sprites in order
    [SerializeField] float maxRotationDegrees = 10;
    [SerializeField] float walkWobbleTime = 0.5f;
    [SerializeField] float maxScaleWobble = 0.05f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxLeanAngle = 5f;
    [SerializeField] float maxLeadOffset = 0.2f;

    PlayerController parentPlayerController;
    Rigidbody2D parentRigidbody2D;
    Vector3 baseScale;
    Vector3 baseLocalPosition;
    float wobblePhase;
    float currentIntensity;
    float intensityVelocity;
    float currentLean;
    float leanVelocity;
    float currentLead;
    float leadVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentRigidbody2D = GetComponentInParent<Rigidbody2D>();
        parentPlayerController = GetComponentInParent<PlayerController>();
        baseScale = transform.localScale;
        baseLocalPosition = transform.localPosition;

        AssignSpriteByPlayerNumber();
    }

    void AssignSpriteByPlayerNumber()
    {
        if (parentPlayerController == null) return;
        if (playerSpritesInOrder == null || playerSpritesInOrder.Length == 0) return;

        // Try to get SpriteRenderer on this object
        if (TryGetComponent(out SpriteRenderer sr))
        {
            int index = parentPlayerController.PlayerNumber - 1;
            
            // If index is within bounds, use it.
            if (index >= 0 && index < playerSpritesInOrder.Length)
            {
                sr.sprite = playerSpritesInOrder[index];
            }
            // If it's 1-based (1-6) and we have 6 sprites, adjust to 0-5
            else if (index > 0 && index <= playerSpritesInOrder.Length)
            {
                sr.sprite = playerSpritesInOrder[index - 1];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float velocityMag = parentRigidbody2D.linearVelocity.magnitude;

        // Face movement direction
        if (parentRigidbody2D != null && Mathf.Abs(parentRigidbody2D.linearVelocity.x) > 0.1f)
        {
            float lookDirection = - Mathf.Sign(parentRigidbody2D.linearVelocity.x);
            // Update baseScale x to match look direction
            baseScale.x = Mathf.Abs(baseScale.x) * lookDirection;
        }

        // Calculate intensity based on velocity (0 to 1 range)
        float targetIntensity = Mathf.Clamp01(velocityMag / 5f); 
        currentIntensity = Mathf.SmoothDamp(currentIntensity, targetIntensity, ref intensityVelocity, smoothTime);

        // Calculate Lean and Lead based on horizontal velocity
        float horizontalVel = parentRigidbody2D.linearVelocity.x;
        float targetLean = -Mathf.Clamp(horizontalVel / 5f, -1f, 1f) * maxLeanAngle;
        float targetLead = Mathf.Clamp(horizontalVel / 5f, -1f, 1f) * maxLeadOffset;

        currentLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, smoothTime);
        currentLead = Mathf.SmoothDamp(currentLead, targetLead, ref leadVelocity, smoothTime);

        // Apply Lead offset
        transform.localPosition = baseLocalPosition + new Vector3(currentLead, 0, 0);

        if (currentIntensity > 0.001f)
        {
            // Update phase based on time and intensity
            wobblePhase += Time.deltaTime * (1f / walkWobbleTime);
            
            // Rotation wobble (1 cycle per walkWobbleTime) + Lean
            float wobbleRotation = Mathf.Sin(wobblePhase * Mathf.PI * 2) * maxRotationDegrees * currentIntensity;
            transform.localRotation = Quaternion.Euler(0, 0, wobbleRotation + currentLean);

            // Squash and stretch (2 cycles per walkWobbleTime for a more natural step feel)
            float scaleY = 1f + Mathf.Sin(wobblePhase * Mathf.PI * 2 * 2) * maxScaleWobble * currentIntensity;
            float scaleX = 1f / scaleY; // Keep area consistent
            transform.localScale = new Vector3(baseScale.x * scaleX, baseScale.y * scaleY, baseScale.z);
        }
        else
        {
            // Reset to base state when stopped
            transform.localRotation = Quaternion.Euler(0, 0, currentLean);
            transform.localScale = baseScale;
            wobblePhase = 0;
        }
    }
}
