using UnityEngine;
using System.Collections.Generic;

public class ActivationGroup : Activatable
{
    private List<Activatable> childActivatables = new List<Activatable>();

    protected override void Start()
    {
        // Get all Activatable components in children, excluding the group itself
        Activatable[] allActivatables = GetComponentsInChildren<Activatable>();
        foreach (var activatable in allActivatables)
        {
            if (activatable != this)
            {
                childActivatables.Add(activatable);
            }
        }

        // Call base Start to ensure SpriteRenderer and initial state are handled
        base.Start();
    }

    private void Update()
    {
        // Support timer-based activation for the group
        UpdateActivationTimer();
    }

    public override void ActivationEffect()
    {
        foreach (var activatable in childActivatables)
        {
            if (activatable != null)
            {
                activatable.Activate();
            }
        }
    }

    public override void DeactivationEffect()
    {
        foreach (var activatable in childActivatables)
        {
            if (activatable != null)
            {
                activatable.Deactivate();
            }
        }
    }
}
