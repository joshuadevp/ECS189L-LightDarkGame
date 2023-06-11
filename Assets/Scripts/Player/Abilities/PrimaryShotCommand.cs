using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryShotCommand : PlayerCommand
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Player playerScript;

    float lastCast = 0;

    public override void Execute(GameObject player)
    {
        //var playerScript = player.GetComponent<Player>();
        //Debug.Log("here");
        // Shoot projectile when enough time has passed
        if (Time.time - lastCast > playerScript.ShotInterval.Value)
        {
            //Debug.Log("SHOT!!");
            var projectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
            var rb = projectile.GetComponent<Rigidbody>();

            // Just setting the velocity allows us to customize the projectile's mass
            rb.velocity = gameObject.transform.up * playerScript.ProjectileSpeed.Value;
            Destroy(projectile, playerScript.ProjectileLifetime.Value);
            lastCast = Time.time;
        }
    }
}
