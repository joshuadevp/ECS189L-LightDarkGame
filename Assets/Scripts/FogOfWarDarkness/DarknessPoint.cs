using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessPoint
{
    public Vector3 worldPosition { get; set; }
    public Vector2Int indexPosition { get; set; }
    public int currentHealth { get; set; }
    public int maxHealth { get; set; }
    private bool active;

    public void Init(DarknessSpec spec)
    {
        // TODO SET SETTINGS WITH SPEC
        currentHealth = spec.currentHealth;
        maxHealth = spec.maxHealth;
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
}
