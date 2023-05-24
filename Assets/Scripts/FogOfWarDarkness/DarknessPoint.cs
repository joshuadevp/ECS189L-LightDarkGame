using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessPoint
{
    public GameObject darknessObject;
    public Vector2 position;
    private bool active;
    private int health = 1;

    public void Init(DarknessSpec spec)
    {
        position.x = darknessObject.transform.position.x;
        position.y = darknessObject.transform.position.z;
        // TODO SET SETTINGS WITH SPEC
    }

    public bool IsActive()
    {
        return health > 0;
    }

    public void SetActive(bool a)
    {
        darknessObject.SetActive(a);
        active = a;
    }
}
