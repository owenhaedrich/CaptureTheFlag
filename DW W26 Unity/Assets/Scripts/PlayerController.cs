using UnityEngine;
using UnityEngine.InputSystem;
public enum Team { Red, Blue };

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public int PlayerNumber { get; private set; }
    [field: SerializeField] public Color PlayerColor { get; private set; }
    [field: SerializeField] public Team PlayerTeam { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 150f;
    [field: SerializeField] public float BoostForce { get; private set; } = 50f;
    [field: SerializeField] public float BoostAttackTime { get; private set; } = 0.3f;
    [field: SerializeField] public float BoostCooldown { get; private set; } = 0.7f;
    [field: SerializeField] public float HitForce { get; private set; } = 70f;

    Transform SpawnPoint;
    bool DoBoost;
    float BoostCooldownTimer = 0f;

    // Player input information
    private PlayerInput PlayerInput;
    private InputAction InputActionMove;
    private InputAction InputActionBoost;

    // Assign color value on spawn from main spawner
    public void AssignColor(Color color)
    {
        // record color
        PlayerColor = color;

        // Assign to sprite renderer
        if (SpriteRenderer == null)
            Debug.Log($"Failed to set color to {name} {nameof(PlayerController)}.");
        else
            SpriteRenderer.color = color;
    }

    // Set Spawn Point
    public void SetSpawnPoint(PlayerSpawn spawn)
    {
        SpawnPoint = spawn.transform;
    }

    // Set up player input
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        // Record our player input (ie controller).
        PlayerInput = playerInput;
        // Find the references to the "Move" and "Jump" actions inside the player input's action map
        // Here I specify "Player/" but it in not required if assigning the action map in PlayerInput inspector.
        InputActionMove = playerInput.actions.FindAction($"Player/Move");
        InputActionBoost = playerInput.actions.FindAction($"Player/Jump");
    }

    // Assign player number on spawn
    public void AssignPlayerNumber(int playerNumber)
    {
        this.PlayerNumber = playerNumber;
    }

    // Runs each frame
    public void Update()
    {
        // Update and check boost cooldown timer
        if (BoostCooldownTimer > 0)
        {
            BoostCooldownTimer -= Time.deltaTime;
            if (BoostCooldownTimer < 0)
                BoostCooldownTimer = 0;
        }
        else
        {
            // Read the "Boost" action state, which is a boolean value
            if (InputActionBoost.WasPressedThisFrame())
            {
                // Buffer input becuase I'm controlling the Rigidbody through FixedUpdate
                // and checking there we can miss inputs.
                DoBoost = true;
            }
        }
    }

    // Runs each phsyics update
    void FixedUpdate()
    {
        if (Rigidbody2D == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody2D)} is null.");
            return;
        }

        // MOVE
        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();

        Vector2 moveForce = moveValue * MoveSpeed;
        // Apply fraction of force each frame
        Rigidbody2D.AddForce(moveForce, ForceMode2D.Force);

        // BOOST - review Update()
        if (DoBoost && BoostCooldownTimer == 0)
        {
            // Apply all force immediately
            Rigidbody2D.AddForce(BoostForce*moveValue, ForceMode2D.Impulse);
            DoBoost = false;
            BoostCooldownTimer = BoostCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}.");
        // Check if it's another player and if so, apply some force to them based on our movement direction and boost state.
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController otherPlayer))
        {
            bool inBoostWindow = BoostCooldownTimer > BoostCooldown - BoostAttackTime;
            // Check if withing boost attack time window
            if (inBoostWindow)
            {
                // Calculate the direction and force to apply
                Vector2 direction = (otherPlayer.transform.position - transform.position).normalized;

                // Apply the force to the other player's Rigidbody
                otherPlayer.Rigidbody2D.AddForce(direction * HitForce, ForceMode2D.Impulse);
            }
        }
    }

    // OnValidate runs after any change in the inspector for this script.
    private void OnValidate()
    {
        Reset();
    }

    // Reset runs when a script is created and when a script is reset from the inspector.
    private void Reset()
    {
        // Get if null
        if (Rigidbody2D == null)
            Rigidbody2D = GetComponent<Rigidbody2D>();
        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
