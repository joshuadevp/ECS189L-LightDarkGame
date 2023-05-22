using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessPoint
{
    public Vector3 worldPosition;
    public Vector2Int indexPosition;
    private bool active;
    private int health;

    public void Init(DarknessSpec spec)
    {
        health = 1;
        // TODO SET SETTINGS WITH SPEC
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetActive(bool a)
    {
        active = a;
    }
}
