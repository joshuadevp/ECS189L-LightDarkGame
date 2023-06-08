using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains data for spawning an enemy
public struct SpawnSpec
{
    public GameObject Enemy;
    public DarknessSettings GlobalSettings;
    // Float to multiply by spawn chance.
    public float SpawnChanceModifier;
}
