using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Activatable[] connectedActivatables;

    bool occupied = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            if (!occupied)
            {
                occupied = true;
                if (SoundManager.Instance != null) SoundManager.Instance.PlayStepOn();
                
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

                foreach (var activatable in connectedActivatables)
                {
                    if (activatable != null) activatable.Deactivate();
                }
            }
        }
    }
}
