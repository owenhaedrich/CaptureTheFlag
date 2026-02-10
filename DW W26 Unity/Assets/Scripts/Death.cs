using UnityEngine;

public class Death : Activatable
{
    Collider2D deathCollider;

    private void Awake()
    {
        deathCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        UpdateActivationTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (active)
        {
            KillPlayers();
        }
    }

    // Kill players that are already inside the death zone when it becomes active.
    void KillPlayers()
    {
        var colliders = Physics2D.OverlapBoxAll(deathCollider.bounds.center, deathCollider.bounds.size, 0);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerController player))
                player.Die();
        }
    }

    public override void ActivationEffect()
    {
        KillPlayers();
    }
}
