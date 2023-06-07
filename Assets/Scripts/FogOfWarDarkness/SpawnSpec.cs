using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains data for spawning an enemy
public struct SpawnSpec
{
    public GameObject Enemy;
    public DarknessSettings GlobalSettings;
    public float SpawnChanceModifier;
}
