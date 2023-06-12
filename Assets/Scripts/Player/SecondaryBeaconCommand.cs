using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryBeaconCommand : MonoBehaviour, IAbilityCommand
{
    private GameObject projectilePrefab;

    [SerializeField] float decoyLifetime = 2.0f;
    [SerializeField] float decoyCooldownTimer = 5.0f;
    private float timeSinceLastShot = 0;

    private void Awake()
    {
        projectilePrefab = FindObjectOfType<ProjectileRegistry>().GetProjectile(1);
        timeSinceLastShot = 0.0f;
    }
    public float Execute(GameObject gameObject, float timeSinceLastShot)
    {
        timeSinceLastShot += Time.deltaTime;

        //Shoot projectile when shotInterval time has passed
        if (timeSinceLastShot >= decoyCooldownTimer)
        {
            Debug.Log("Place Beacon");
            var mousePosition = FindObjectOfType<MouseScreenController>().transform.position;
            var projectile = (GameObject)Instantiate(projectilePrefab, mousePosition, Quaternion.identity);
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
        
        while(projectile.transform.position != mousePosition)
        {
            projectile.transform.position = Vector3.SmoothDamp(projectile.transform.position, mousePosition, ref velocity, smoothTime);
            yield return null;
        }
        Destroy(projectile);
    }
}