using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For handling lists

public class PedestrianManager : MonoBehaviour
{
    [System.Serializable]
    public class ZonePair
    {
        public BoxCollider areaA;
        public BoxCollider areaB;
    }

    public GameObject pedestrianPrefab; // Assign the pedestrian prefab
    public ZonePair[] zonePairs; // Assign zone pairs representing the spawn areas
    public float fixedY; // The fixed Y-axis value for pedestrian spawning
    public float spawnInterval = 3.0f; // Time interval for spawning pedestrians

    private Coroutine pedestrianSpawningCoroutine;
    private List<GameObject> spawnedPedestrians = new List<GameObject>(); // Keep track of spawned pedestrians

    private void Start()
    {
        pedestrianSpawningCoroutine = StartCoroutine(SpawnPedestrians());
    }

    private IEnumerator SpawnPedestrians()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Choose a random zone pair
            ZonePair selectedZone = zonePairs[UnityEngine.Random.Range(0, zonePairs.Length)];

            // Choose randomly between area A and area B to spawn
            BoxCollider spawnArea = (UnityEngine.Random.Range(0, 2) == 0) ? selectedZone.areaA : selectedZone.areaB;
            BoxCollider targetArea = (spawnArea == selectedZone.areaA) ? selectedZone.areaB : selectedZone.areaA;

            Vector3 spawnPosition = GetRandomPositionInCollider(spawnArea);
            GameObject pedestrian = Instantiate(pedestrianPrefab, spawnPosition, Quaternion.identity);

            // Get the closest point on the target zone's perimeter
            Vector3 targetPosition = GetClosestPointOnZonePerimeter(targetArea, pedestrian.transform.position);

            Pedestrian pedestrianScript = pedestrian.GetComponent<Pedestrian>();
            pedestrianScript.SetTarget(targetPosition);

            spawnedPedestrians.Add(pedestrian); // Add the new pedestrian to the list
        }
    }

    private Vector3 GetRandomPositionInCollider(BoxCollider collider)
    {
        Vector3 point = new Vector3(
            UnityEngine.Random.Range(-collider.size.x / 2, collider.size.x / 2),
            0, // This will be overridden to fixedY
            UnityEngine.Random.Range(-collider.size.z / 2, collider.size.z / 2)
        );

        point = collider.transform.TransformPoint(point); // Convert to world space
        point.y = fixedY; // Override the Y position with fixedY

        return point;
    }

    private Vector3 GetClosestPointOnZonePerimeter(BoxCollider zone, Vector3 position)
    {
        // Convert the world position to the local space of the collider
        Vector3 localPosition = zone.transform.InverseTransformPoint(position);

        // Clamp the position to the collider's bounds
        localPosition.x = Mathf.Clamp(localPosition.x, -zone.size.x / 2, zone.size.x / 2);
        localPosition.z = Mathf.Clamp(localPosition.z, -zone.size.z / 2, zone.size.z / 2);

        // Convert the position back to world space
        Vector3 worldPosition = zone.transform.TransformPoint(localPosition);

        // Override the Y position with fixedY
        return new Vector3(worldPosition.x, fixedY, worldPosition.z);
    }

    public void StopAndClearPedestrians()
    {
        if (pedestrianSpawningCoroutine != null)
        {
            StopCoroutine(pedestrianSpawningCoroutine);
        }

        // Destroy all pedestrians
        foreach (var pedestrian in spawnedPedestrians)
        {
            Destroy(pedestrian);
        }
        spawnedPedestrians.Clear();
    }

    public void RestartPedestrianSpawning()
    {
        StopAndClearPedestrians();
        pedestrianSpawningCoroutine = StartCoroutine(SpawnPedestrians());
    }

}
