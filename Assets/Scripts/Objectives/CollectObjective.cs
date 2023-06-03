using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollectObjective : ScriptableObject, IObjective
{
    [SerializeField]
    private GameObject collectable;
    private GameObject player;
    bool completed = false;
    bool started = false;

    public void ManualUpdate()
    {
        if (!completed && started && Vector3.Magnitude(player.transform.position - collectable.transform.position) < 1)
        {
            completed = true;
            Destroy(collectable);
        }
    }

    public void Setup(Vector2 loc)
    {
        // Generate new collectable instance and place where we want it
        collectable = Instantiate(collectable, loc, Quaternion.identity);
        player = GameObject.FindFirstObjectByType<MoveCube>().gameObject;
        started = true;
    }

    public bool Completed()
    {
        return completed;
    }
}
