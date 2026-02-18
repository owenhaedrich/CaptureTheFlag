using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Activatable[] connectedActivatables;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    SpriteRenderer spriteRenderer;
    bool occupied = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            if (!occupied)
            {
                occupied = true;
                if (SoundManager.Instance != null) SoundManager.Instance.PlayStepOn();

                if (spriteRenderer != null && activeSprite != null)
                {
                    spriteRenderer.sprite = activeSprite;
                }
                
                foreach (var activatable in connectedActivatables)
                {
                    if (activatable != null) activatable.Activate();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            if (occupied)
            {
                occupied = false;
                if (SoundManager.Instance != null) SoundManager.Instance.PlayStepOff();

                if (spriteRenderer != null && inactiveSprite != null)
                {
                    spriteRenderer.sprite = inactiveSprite;
                }

                foreach (var activatable in connectedActivatables)
                {
                    if (activatable != null) activatable.Deactivate();
                }
            }
        }
    }
}
