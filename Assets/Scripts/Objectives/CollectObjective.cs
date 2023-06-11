using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollectObjective : ScriptableObject, IObjective
{
    [SerializeField]
    private GameObject collectable;
    private GameObject player;
    private bool completed = false;
    private bool started = false;
    private Vector2 loc;

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
        this.loc = loc;
        // Generate new collectable instance and place where we want it
        collectable = Instantiate(collectable, loc, Quaternion.identity);
        // TODO: CHANGE TO TYPE OF ACTUAL PLAYER SCRIPT
        player = GameObject.FindFirstObjectByType<MoveCube>().gameObject;
        started = true;
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
