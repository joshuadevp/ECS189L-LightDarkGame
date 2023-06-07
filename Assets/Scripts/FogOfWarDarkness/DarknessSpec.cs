// Contains data for a point of darkness
public struct DarknessSpec
{
    public int MaxHealth;
    public int CurrentHealth;
    public SpawnSpec SpawnSpec;
    public float Density;
    public DarknessSettings GlobalSettings;
    public float SpreadChanceModifier;

    static public DarknessSpec GetNullSpec() { return new DarknessSpec() { Density = 0, MaxHealth = 0, CurrentHealth = 0 }; }
}
