using UnityEngine;

public class TestControls : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed = 50f;
    public float brakeForce = 30f;
    public float dragFactor = 3f;
    public float baseMaxTurnAngle = 10f;
    public float minTurnAngle = 3f;
    public float k = 3f;
    public float maxTurnSpeed = 30f;
    public Transform[] wheels;

    public float currentSpeed = 0f;
    private float currentAngle = 0f;

    void FixedUpdate()
    {
        transform.Rotate(0, currentAngle * currentSpeed * Time.deltaTime, 0);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

    }
    
    // Method to handle acceleration
    public void HandleAcceleration(float input)
    {
        currentSpeed += input * acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        
        // Apply drag when there's minimal acceleration input
        if (Mathf.Abs(input) < 0.1f)
        {
            float drag = dragFactor * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, drag);
        }
    }

    // Method to handle steering
    public void HandleSteering(float input)
    {
        float dynamicMaxTurnAngle = minTurnAngle + (baseMaxTurnAngle - minTurnAngle) * (1 - Mathf.Abs(currentSpeed) / maxSpeed);
        float desiredAngle = input * dynamicMaxTurnAngle;
        float difference = desiredAngle - currentAngle;
        float currentTurningSpeed = k * difference;
        currentTurningSpeed = Mathf.Clamp(currentTurningSpeed, -maxTurnSpeed, maxTurnSpeed);
        currentAngle += currentTurningSpeed * Time.deltaTime;
        //Debug.Log("Desired Angle: " + desiredAngle);
        //Debug.Log("Current Angle: " + currentAngle);


        foreach (Transform wheel in wheels)
        {
            wheel.localRotation = Quaternion.Euler(0, currentAngle * 2, -90);
        }
    }

    // Method to handle braking
    public void HandleBraking(float input)
    {
        float brake = brakeForce * input * Time.deltaTime;
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
    }

    public void ResetControls()
    {
        currentSpeed = 0f;
        currentAngle = 0f;
    }


}
