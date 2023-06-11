using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectileCollisionDamage : MonoBehaviour
{
    FogOfDarknessManager darknessManager;
    private int hitCounter = 0;
    // Start is called before the first frame update
    void Awake()
    {
        darknessManager = GameObject.FindObjectOfType<FogOfDarknessManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!darknessManager)
        {
            darknessManager = GameObject.FindObjectOfType<FogOfDarknessManager>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Darkness")
        {
            darknessManager.DamageDarkness(other.transform.position, 200);
            hitCounter++;
        }
        else if (hit.tag == "Enemy")
        {
            hitCounter++;
            hit.GetComponent<Enemy>().HitBy(this.gameObject, 10);
        }
        if (hitCounter == 10)
        {
            Destroy(this.gameObject);
        }
    }
}
