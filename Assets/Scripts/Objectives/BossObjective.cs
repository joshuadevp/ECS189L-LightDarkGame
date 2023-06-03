using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObjective : ScriptableObject, IObjective
{
    GameObject boss;
    bool completed = false;

    void OnEnable()
    {

    }

    public void ManualUpdate()
    {
        if (boss == null)
        {
            completed = true;
        }
    }

    public void Setup(Vector2 loc)
    {
        // Find player and Spawn boss enemy
        FogOfDarknessManager manager = FindObjectOfType<FogOfDarknessManager>();
    }

    public bool Completed()
    {
        return completed;
    }
}
