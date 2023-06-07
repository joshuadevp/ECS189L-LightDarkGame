using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryFlickerCommand : MonoBehaviour, IAbilityCommand
{
    private GameObject projectilePrefab;

    private float baseSize;
    private float damageMultiplier;
    private float shotSpeed = 20.0f;
    private float shotInterval = 0.25f;
    private float lifetime = 2.0f;

    //private float timeSinceLastShot = 0;

    private void Awake()
    {
        projectilePrefab = FindObjectOfType<ProjectileRegistry>().GetProjectile(0);
    }
    public float Execute(GameObject gameObject, float timeSinceLastShot)
    {
        //timeSinceLastShot += Time.deltaTime;

        //Shoot projectile when shotInterval time has passed
        if (timeSinceLastShot >= shotInterval)
        {
            Debug.Log("Shoot Flicker");
            var projectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();

            // Just setting the velocity allows us to customize the projectile's mass
            rb.velocity = gameObject.transform.up * shotSpeed;
            Destroy(projectile, lifetime);
            return 0;
        }
        //Don't shoot projectile if not enough time has passed
        else
        {
            Debug.Log("Flicker On Cooldown");
            return timeSinceLastShot;
        }
    }
}