using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PerlinDarknessGenerator : IDarknessGenerator
{
    [SerializeField]
    float refinement;
    private float seed;

    void OnEnable()
    {
        seed = Random.Range(0, 65534);
    }

    override public DarknessSpec Generate(Vector2 loc)
    {
        float noise = Mathf.PerlinNoise(loc.x * refinement + seed, loc.y * refinement + seed);
        float modifier = Mathf.Clamp(noise, 0.5f, 1f);
        int health = (int)(defaultSettings.DarknessMaxHealth / modifier);
        return new DarknessSpec()
        {
            Density = modifier,
            CurrentHealth = health,
            MaxHealth = health,
            SpreadChanceModifier = 1f / modifier,
            GlobalSettings = globalSettings,
            SpawnSpec = new SpawnSpec()
            {
                SpawnChanceModifier = 1f / modifier,
                Enemy = enemy,
                GlobalSettings = globalSettings
            }
        };
    }

    override public DarknessSpec GenerateDefault()
    {
        return new DarknessSpec()
        {
            Density = 1,
            CurrentHealth = defaultSettings.DarknessMaxHealth,
            MaxHealth = defaultSettings.DarknessMaxHealth,
            SpreadChanceModifier = 1f,
            GlobalSettings = globalSettings,
            SpawnSpec = new SpawnSpec()
            {
                SpawnChanceModifier = 1f,
                Enemy = enemy,
                GlobalSettings = globalSettings
            }
        };
    }
}
