using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PerlinDarknessGenerator : IDarknessGenerator
{
    [SerializeField]
    DarknessSettings defaultSettings;
    [SerializeField]
    GameObject enemy;
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
            SpreadChance = defaultSettings.DarknessSpreadChance / modifier,
            SpawnSpec = new SpawnSpec()
            {
                SpawnChance = defaultSettings.EnemySpawnChance / modifier,
                Enemy = enemy
            }
        };
    }
}
