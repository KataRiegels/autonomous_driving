using UnityEngine;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    [Header("Agent Parameters")]
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Checkpoint[] checkpoints;
    private TestControls carControls;  // Reference to the TestControls script
    private Coroutine episodeTimer;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        carControls = GetComponent<TestControls>();  // Get the TestControls component attached to the same GameObject
    }

    public override void OnEpisodeBegin()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        carControls.ResetControls();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.ActivateCheckpoints();
        }
        // Start the timer coroutine and keep its reference
        episodeTimer = StartCoroutine(EndEpisodeAfterDelay(10.0f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    //    sensor.AddObservation(transform.localPosition);
    //    sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Extract the actions
        float accelerationInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float steeringInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        float brakingInput = Mathf.Clamp(actions.ContinuousActions[2], 0f, 1f);

        // Log the input values
        //Debug.Log("Acceleration Input: " + accelerationInput);
        //Debug.Log("Steering Input: " + steeringInput);
        //Debug.Log("Braking Input: " + brakingInput);

        // Call the TestControls methods based on the received actions
        carControls.HandleAcceleration(accelerationInput);
        carControls.HandleSteering(steeringInput);
        carControls.HandleBraking(brakingInput);
        // Speed reward test
        float speedRewardFactor = 0.01f;  // This factor determines how much reward is given based on speed.
        float speedReward = carControls.currentSpeed * speedRewardFactor;
        AddReward(speedReward);
        //Debug.Log("Speed Reward" + speedReward);

        // Punish it for being passive.
        AddReward(-0.0001f);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");  // Acceleration
        continuousActions[1] = Input.GetAxisRaw("Horizontal");  // Steering
        continuousActions[2] = Input.GetKey(KeyCode.Space) ? 1f : 0f;  // Braking
    }

    // Collision logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GreenWall>(out GreenWall greenWall)) 
        {
            AddReward(+30f);
            Debug.Log("Completed task!");
            //EndEpisode();
            EndEpisodeAndStopTimer();
        }
        else if (other.TryGetComponent<RedWall>(out RedWall redwall)) 
        {
            AddReward(-10f);
            Debug.Log("Boundaries!");
            //EndEpisode();
            EndEpisodeAndStopTimer();
        }
        else if (other.TryGetComponent<CPReward>(out CPReward cpReward)) 
        {
            AddReward(+5f);
            Debug.Log("CP Green!");
        }
        else if (other.TryGetComponent<CPPunish>(out CPPunish cpPunish)) 
        {
            AddReward(-8f);
            Debug.Log("CP Red!");
        }
    }
    private void EndEpisodeAndStopTimer()
    {
    // Stop the timer coroutine if it's running
        if (episodeTimer != null)
        {
            StopCoroutine(episodeTimer);
        }
        EndEpisode();
    }
    private IEnumerator EndEpisodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndEpisode();
    }
}
