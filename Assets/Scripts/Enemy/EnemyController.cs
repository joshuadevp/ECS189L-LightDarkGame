using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected Enemy main;
    protected PlayerController player;

    public abstract void HitBy(GameObject projectile);
}
