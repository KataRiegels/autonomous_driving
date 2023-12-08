using UnityEngine;

public class GoalPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetRandomPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has one of the specified tags
        if (other.CompareTag("car") || other.CompareTag("parkedCar") || other.CompareTag("goal"))
        {
            SetRandomPosition();
        }
    }

    public void SetRandomPosition()
    {
        float x = Random.Range(-90.0f, 90.0f);
        float z = Random.Range(-90.0f, 90.0f);
        float y = 1.93f; // Fixed y position

        transform.position = new Vector3(x, y, z);
    }
}
