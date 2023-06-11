using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryShotCommand : PlayerCommand
{
    [SerializeField] private PrimarySpec pc;
    [SerializeField] private GameObject projectilePrefab;

    // Projectile Stats, applied with modifiers.
    private float damage;
    private float criticalChance;
    private float criticalMultiplier;

    private float size;
    private float lightRadius;

    private float projectileLifetime;
    private float projectileSpeed;
    private float shotInterval;
    private float knockback;
    private int pierce;

    float lastCast = 0f;

    private void Awake()
    {
        damage = pc.BaseDamage;
        criticalChance = pc.BaseCriticalChance;
        criticalMultiplier = pc.BaseCriticalMultipler;
        size = pc.BaseSize;
        projectileLifetime = pc.ProjectileLifetime;
        projectileSpeed = pc.ProjectileSpeed;
        shotInterval = pc.ShotInterval;
        knockback = pc.Knockback;
        pierce = pc.Pierce;
    }

    public override void Execute(GameObject player)
    {
        Debug.Log("Firing");
        // Shoot projectile when enough time has passed
        if (Time.time - lastCast > shotInterval)
        {
            Debug.Log("SHOT!!");
            var projectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();

            // Just setting the velocity allows us to customize the projectile's mass
            rb.velocity = gameObject.transform.up * projectileSpeed;
            Destroy(projectile, projectileLifetime);
            lastCast = Time.time;
        }
    }
}
