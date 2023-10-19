using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // This method is called when another collider enters this object's trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object contains the "CarAgent" component
        if (other.TryGetComponent<MoveToGoalAgent>(out MoveToGoalAgent moveToGoalAgent))
        {
            // Disable this object (the one with the Checkpoint script attached)
            gameObject.SetActive(false);
        }
    }

    // Public method to activate the object
    public void ActivateCheckpoints()
    {
        gameObject.SetActive(true);
    }

}
