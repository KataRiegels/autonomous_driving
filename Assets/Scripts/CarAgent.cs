using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CarAgent : Agent
{
    //private SimpleCarController carController;
    private TestControls testControls;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public float steeringInput = 0f;
    public float brakeInput = 0f;
    //[System.Serializable]
    //public GameObject[] tracks;

    //[System.Serializable]
    /*public class Rewards
    {
        public float greenWall = 0.1f;
        public float redWall = -0.01f;
    }*/
    //public Rewards reward = new Rewards();
    private void Start()
    {
        //carController = GetComponent<SimpleCarController>();
        testControls = GetComponent<TestControls>();

        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

    }


    public override void OnEpisodeBegin()
    {   // Reset the environment for the next episode here
        //Debug.Log("Episode began, resetting agent");  // Check reset

        // Reset the car's position and rotation to it's initial state
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset the car's speed to 0
        testControls.ResetSpeed();

        // Activate tires
        //foreach (var obj in tracks)
        //{
        //    obj.SetActive(true);
        //}


    }

    /*public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(testControls.currentSpeed / testControls.maxSpeed);
    }*/

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float accelerationInput = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        //float steeringInput = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);
        steeringInput = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);
        brakeInput = Mathf.Clamp(actionBuffers.ContinuousActions[2], 0f, 1f);

        // Use the inputs of our car controller script
        testControls.ApplyAcceleration(accelerationInput);
        testControls.ApplySteering(steeringInput);
        testControls.ApplyBraking(brakeInput);
        // testControls.ApplyBrakingw

        // Add a small penalty for every action taken (every time step)
        //AddReward(-0.001f);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float accelerationInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            brakeInput = 0.5f;
        } else { brakeInput = 0f; }
        //float steeringInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Alpha1))

        {
            steeringInput = -1f;
               testControls.CalculateSteering(steeringInput);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            steeringInput = -0.5f;
               testControls.CalculateSteering(steeringInput);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            steeringInput = 0f;
               testControls.CalculateSteering(steeringInput);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            steeringInput = 0.5f;
               testControls.CalculateSteering(steeringInput);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            steeringInput = 1f;
               testControls.CalculateSteering(steeringInput);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            OnEpisodeBegin();
        }

        // testControls.ApplyAcceleration(accelerationInput);
        // testControls.ApplySteering(steeringInput);
        actionsOut.ContinuousActions.Array[0] = accelerationInput;
        actionsOut.ContinuousActions.Array[1] = steeringInput;
        actionsOut.ContinuousActions.Array[2] = brakeInput;
    }

    /*public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("GreenWall"))
        {
            Debug.Log("Collided with GreenWall");
            AddReward(reward.greenWall);

            // De-activate tires
            //foreach (var obj in tracks)
            //{
            //    obj.SetActive(false);
            //}
            //
            //EndEpisode();


        }
        else if (collision.gameObject.CompareTag("RedWall"))
        {
            Debug.Log("Collided with RedWall");
            AddReward(reward.redWall);

            // De-activate tires
            //foreach (var obj in tracks)
            //{
            //    obj.SetActive(false);
            //}

            EndEpisode();

        }
    }*/
}