using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryFlickerCommand : MonoBehaviour, IPlayerCommand
{
    private float baseSize;
    private float damageMultiplier;
    private float shotInterval = 0.5f;
    private float lifetime = 2.0f;

    private float timeSinceLastShot = 0;
    public void Execute(GameObject gameObject)
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shotInterval)
        {
            Debug.Log("Shoot Flicker");
            //Instantiate()
            timeSinceLastShot = 0;
        }
        else
        {
            Debug.Log("Flicker On Cooldown");
        }
    }
}