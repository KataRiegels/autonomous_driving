using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float rayLength = 5.0f; // Length of the ray
    private Vector3 targetPosition;
    private LayerMask carLayerMask; // Layer mask for detecting cars

    private void Start()
    {
        carLayerMask = LayerMask.GetMask("Car"); // Initialize the car layer mask
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    private void FixedUpdate()
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Rotate the pedestrian to face the target position
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }

        // Raycast to check for cars in front of the pedestrian
        RaycastHit hit;
        bool isCarDetected = Physics.Raycast(transform.position, transform.forward, out hit, rayLength, carLayerMask);
        Debug.DrawRay(transform.position, transform.forward * rayLength, isCarDetected ? Color.red : Color.green);

        if (isCarDetected)
        {
            // If a car is detected, do not move
            return;
        }

        // Move towards the target position if no car is detected
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the pedestrian has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // Adjust threshold as needed
        {
            Destroy(gameObject); // Destroy the pedestrian object
        }
    }
}
