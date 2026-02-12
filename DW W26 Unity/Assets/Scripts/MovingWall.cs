using UnityEngine;
using System.Collections.Generic;

public class MovingWall : MonoBehaviour
{
    [SerializeField] float moveSpeed = 100f;

    List<Vector3> Positions = new List<Vector3>();
    Rigidbody2D WallRigidBody;

    float closeEnoughDistance = 1.0f;
    int targetPositionIndex = 0;

    private void Start()
    {
        WallRigidBody = GetComponent<Rigidbody2D>();

        //Get positions from the transforms of the children
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childTransforms)
        {
            Positions.Add(childTransform.position);
        }
    }

    private void FixedUpdate()
    {
        //Move between the positions at the moveSpeed
        Vector2 vectorToNextPosition = Positions[targetPositionIndex] - transform.position;
        Vector2 directionToNextPosition = vectorToNextPosition.normalized;
        WallRigidBody.AddForce(directionToNextPosition * moveSpeed);

        //Update index when close to the target position
        Debug.Log(vectorToNextPosition.magnitude);
        if (vectorToNextPosition.magnitude < closeEnoughDistance)
        {
            targetPositionIndex++;
            if (targetPositionIndex >= Positions.Count) targetPositionIndex = 0;
        }
    }
}
