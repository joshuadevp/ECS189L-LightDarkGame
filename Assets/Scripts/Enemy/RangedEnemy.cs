using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MeleeEnemy
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileDamage;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifetime;
    [SerializeField] float firerate;

    protected new void Start()
    {
        base.Start();
        InvokeRepeating("SpawnProjectile", 0.5f, 1/firerate);
    }

    void Update()
    {
        if (stunned)
        {
            transform.position += (knockbackMovement * Time.deltaTime);
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += (direction * main.Speed * Time.deltaTime);
    }

    void SpawnProjectile() 
    {
        var projectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
        projectile.GetComponent<EnemyProjectile>().SetDamage(projectileDamage);
        var rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
        Destroy(projectile, projectileLifetime);
    }
}
