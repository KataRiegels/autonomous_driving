using UnityEngine;
using System.Collections.Generic;

public class AgentColorManager : MonoBehaviour
{
    private List<Renderer> targetRenderers = new List<Renderer>(); // List to hold renderers
    [SerializeField] private Material activeMaterial;  // Material for active state
    [SerializeField] private Material inactiveMaterial; // Material for inactive state

    void Awake()
    {
        // Add the Renderer of this GameObject if it exists
        var ownRenderer = GetComponent<Renderer>();
        if (ownRenderer != null)
        {
            targetRenderers.Add(ownRenderer);
        }

        // Search for and add Renderer of child objects named "Top"
        foreach (Transform child in transform)
        {
            if (child.name == "Top")
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    targetRenderers.Add(childRenderer);
                }
            }
        }
    }

    public void SetActiveMaterial()
    {
        foreach (var renderer in targetRenderers)
        {
            if (renderer != null)
            {
                renderer.material = activeMaterial;
            }
        }
    }

    public void SetInactiveMaterial()
    {
        foreach (var renderer in targetRenderers)
        {
            if (renderer != null)
            {
                renderer.material = inactiveMaterial;
            }
        }
    }
}
