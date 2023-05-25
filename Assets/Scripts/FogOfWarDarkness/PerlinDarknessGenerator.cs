using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinDarknessGenerator : IDarknessGenerator
{
    private float seed;
    public PerlinDarknessGenerator()
    {
        seed = Random.Range(0, float.MaxValue / 2);
    }
    public DarknessSpec Generate(Vector2 loc)
    {
        float noise = Mathf.PerlinNoise(loc.x + seed, loc.y + seed);
        return new DarknessSpec();
    }
}
