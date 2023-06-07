using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Represents a single point of darkness in world
public class DarknessPoint
{
    private bool active;

    public Vector2 WorldPosition;
    public Vector2Int IndexPosition;
    public int CurrentHealth;
    public int MaxHealth;
    public float Density;
    public DarknessSpec DarknessSpec;

    public void Init(DarknessSpec spec)
    {
        // TODO SET SETTINGS WITH SPEC
        CurrentHealth = spec.CurrentHealth;
        MaxHealth = spec.MaxHealth;
        Density = spec.Density;
        this.DarknessSpec = spec;
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
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
    public bool Spread(Vector2[] openSpots, int size, FogOfDarknessManager manager)
    {
        if (DarknessSpec.GlobalSettings == null) return false; // Points without settings cannot spread
        if (Random.Range(0f, 1f) < DarknessSpec.GlobalSettings.DarknessSpreadChance * DarknessSpec.SpreadChanceModifier)
        {
            manager.CreateDarknessPoint(openSpots[Random.Range(0, size)], DarknessSpec);
            return true;
        }
        return false;
    }

    // Attempts to spawn an enemy, returning true if it did
    public bool Spawn()
    {
        if (Random.Range(0f, 1f) < DarknessSpec.SpawnSpec.GlobalSettings.EnemySpawnChance * DarknessSpec.SpawnSpec.SpawnChanceModifier)
        {
            GameObject.Instantiate(DarknessSpec.SpawnSpec.Enemy, WorldPosition, Quaternion.identity);
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return IndexPosition.GetHashCode();
    }
}
