using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SurviveObjective : ScriptableObject, IObjective
{
    [SerializeField]
    DarknessSettings globalSettings;
    [SerializeField]
    float removeDarknessRadius;
    [SerializeField]
    float playerTetherDistance;
    [SerializeField]
    float timeToSurvive;
    [SerializeField]
    float newGlobalSpawnChance;
    [SerializeField]
    float newGlobalSpreadChance;
    private GameObject player;
    private bool completed = false;
    private Vector2 loc;
    private float timer;
    private float oldSpreadChance;
    private float oldSpawnChance;

    public void ManualUpdate()
    {
        if (!completed && Vector3.Magnitude(player.transform.position - (Vector3)loc) < playerTetherDistance)
        {
            globalSettings.DarknessSpreadChance = newGlobalSpreadChance;
            globalSettings.EnemySpawnChance = newGlobalSpawnChance;
            timer += Time.deltaTime;
        }
        if(timer > timeToSurvive)
        {
            globalSettings.DarknessSpreadChance = oldSpreadChance;
            globalSettings.EnemySpawnChance = oldSpawnChance;
            completed = true;
        }
    }

    public void Setup(Vector2 loc)
    {
        this.loc = loc;
        FogOfDarknessManager manager = FindObjectOfType<FogOfDarknessManager>();
        manager.RemoveDarknessPointsCircle(loc, removeDarknessRadius);
        player = GameObject.FindFirstObjectByType<Player>().gameObject;
        oldSpawnChance = globalSettings.EnemySpawnChance;
        oldSpreadChance = globalSettings.DarknessSpreadChance;
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
