using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkedCarManager : MonoBehaviour
{
    public GameObject carPrefab; // The car prefab to instantiate
    public Transform[] spawnPositions; // The positions where cars will be spawned
    public int numberOfCarsToSpawn; // The number of cars to spawn

    private List<GameObject> parkedCars; // List to keep track of parked car instances

    void Start()
    {
        parkedCars = new List<GameObject>();
        SpawnCars();
    }

    private void SpawnCars()
    {
        // Clear existing cars
        foreach (var car in parkedCars)
        {
            Destroy(car);
        }
        parkedCars.Clear();

        // New spawning logic
        List<Transform> availablePositions = new List<Transform>(spawnPositions);
        for (int i = 0; i < numberOfCarsToSpawn; i++)
        {
            if (availablePositions.Count == 0)
                break; // Break if there are no more positions available

            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform spawnPosition = availablePositions[randomIndex];
            GameObject newCar = Instantiate(carPrefab, spawnPosition.position, spawnPosition.rotation);

            parkedCars.Add(newCar); // Add the new car to the list

            availablePositions.RemoveAt(randomIndex); // Remove the used position
        }
    }

    public void RepositionParkedCars()
    {
        List<Transform> availablePositions = new List<Transform>(spawnPositions);
        foreach (GameObject car in parkedCars)
        {
            if (availablePositions.Count == 0)
                break;

            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform newPosition = availablePositions[randomIndex];
            car.transform.position = newPosition.position;
            car.transform.rotation = newPosition.rotation;

            availablePositions.RemoveAt(randomIndex);
        }
    }
}