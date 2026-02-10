using UnityEngine;

public class Activatable : MonoBehaviour
{
    [SerializeField] float activeTime = 0.1f; // Time to stay active after being activated
    [SerializeField] public bool holdable = true;
    [SerializeField] public bool alwaysOn = false;

    public bool active { get; private set; } = false;
    float activeTimer = 0f; // Timer to track how long the object has been active

    public void UpdateActivationTimer()
    {
        if (alwaysOn)
        {
            active = true;
            return;
        }
        if (active)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0f)
            {
                active = false;
                activeTimer = 0f;
            }
        }
    }

    public void Activate()
    {
        active = true;
        activeTimer = activeTime;
        ActivationEffect();
    }

    //Override this method with effects that should happen when the object is activated.
    public virtual void ActivationEffect()
    { }
}