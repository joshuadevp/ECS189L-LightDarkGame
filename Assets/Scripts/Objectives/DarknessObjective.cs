using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DarknessObjective : ScriptableObject, IObjective
{
    [SerializeField]
    float radius;
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    IDarknessGenerator darknessGenerator;
    [SerializeField]
    float spawnChanceModifier;
    private Vector2 loc;
    private GameObject player;
    private bool completed;
    private List<DarknessPoint> points;

    public void ManualUpdate()
    {
        if (!completed)
        {
            foreach (DarknessPoint p in points)
            {
                // If any point in the list is alive the objective is not complete
                if (p.IsAlive())
                {
                    return;
                }
            }
            completed = true;
        }
    }

    public void Setup(Vector2 loc)
    {
        this.loc = loc;
        // Find player and Spawn boss enemy
        FogOfDarknessManager manager = FindObjectOfType<FogOfDarknessManager>();
        // Spawn darkness points
        DarknessSpec spec = darknessGenerator.GenerateDefault();
        spec.SpreadChanceModifier = 0; // We don't want our special darkness spreading
        spec.SpawnSpec.SpawnChanceModifier = spawnChanceModifier; // It should spawn more enemies
        spec.Density = 3;
        points = manager.CreateDarknessPointsCircle(loc, radius, spec);
    }

    public bool Completed()
    {
        return completed;
    }

    public Vector2 GetLocation()
    {
        return loc;
    }
}
