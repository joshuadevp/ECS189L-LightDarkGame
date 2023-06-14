using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryCandleCommand : MonoBehaviour, IAbilityCommand
{
    private GameObject projectilePrefab;

    private float baseSize;
    private float damageMultiplier = 1.2f;
    private float shotSpeed = 20.0f;
    private float shotInterval = 1.5f;
    private float lifetime = 2.0f;

    private float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    //private float timeSinceLastShot = 0;

    private void Awake()
    {
        projectilePrefab = FindObjectOfType<ProjectileRegistry>().GetProjectile(1);
    }
    public float Execute(GameObject gameObject, float timeSinceLastShot)
    {
        //timeSinceLastShot += Time.deltaTime;

        //Shoot projectile when shotInterval time has passed
        if (timeSinceLastShot >= shotInterval)
        {
            Debug.Log("Shoot ECandle");
            var projectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
            //var rb = projectile.GetComponent<Rigidbody>();
            //rb.AddForce(gameObject.transform.up * shotSpeed, ForceMode.Impulse);
            StartCoroutine(TravelToMouse(projectile));
            return 0;
        }
        //Don't shoot projectile if not enough time has passed
        else
        {
            Debug.Log("ECandle On Cooldown");
            return timeSinceLastShot;
        }
    }

    public IEnumerator TravelToMouse(GameObject projectile)
    {
        var mousePosition = FindObjectOfType<MouseScreenController>().transform.position;
        while(projectile.transform.position != mousePosition)
        {
            projectile.transform.position = Vector3.SmoothDamp(projectile.transform.position, mousePosition, ref velocity, smoothTime);
            yield return null;
        }
        Destroy(projectile);
    }
}