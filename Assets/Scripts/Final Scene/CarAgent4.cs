using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent4 : Agent
{
    [Header("Agent Parameters")]
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float closestDistance;
    private float totalDistanceReward;
    [SerializeField] private Transform targetTransform;
    private TestControls carControls; // Reference to the TestControls script
    private EpisodeManager episodeManager; // Reference to the Episode Manager
    public bool IsActive { get; private set; } // Flag to indicate if the agent is active
    public bool HasHitGoal { get; private set; } = false;
    private AgentColorManager colorManager;
    private RayCaster rayCaster;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        carControls = GetComponent<TestControls>();  // Get the TestControls component attached to the same GameObject
        episodeManager = FindObjectOfType<EpisodeManager>(); // Find the Episode Manager in the scene
        colorManager = GetComponent<AgentColorManager>();
        rayCaster = GetComponentInChildren<RayCaster>();
        IsActive = true;
    }

    public override void OnEpisodeBegin()
    {
        IsActive = true;
        HasHitGoal = false;
        // Change color to active
        colorManager?.SetActiveMaterial();
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        carControls.ResetControls();
        closestDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
        //Debug.DrawLine(transform.localPosition, targetTransform.localPosition, Color.red);
        totalDistanceReward = 0f;
    }
    // collect observations
    //public override void CollectObservations(VectorSensor sensor)
    //{
        // Add agent's own position
        //sensor.AddObservation(transform.localPosition);

        // Add the target's position
        //sensor.AddObservation(targetTransform.localPosition);


        // Add other observations if needed
    //}

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.DrawLine(transform.localPosition, targetTransform.localPosition, Color.green);
        if (!IsActive)  {return;}
        // Extract the actions
        float accelerationInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float steeringInput = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        float brakingInput = Mathf.Clamp(actions.ContinuousActions[2], 0f, 1f);

        // Call the TestControls methods based on the received actions
        carControls.HandleAcceleration(accelerationInput);
        carControls.HandleSteering(steeringInput);
        carControls.HandleBraking(brakingInput);

        // Function that rewards the car for getting closer
        RewardForGettingCloser();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (!IsActive)  {return;}
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");  // Acceleration
        continuousActions[1] = Input.GetAxisRaw("Horizontal");  // Steering
        continuousActions[2] = Input.GetKey(KeyCode.Space) ? 1f : 0f;  // Braking
        //RewardForGettingCloser();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive)  {return;}
        if (other.TryGetComponent<Goal>(out Goal goal)) 
        {
            AddReward(10f);
            //Debug.Log("Completed task!");
            HasHitGoal = true;
            SignalCompletion();
        }
        else if (other.TryGetComponent<RedWall>(out RedWall redwall)) 
        {
            AddReward(-50f);
            SignalCompletion();
        }
        else if (other.TryGetComponent<RedLight>(out RedLight redlight)) 
        {
            AddReward(-50f);
            SignalCompletion();
        }
        else if (other.TryGetComponent<Pedestrian>(out Pedestrian pedestrian)) 
        {
            AddReward(-50f);
            SignalCompletion();
        }
        else if (other.TryGetComponent<ParkedCar>(out ParkedCar parkedCar)) 
        {
            AddReward(-50f);
            //Debug.Log("Hit parked Car!");
            SignalCompletion();
        }
    }

    private void RewardForGettingCloser()
    {
        if (!IsActive || targetTransform == null) return;

        float currentDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
        if (currentDistance < closestDistance)
        {
            // Get the parallelism ratio from the RayCaster
            float parallelismRatio = rayCaster.GetParallelismRatio();

            // Apply the ratio to the distance reward
            float reward = (closestDistance - currentDistance) * 0.5f; // Adjust reward with ratio
            reward += (closestDistance - currentDistance) * 0.5f * parallelismRatio;
            //reward += 1f; // Adjust reward with ratio
            // float reward = (closestDistance - currentDistance) * parallelismRatio; // Adjust reward with ratio
            AddReward(reward);
            closestDistance = currentDistance;
            totalDistanceReward += reward;
        }
        else {AddReward(-0.01f);}
    }

    private void SignalCompletion()
    {
        IsActive = false;
        // Change color to nonactive
        colorManager?.SetInactiveMaterial();
        // Set current speed of car to 0
        carControls.Freeze();
        //Debug.Log("totalDistanceReward: " + totalDistanceReward);
    }

    public new void StartNewEpisode()
    {
        OnEpisodeBegin();
    }

    public void EndEpisode()
    {
        //Debug.Log("totalDistanceReward: " + totalDistanceReward);
        base.EndEpisode();
    }
}
