using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessPoint
{
    public Vector2 worldPosition;
    public Vector2Int indexPosition;
    public int currentHealth;
    public int maxHealth;
    private bool active;
    public float density;
    public DarknessSpec spec;

    public void Init(DarknessSpec spec)
    {
        // TODO SET SETTINGS WITH SPEC
        currentHealth = spec.currentHealth;
        maxHealth = spec.maxHealth;
        density = spec.density;
        this.spec = spec;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public bool IsActive()
    {
        return active && IsAlive();
    }

    public void SetActive(bool a)
    {
        active = a;
    }

    // Attempts to spread, returning true if it did
    public bool Spread(Vector3[] openSpots, int size, FogOfDarknessManager manager)
    {
        if (Random.Range(0f, 1f) > 0.95)
        {
            manager.CreateDarknessPoint(openSpots[Random.Range(0, size)], spec);
            return true;
        }
        return false;
    }
}
