using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    float damage;
    public void SetDamage(float dmg) 
    {
        damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
