using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TestControls : MonoBehaviour
{

    public float acceleration = 10f;
    public float maxSpeed = 60f;
    public float steeringSensitivity = 6f;
    public float brakeForce = 50f;
    public float dragFactor = 3f;
    
    // Make currentSpeed a public property

    public float maxAngle = 60f;
    public float currentSpeed { get; private set; }
    public float currentAngle {get; private set;}
    public float currentAngleChange {get; private set;}
    public float currentDesiredAngle {get; private set;}

    void FixedUpdate()
    {
        // Apply drag (simulated friction) when there's minimal acceleration input
        if (Mathf.Abs(currentSpeed) < 0.1f)
        {
            float drag = dragFactor * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed*Time.deltaTime, 0, drag);
        }

        // Update the car's position
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
    public void ApplyBraking(float input)
    {
        float brakePedal = input;

        float speedToSub = 0;
        float brakeConst = brakeForce;
        speedToSub = brakeConst * brakePedal/currentSpeed;
        Debug.Log(speedToSub);
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, speedToSub);    
        //currentSpeed = speedToSub;

    }

    public void ApplyAcceleration(float input)
    {

        float gasPedal = input;
        float speedToAdd = 0;

        // If the car tries to reverse when the car drives forward, and vice versa
        if (currentSpeed * input >= 0){
            float accelerationConst = acceleration;
            speedToAdd = accelerationConst * gasPedal;
        }

        currentSpeed += speedToAdd; 
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);



/*
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
                // float brake = brakeForce * Time.deltaTime;
                // currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
                currentSpeed += input * acceleration * Time.deltaTime;
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
*/
    }

    public void CalculateSteering(float input)
    {

        float constSpeedInfluence = 1; 
        // float maxAngleWithSpeed =  maxAngle * (constSpeedInfluence * Mathf.Exp(-Mathf.Abs(currentSpeed)));
        // float desiredAngleInput = input * maxAngle * constSpeedInfluence * Mathf.Exp(Mathf.Abs(currentSpeed));
        float maxAngleWithSpeed =  maxAngle * (math.pow(constSpeedInfluence, -Mathf.Abs(currentSpeed)*constSpeedInfluence ));
        //float desiredAngleInput = input * maxAngleWithSpeed;

        // How much the agent attempts to change the steering angle
        float desiredAngleInput = Mathf.Clamp(input, -1f, 1f) * 60;
        currentDesiredAngle = desiredAngleInput;
        Debug.Log("DesAng:" + desiredAngleInput);
        Debug.Log("Current: " + currentAngle);
        float constAppliedSteering = 1.03f;
        //Debug.Log("Mathf.Abs(currentAngle - desiredAngleInput): " + (currentAngle - desiredAngleInput));
        //float appliedSteeringWeight = 0.1f* Mathf.Exp(constAppliedSteering * Mathf.Abs(currentAngle - desiredAngleInput));
        float appliedSteeringWeight = 0.5f * math.pow(constAppliedSteering, Mathf.Abs(currentAngle - desiredAngleInput));
        //Debug.Log("appliedSteeringWeight: " + appliedSteeringWeight);
        currentAngleChange = appliedSteeringWeight;
        Debug.Log("CurrentAngChange: " + currentAngleChange);

    }


    public void ApplySteering(float input)
    {

        
        // effect from the car's speed
        // float speedInfluence = constSpeedInfluence * Mathf.Exp(Mathf.Abs(currentSpeed));
        // float deltaAngleWeight = appliedSteeringWeight - speedInfluence;

        // the change to apply
        //float deltaAngleWeight = appliedSteeringWeight;
        float deltaAngleWeight = currentAngleChange;
        float desiredAngleInput = currentDesiredAngle;

        // the new angle should be 
        float newAngle = Mathf.MoveTowards(currentAngle, desiredAngleInput , deltaAngleWeight);
        currentAngle = newAngle;
        transform.Rotate(0, currentAngle * Time.deltaTime, 0);


/*


        float steeringFactor = steeringSensitivity * Mathf.Exp(-Mathf.Abs(currentSpeed) / maxSpeed);
        float steeringAngle = input * steeringFactor;
        transform.Rotate(0, steeringAngle * currentSpeed * Time.deltaTime, 0);
*/
    }

    public void ResetSpeed()
    {
        currentSpeed = 0;
    }
}
