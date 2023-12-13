using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalControl : MonoBehaviour
{
    public bool enableLocationChange = true; // Public variable to toggle location change

    // Function to set the goal location
    public void SetGoalLocation()
    {
        if (enableLocationChange) // Check if location change is enabled
        {
            // Define the two positions
            Vector3 position1 = new Vector3(-75, 1.93f, 140);
            Vector3 position2 = new Vector3(130, 1.93f, 140);
            Vector3 position3 = new Vector3(130, 1.93f, -50);
            Vector3 position4 = new Vector3(-75, 1.93f, -50);
            
            // Create an array of positions
            Vector3[] positions = new Vector3[] { position1, position2, position3, position4 };

            // Randomly select one of the positions
            Vector3 selectedPosition = positions[Random.Range(0, positions.Length)];

            // Set the transform position of this object
            transform.position = selectedPosition;
        }
    }
}
