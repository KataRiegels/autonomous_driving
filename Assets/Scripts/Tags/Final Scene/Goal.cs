using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool enableLocationChange = true; // Public variable to toggle location change

    // Function to set the goal location
    public void SetGoalLocation()
    {
        if (enableLocationChange) // Check if location change is enabled
        {
            // Define the two positions
            Vector3 position1 = new Vector3(-25, 1.93f, 157);
            Vector3 position2 = new Vector3(35, 1.93f, 157);

            // Randomly select one of the positions
            Vector3 selectedPosition = Random.Range(0, 2) == 0 ? position1 : position2;

            // Set the transform position of this object
            transform.position = selectedPosition;
        }
    }
}
