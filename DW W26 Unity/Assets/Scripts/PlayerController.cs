using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

    [SerializeField] float RespawnDelay = 0.75f;
    [SerializeField] float MidlineX = 0f;

    public Flag CarriedFlag { get; private set; }
    public Collider2D PlayerCollider { get; private set; }

    Transform SpawnPoint;
    bool DoBoost;
    float BoostCooldownTimer = 0f;

    PlayerInput PlayerInput;
    InputAction InputActionMove;
    InputAction InputActionBoost;

    bool IsDead = false;

    private void Awake()
    {
        if (Rigidbody2D == null) Rigidbody2D = GetComponent<Rigidbody2D>();
        if (SpriteRenderer == null) SpriteRenderer = GetComponent<SpriteRenderer>();
        if (PlayerCollider == null) PlayerCollider = GetComponent<Collider2D>();
    }

    public void AssignColor(Color color)
    {
        PlayerColor = color;
        if (SpriteRenderer != null) SpriteRenderer.color = color;
    }

    public void AssignTeam(Team team)
    {
        PlayerTeam = team;
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }

    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        PlayerInput = playerInput;
        InputActionMove = playerInput.actions.FindAction("Player/Move");
        InputActionBoost = playerInput.actions.FindAction("Player/Jump");
    }

    public void AssignPlayerNumber(int playerNumber)
    {
        PlayerNumber = playerNumber;
    }

    public void SetCarriedFlag(Flag flag)
    {
        CarriedFlag = flag;
    }

    public void ClearCarriedFlag(Flag flag)
    {
        if (CarriedFlag == flag) CarriedFlag = null;
    }

    public bool PressedBoostThisFrame()
    {
        return InputActionBoost != null && InputActionBoost.WasPressedThisFrame();
    }

    public bool IsInEnemyEnd()
    {
        if (PlayerTeam == Team.Red) return transform.position.x > MidlineX;
        return transform.position.x < MidlineX;
    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;

        if (CarriedFlag != null)
        {
            CarriedFlag.Drop();
            CarriedFlag = null;
        }

        if (PlayerCollider != null) PlayerCollider.enabled = false;
        if (SpriteRenderer != null) SpriteRenderer.enabled = false;

        if (Rigidbody2D != null)
        {
            Rigidbody2D.linearVelocity = Vector2.zero;
            Rigidbody2D.angularVelocity = 0f;
            Rigidbody2D.simulated = false;
        }

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(RespawnDelay);
        Respawn();
    }

    public void Respawn()
    {
        if (SpawnPoint != null) transform.position = SpawnPoint.position;

        if (Rigidbody2D != null)
        {
            Rigidbody2D.simulated = true;
            Rigidbody2D.linearVelocity = Vector2.zero;
            Rigidbody2D.angularVelocity = 0f;
        }

        if (SpriteRenderer != null) SpriteRenderer.enabled = true;
        if (PlayerCollider != null) PlayerCollider.enabled = true;

        IsDead = false;
    }

    public void Update()
    {
        if (BoostCooldownTimer > 0)
        {
            BoostCooldownTimer -= Time.deltaTime;
            if (BoostCooldownTimer < 0) BoostCooldownTimer = 0;
        }
        else
        {
            if (InputActionBoost != null && InputActionBoost.WasPressedThisFrame())
                DoBoost = true;
        }
    }

    void FixedUpdate()
    {
        if (Rigidbody2D == null || InputActionMove == null) return;

        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();
        Rigidbody2D.AddForce(moveValue * MoveSpeed, ForceMode2D.Force);

        if (DoBoost && BoostCooldownTimer == 0)
        {
            Rigidbody2D.AddForce(BoostForce * moveValue, ForceMode2D.Impulse);

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayDash();

            DoBoost = false;
            BoostCooldownTimer = BoostCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerController otherPlayer)) return;

        bool inBoostWindow = BoostCooldownTimer > BoostCooldown - BoostAttackTime;
        if (!inBoostWindow) return;

        Vector2 direction = (otherPlayer.transform.position - transform.position).normalized;
        if (otherPlayer.Rigidbody2D != null)
            otherPlayer.Rigidbody2D.AddForce(direction * HitForce, ForceMode2D.Impulse);

        if (otherPlayer.IsInEnemyEnd() || otherPlayer.CarriedFlag != null)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayDashKill();

            otherPlayer.Die();
        }
    }

    private void OnValidate()
    {
        Reset();
    }

    private void Reset()
    {
        if (Rigidbody2D == null) Rigidbody2D = GetComponent<Rigidbody2D>();
        if (SpriteRenderer == null) SpriteRenderer = GetComponent<SpriteRenderer>();
        if (PlayerCollider == null) PlayerCollider = GetComponent<Collider2D>();
    }
}
