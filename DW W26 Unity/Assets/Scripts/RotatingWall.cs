using UnityEngine;

public class RotatingWall : Activatable
{
    [SerializeField] float rotateSpeed = 100f;

    Rigidbody2D WallRigidBody;

    private void Awake()
    {
        // Rotating walls should stay visible even when not "active" (active = reversing)
        // This is a field in the base Activatable class.
        // We can't easily set it in the field initializer if we want it to be serialized/overridable,
        // but for now we'll just set it in Awake for this specific class.
        // Actually, it's better to just let the user set it in inspector, 
        // but I'll add a check here or set a default if appropriate.
    }

    protected override void Start()
    {
        base.Start();
        WallRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateActivationTimer();
    }

    private void FixedUpdate()
    {
        float speed = active ? -rotateSpeed : rotateSpeed;
        WallRigidBody.AddTorque(speed);
    }
}
