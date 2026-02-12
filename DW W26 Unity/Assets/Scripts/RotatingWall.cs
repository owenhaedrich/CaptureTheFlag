using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 100f;

    Rigidbody2D WallRigidBody;

    private void Start()
    {
        WallRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        WallRigidBody.AddTorque(rotateSpeed);
    }
}
