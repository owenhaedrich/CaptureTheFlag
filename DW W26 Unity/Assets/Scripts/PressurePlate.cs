using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Activatable connectedActivatable;

    bool occupied = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            if (!occupied)
            {
                occupied = true;
                if (SoundManager.Instance != null) SoundManager.Instance.PlayStepOn();
            }

            connectedActivatable.Activate();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player) && connectedActivatable.holdable)
        {
            connectedActivatable.Activate();
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
            }
        }
    }
}
