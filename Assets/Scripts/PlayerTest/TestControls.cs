using UnityEngine;

public class TestControls : MonoBehaviour
{
    public float acceleration = 3f;
    public float maxSpeed = 100f;
    public float brakeForce = 30f;
    public float dragFactor = 3f;
    public float baseMaxTurnAngle = 45f;
    public float minTurnAngle = 10f;
    public float k = 0.33f;
    public float maxTurnSpeed = 30f;
    public Transform[] wheels;
    public Transform frontWheel;
    public Transform backWheel;
    private float length;

    private float currentSpeed = 0f;
    private float currentAngle = 0f;

    private void Start()
    {
        var wheelScale = frontWheel.parent.localScale.z * transform.localScale.z;
        var scaledDistanceBetweenWheels = frontWheel.localPosition.z - backWheel.localPosition.z;
        var wheelsDistance = scaledDistanceBetweenWheels * wheelScale;
        length = wheelsDistance;
    }

    private float DeltaTime(float input)
    {
        //return 9f;
        return input * Time.deltaTime * 50 ;
    }

    void FixedUpdate()
    {



        //transform.localRotation = Quaternion.Euler(0, 0, currentAngle * Time.deltaTime);
        //transform.position += moveVector;

        //transform.rotation = anotherVector;
        var transformSpeed = 0.5f * currentSpeed;
        var transformVector = Vector3.forward * transformSpeed;
        //transform.Translate(transformVector * 0.02f);
        //transform.Translate(transformVector * Time.fixedDeltaTime);

        transform.Translate(transformVector * Time.deltaTime);

        //transform.Translate(transformVector);

        var transformAngle = currentAngle * transformSpeed;
        var transformAngleVector = Vector3.up * transformAngle / length;
        //var transformAngleVector = Vector3.up * transformAngle;

        //transform.Rotate(transformAngleVector);
        //transform.Rotate(transformAngleVector * Time.fixedDeltaTime);

        transform.Rotate(transformAngleVector * Time.deltaTime);
        //transform.Rotate(transformAngleVector * 0.02f);


        //transform.localEulerAngles = anotherVector;
        //transform.localRotation = new Vector3(0, currentAngle, 0);
        //transform.position += moveVector * Time.deltaTime;
        //transform.SetLocalPositionAndRotation(moveVector, anotherVector); 

        //var something = (Vector3.forward * currentSpeed * Time.deltaTime).z;

        //transform.Rotate(0, currentAngle * Time.deltaTime * currentSpeed * 0.5f, 0);
        //transform.RotateAround(0, currentAngle * Time.deltaTime * currentSpeed * 0.5f, 0);
        //transform.Rotate(Vector3.up * currentAngle * currentSpeed * 0.5f * Time.deltaTime);

        //transform.rotation = 
        //transform.TransformDirection(Vector3.up * currentAngle * Time.deltaTime);
        //transform.Translate(new Vector3(0,0,1+currentSpeed * Time.deltaTime));

    }

    // Method to handle acceleration
    public void HandleAcceleration(float input)
    {
        //input = Time.deltaTime;

        float clampedInput = Mathf.Clamp(input, -1f, 1f) * acceleration;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        currentSpeed += DeltaTime( clampedInput);
        Debug.Log("currentSpeed: " + currentSpeed);

        // Apply drag when there's minimal acceleration input
        if (Mathf.Abs(input) < 0.1f)
        {
            float drag = dragFactor * 0.1f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, drag);
        }
    }

    // Method to handle steering
    public void HandleSteering(float input)
    {
        //input *= 50*Time.deltaTime;
        //float dynamicMaxTurnAngle = minTurnAngle + (baseMaxTurnAngle - minTurnAngle) * (1 - Mathf.Abs(currentSpeed) / maxSpeed);
        float dynamicMaxTurnAngle = 45f;
        float speedAppliedConst = 1.04f;
        float speedAngleImpact = (-1 / 5) + 0.2f * Mathf.Pow(speedAppliedConst, Mathf.Abs(currentSpeed));

        //float speedAngleImpact = 0.04 * currentSpeed;
        //float 
        float desiredAngle = input * dynamicMaxTurnAngle;
        float difference = Mathf.Abs(desiredAngle - currentAngle);
        float currentTurningSpeed = (k * difference);
        //float currentTurningSpeed = (k * difference);



        //currentAngle = Mathf.MoveTowards(currentAngle, desiredAngle, appliedSteeringWeight);
        //currentAngle = Mathf.MoveTowards(currentAngle, desiredAngle * Time.deltaTime, currentTurningSpeed * Time.deltaTime);
        currentAngle = Mathf.MoveTowards(currentAngle, desiredAngle, DeltaTime(currentTurningSpeed));
        //currentAngle = Mathf.MoveTowards(currentAngle, desiredAngle, appliedSteeringWeight * Time.deltaTime);


        //float currentTurningSpeed = k * difference;
        //currentTurningSpeed = Mathf.Clamp(currentTurningSpeed, -maxTurnSpeed, maxTurnSpeed);
        //currentAngle += currentSpeed * 0.1f;
        // currentAngle += currentTurningSpeed * Time.deltaTime;
        //currentAngle += appliedSteeringWeight * Time.deltaTime;
        float constAppliedSteering = 1.03f;
        float appliedSteeringWeight = 1f * Mathf.Pow(constAppliedSteering, Mathf.Abs(currentAngle - desiredAngle));
        //currentAngle += Time.deltaTime;

        foreach (Transform wheel in wheels)
        {
            wheel.localRotation = Quaternion.Euler(0, currentAngle, -90);
        }


    }

    // Method to handle braking
    public void HandleBraking(float input)
    {
        //input *= 50 * Time.deltaTime;

        float brake = brakeForce * input * Time.deltaTime;
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brake);
    }

    public void ResetControls()
    {
        currentSpeed = 0f;
        currentAngle = 0f;
    }


}