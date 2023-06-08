using UnityEngine;

// Contains data for a point of darkness
public struct DarknessSpec
{
    public int MaxHealth;
    public int CurrentHealth;
    public SpawnSpec SpawnSpec;
    // Float between 0 and 1 to represent how strong the point is.
    public float Density;
    public DarknessSettings GlobalSettings;
    // Float to multiply by spread chance.
    public float SpreadChanceModifier;

    static public DarknessSpec GetNullSpec() { return new DarknessSpec() { Density = 0, MaxHealth = 0, CurrentHealth = 0 }; }
}
