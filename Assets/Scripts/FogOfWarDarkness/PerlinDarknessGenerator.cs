using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PerlinDarknessGenerator : IDarknessGenerator
{
    public float refinement;
    private float seed;
    void OnEnable()
    {
        seed = Random.Range(0, 65534);
    }

    override public DarknessSpec Generate(Vector2 loc) 
    {
        float noise = Mathf.PerlinNoise(loc.x * refinement + seed, loc.y * refinement + seed);
        return new DarknessSpec() { density = noise, currentHealth = 1, maxHealth = 1};
    }
}
