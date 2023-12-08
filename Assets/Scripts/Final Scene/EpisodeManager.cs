using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;

public class EpisodeManager : MonoBehaviour
{
    public List<CarAgent4> agents; // List to hold all agents
    public StopLight stopLight;
    public PedestrianManager pedestrianManager; // Reference to the PedestrianManager
    public ParkedCarManager parkedCarManager; // Reference to the ParkedCar script
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
        foreach (var agent in agents)
        {
            agent.EndEpisode();
        }
        stopLight.ResetStopLight();
    }

    private void StartNewEpisode()
    {
        foreach (var agent in agents)
        {
            agent.StartNewEpisode();
        }
        pedestrianManager.RestartPedestrianSpawning(); // Restart pedestrian spawning
        parkedCarManager.RepositionParkedCars();
        episodeInProgress = true;
        episodeStartTime = Time.time;
    }
}