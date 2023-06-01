using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObjective : ScriptableObject, IObjective
{
    GameObject collectable;
    GameObject player;
    bool completed = false;

    void OnEnable()
    {

    }

    void Update()
    {
        if (Vector3.Magnitude(player.transform.position - collectable.transform.position) < 1)
        {
            completed = true;
            Destroy(collectable);
        }
    }

    public void Setup(Vector2 loc)
    {
        // Find player and collectable we want to spawn
    }

    public bool Completed()
    {
        return completed;
    }
}
