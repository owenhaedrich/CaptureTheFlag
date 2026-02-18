using UnityEngine;

public class Activatable : MonoBehaviour
{
    [SerializeField] float activeTime = 0.1f; // Time to stay active after being activated
    [SerializeField] public bool holdable = true;
    [SerializeField] public bool alwaysOn = false;
    [SerializeField] bool hideWhenInactive = false;
    [SerializeField] bool inverted = false;

    [Header("Sprite Settings")]
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    [Header("Color Settings")]
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color inactiveColor = Color.white;

    public bool active { get; private set; } = false;
    float activeTimer = 0f; // Timer to track how long the object has been active
    private int currentTriggers = 0;
    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateState();
        UpdateAppearance();
    }

    public void UpdateActivationTimer()
    {
        bool shouldBeActive = ShouldBeActiveAccordingToTriggers();

        if (alwaysOn)
        {
            active = true;
        }
        else if (active && !shouldBeActive) // Only countdown if currently active but the triggers say it should be off
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0f)
            {
                SetState(false);
                activeTimer = 0f;
            }
        }
        else if (shouldBeActive && !active) // Pulse activation for cases where timer might have turned it off but triggers are still active (unlikely with current flow but safer)
        {
             UpdateState();
        }

        UpdateAppearance();
    }

    public void Activate()
    {
        currentTriggers++;
        UpdateState();
    }

    public void Deactivate()
    {
        currentTriggers = Mathf.Max(0, currentTriggers - 1);
        UpdateState();
    }

    private void UpdateState()
    {
        bool shouldBeActive = ShouldBeActiveAccordingToTriggers();
        
        if (shouldBeActive && !active)
        {
            active = true;
            activeTimer = activeTime;
            ActivationEffect();
        }

        UpdateAppearance();
    }

    private bool ShouldBeActiveAccordingToTriggers()
    {
        return alwaysOn || (currentTriggers % 2 != (inverted ? 1 : 0));
    }

    private void SetState(bool newState)
    {
        if (active == newState) return;
        active = newState;
        if (active) ActivationEffect();
        else DeactivationEffect();

        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (spriteRenderer == null) return;

        if (active)
        {
            if (activeSprite != null) spriteRenderer.sprite = activeSprite;
            spriteRenderer.color = activeColor;
        }
        else
        {
            if (inactiveSprite != null) spriteRenderer.sprite = inactiveSprite;
            spriteRenderer.color = inactiveColor;
        }

        if (hideWhenInactive)
        {
            spriteRenderer.enabled = active;
        }
    }

    //Override this method with effects that should happen when the object is activated.
    public virtual void ActivationEffect()
    { }

    //Override this method with effects that should happen when the object is deactivated.
    public virtual void DeactivationEffect()
    { }
}