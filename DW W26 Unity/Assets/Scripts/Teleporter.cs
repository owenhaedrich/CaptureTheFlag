using UnityEngine;

public class Teleporter : Activatable
{    
    [SerializeField] Teleporter destinationTeleporter;
    [SerializeField] float TeleportCooldown = 0.5f;

    Collider2D teleporterCollider;
    float cooldownTimer = 0f;

    private void Awake()
    {
        if (teleporterCollider == null) teleporterCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        UpdateActivationTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && cooldownTimer <= 0f)
        {
            TeleportPlayers();
        }
    }

    public override void ActivationEffect()
    {
        TeleportPlayers();
    }

    void TeleportPlayers()
    {
        foreach (Collider2D collider in Physics2D.OverlapBoxAll(teleporterCollider.bounds.center, teleporterCollider.bounds.size, 0f))
        {
            if (collider.gameObject.TryGetComponent(out PlayerController playerController))
            {
                if (cooldownTimer <= 0f)
                {
                    destinationTeleporter.StartCooldown();
                    playerController.transform.position = destinationTeleporter.transform.position;
                    if (playerController.CarriedFlag != null)
                    {
                        playerController.CarriedFlag.transform.position = destinationTeleporter.transform.position;
                    }
                }
            }
        }
    }

    // Prevent teleporting again immediately after teleporting
    void StartCooldown()
    {
        cooldownTimer = TeleportCooldown;
    }
}
