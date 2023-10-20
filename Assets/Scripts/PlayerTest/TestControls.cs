using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControls : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed = 60f;
    public float steeringSensitivity = 6f;
    public float brakeForce = 50f;
    public float dragFactor = 3f;

    // NEW
    public float maxAngle = 60f;
    public float currentAngle {get; private set;}
    public float currentAngleChange {get; private set;}

    public float constSpeedInfluence = 2/(Mathf.Exp(100));

    // NEW END
    
    // Make currentSpeed a public property
    public float currentSpeed { get; private set; }

    void FixedUpdate()
    {
        // Apply drag (simulated friction) when there's minimal acceleration input
        if (Mathf.Abs(currentSpeed) < 0.1f)
        {
            float drag = dragFactor * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, drag);
        }

        // Update the car's position
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    public void ApplyAcceleration(float input)
    {
        // Accelerating forward
        if (input > 0)
        {
            currentSpeed += input * acceleration * Time.deltaTime;
        }
        // Braking or reversing
        else if (input < 0)
        {
            // If the car is moving forward, apply brakes
            if (currentSpeed > 0)
            {
                float brake = brakeForce * Time.deltaTime;
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
            }
            // If the car is already moving backward, accelerate in reverse
            else
            {
                currentSpeed += input * acceleration * Time.deltaTime;
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
    }

    public void ApplySteering(float input)
    {
        //float steeringFactor = steeringSensitivity * Mathf.Exp(-Mathf.Abs(currentSpeed) / maxSpeed);
        //float steeringAngle = input * steeringFactor;
        //transform.Rotate(0, steeringAngle * currentSpeed * Time.deltaTime, 0);
        float maxAngleWithSpeed =  maxAngle / (constSpeedInfluence * Mathf.Exp(Mathf.Abs(currentSpeed)));
        // float desiredAngleInput = input * maxAngle * constSpeedInfluence * Mathf.Exp(Mathf.Abs(currentSpeed));
        float desiredAngleInput = input * maxAngleWithSpeed;
        Debug.Log("Desired Angle: " + desiredAngleInput + "I: " + input);
        // How much the agent attempts to change the steering angle
        float constAppliedSteering = 1;
        float appliedSteeringWeight = constAppliedSteering * Mathf.Exp(Mathf.Abs(currentAngle - desiredAngleInput));

        // effect from the car's speed
        // float speedInfluence = constSpeedInfluence * Mathf.Exp(Mathf.Abs(currentSpeed));
        // float deltaAngleWeight = appliedSteeringWeight - speedInfluence;

        // the change to apply
        float deltaAngleWeight = appliedSteeringWeight;

        // the new angle should be 
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngleInput , deltaAngleWeight); 
        currentAngle = newAngle;
        transform.Rotate(0, newAngle, 0);
    }

    public void ResetSpeed()
    {
        currentSpeed = 0;
    }
}
