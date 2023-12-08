using UnityEngine;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent3 : Agent
{
    [Header("Agent Parameters")]
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float closestDistance;
    private float totalDistanceReward;
    [SerializeField] private Transform targetTransform;
    private TestControls carControls;  // Reference to the TestControls script
    private Coroutine episodeTimer;
    [SerializeField] private float timeLimit = 30.0f;    // Adjusting this, changes the maximum time before resetting the task 

    private void Awake()
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
        closestDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
        totalDistanceReward = 0f;
        // Start the timer coroutine and keep its reference
        episodeTimer = StartCoroutine(EndEpisodeAfterDelay(timeLimit));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Extract the actions
        float accelerationInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float steeringInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        float brakingInput = Mathf.Clamp(actions.ContinuousActions[2], 0f, 1f);

        // Call the TestControls methods based on the received actions
        carControls.HandleAcceleration(accelerationInput);
        carControls.HandleSteering(steeringInput);
        carControls.HandleBraking(brakingInput);

        // Punish it for being passive.
        //AddReward(-0.0001f);

        // Function that rewards the car for getting closer
        RewardForGettingCloser();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");  // Acceleration
        continuousActions[1] = Input.GetAxisRaw("Horizontal");  // Steering
        continuousActions[2] = Input.GetKey(KeyCode.Space) ? 1f : 0f;  // Braking
        RewardForGettingCloser();
    }

    // Collision logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal)) 
        {
            AddReward(+3000f);
            Debug.Log("Completed task!");
            //EndEpisode();
            EndEpisodeAndStopTimer();
        }
        else if (other.TryGetComponent<RedWall>(out RedWall redwall)) 
        {
            AddReward(-100f);
            //Debug.Log("Drove offroad!");
            //EndEpisode();
            EndEpisodeAndStopTimer();
        }
        
        else if (other.TryGetComponent<ParkedCar>(out ParkedCar parkedCar)) 
        {
            AddReward(-100f);
            Debug.Log("Hit parked Car!");
            EndEpisodeAndStopTimer();
        }
    }
    private void RewardForGettingCloser()
    {
        if (targetTransform != null)
        {
            float currentDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
            //float currentDistance = Vector3.Distance(transform.position, targetTransform.position);

            // Check if currentDistance is closer than it's been before
            if (currentDistance < closestDistance)
            {
                float reward = (closestDistance - currentDistance) * 1f; // Adjust as needed // Changed from 0.1f!
                AddReward(reward);
                closestDistance = currentDistance;
                totalDistanceReward += reward;
                //Debug.Log("Reward for getting closer: " + reward);
            }
        }
    }
    private void EndEpisodeAndStopTimer()
    {
    // Stop the timer coroutine if it's running
        if (episodeTimer != null)
        {
            StopCoroutine(episodeTimer);
        }
        //AddReward(-8f);
        Debug.Log("total distance reward = " + totalDistanceReward);
        EndEpisode();
    }
    private IEnumerator EndEpisodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //AddReward(-8f);
        Debug.Log("total distance reward = " + totalDistanceReward);
        EndEpisode();
    }
}