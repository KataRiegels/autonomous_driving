using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;

public class EpisodeManager : MonoBehaviour
{
    public List<CarAgent4> agents; // List to hold all agents
    public StopLight stopLight;
    public PedestrianManager pedestrianManager; // Reference to the PedestrianManager
    public ParkedCarManager parkedCarManager; // Reference to the ParkedCar script
    public Goal goalObject;
    //public GoalControl goalObject2;
    private bool episodeInProgress = true;
    private float episodeStartTime;
    [SerializeField] private float maxEpisodeDuration = 30.0f; // Maximum duration of an episode

    private void Start()
    {
        agents = new List<CarAgent4>(FindObjectsOfType<CarAgent4>());
        episodeStartTime = Time.time;
    }

    private void Update()
    {
        if (episodeInProgress && (AllAgentsDone() || Time.time - episodeStartTime >= maxEpisodeDuration))
        {
            EndEpisodeForAllAgents();
            episodeInProgress = false;
            StartNewEpisode();
        }
    }

    private bool AllAgentsDone()
    {
        foreach (var agent in agents)
        {
            if (agent.IsActive)
            {
                return false;
            }
        }
        return true;
    }

    private void EndEpisodeForAllAgents()
    {
        CheckAllAgentsHitGoal();
        foreach (var agent in agents)
        {
            agent.EndEpisode();
        }
        stopLight.ResetStopLight();
    }

    private void StartNewEpisode()
    {
        goalObject.SetGoalLocation();
        //goalObject2.SetGoalLocation();
        foreach (var agent in agents)
        {
            agent.StartNewEpisode();
        }
        pedestrianManager.RestartPedestrianSpawning(); // Restart pedestrian spawning
        parkedCarManager.RepositionParkedCars();
        episodeInProgress = true;
        episodeStartTime = Time.time;
        GameControl.IncrementEpisode();
    }
    private void CheckAllAgentsHitGoal()
    {
        bool allAgentsHitGoal = true;
        foreach (var agent in agents)
        {
            if (!agent.HasHitGoal)
            {
                allAgentsHitGoal = false;
                break; // If one agent hasn't hit the goal, no need to check further
            }
        }

        if (allAgentsHitGoal)
        {
            GameControl.IncrementSuccession();
        }
        else
        {
            GameControl.ResetSuccession();
        }
    }
}