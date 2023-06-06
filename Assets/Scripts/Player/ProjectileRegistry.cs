using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRegistry : MonoBehaviour
{
    // Supplementary script to hold proj prefabs.
    [SerializeField]
    private List<GameObject> projectiles;
    public GameObject GetProjectile(int index)
    {
        // if the index is out of bounds
        if (index >= projectiles.Count)
        {
            // Default to the first one
            return projectiles[0];
        }
        return projectiles[index];
    }
}
