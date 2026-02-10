using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Activatable connectedActivatable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerController player))
        {
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
}

