//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    private Vector3 initialPosition;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Checkpoint[] checkpoints;
    void Start()
    {
        initialPosition = transform.position;
    }
    public override void OnEpisodeBegin()
    {
        transform.position = initialPosition;

        // Respawn all checkpoints in TrainingGround 
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.ActivateCheckpoints();
        }
    }
   
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector3)transform.localPosition);
        sensor.AddObservation((Vector3)targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GreenWall>(out GreenWall greenWall)) 
        {
            AddReward(+30f);
            EndEpisode();
        }
        else if (other.TryGetComponent<RedWall>(out RedWall redwall)) 
        {
            AddReward(-10f);
            EndEpisode();
        }
        else if (other.TryGetComponent<CPReward>(out CPReward cpReward)) 
        {
            AddReward(+5f);
        }
        else if (other.TryGetComponent<CPPunish>(out CPPunish cpPunish)) 
        {
            AddReward(-8f);
        }
    }
}
