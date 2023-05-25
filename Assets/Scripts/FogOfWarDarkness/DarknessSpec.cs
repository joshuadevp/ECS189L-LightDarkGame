using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct DarknessSpec
{
    public int maxHealth;
    public int currentHealth;
    public SpawnSpec spawnSpec;
    public float density;

    static public DarknessSpec GetNullSpec() { return new DarknessSpec() { density = 0, maxHealth = 0, currentHealth = 0 }; }
}
