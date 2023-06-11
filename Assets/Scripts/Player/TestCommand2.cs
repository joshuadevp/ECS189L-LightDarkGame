using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified from Dretis's flicker command
public class TestCommand2 : PlayerCommand
{
    private float baseSize;
    private float damageMultiplier;
    private float shotInterval = 0.5f;
    private float lifetime = 2.0f;
    [SerializeField] GameObject projectile;

    private float timeSinceLastShot = 0;
    public override void Execute(GameObject player)
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shotInterval)
        {
            Debug.Log("Shoot Flicker");
            Vector3 velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized * 10f;
            Instantiate(projectile, player.transform.position, Quaternion.identity).GetComponent<Projectile>().Initialize(1, velocity);
            
            timeSinceLastShot = 0;
        }
        else
        {
            Debug.Log("Flicker On Cooldown");
        }
    }
}