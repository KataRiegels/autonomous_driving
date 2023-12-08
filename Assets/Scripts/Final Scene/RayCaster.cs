using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public Transform rayOriginBack;
    public Transform rayOriginFront;
    public float rayLength = 20.0f;
    public LayerMask layerMask;
    public Color rayColor = Color.red;

    public bool isHitBack { get; private set; }
    public bool isHitFront { get; private set; }
    public float hitDistanceBack { get; private set; }
    public float hitDistanceFront { get; private set; }

    void Update()
    {
        (isHitBack, hitDistanceBack) = CastRay(rayOriginBack);
        (isHitFront, hitDistanceFront) = CastRay(rayOriginFront);
    }

    private (bool isHit, float hitDistance) CastRay(Transform rayOrigin)
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.right;

        bool isHit = Physics.Raycast(rayOrigin.position, rayDirection, out hit, rayLength, layerMask);

        if (isHit)
        {
            Debug.DrawRay(rayOrigin.position, rayDirection * hit.distance, rayColor);
            return (true, hit.distance);
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayDirection * rayLength, rayColor);
            return (false, rayLength);
        }
    }
    public float GetParallelismRatio()
    {
        if (!isHitBack || !isHitFront) return 0f; // No ratio if either ray isn't hitting

        float shorterDistance = Mathf.Min(hitDistanceBack, hitDistanceFront);
        float longerDistance = Mathf.Max(hitDistanceBack, hitDistanceFront);
        //Debug.Log(shorterDistance/longerDistance);
        return shorterDistance / longerDistance; // Ratio of 0 to 1
    }
}
