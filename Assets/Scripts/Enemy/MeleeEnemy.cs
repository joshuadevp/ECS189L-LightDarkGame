using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class MeleeEnemy : EnemyController
{
    // Modify this constant or modify the value in HitBy() to change stun/knockback duration
    private const float StunDurationOnHit = 0.2f;
    Rigidbody rb;
    Vector3 knockbackMovement;
    bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
        // Consider refractoring if we end up with a global reference of player
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
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

    public override void HitBy(GameObject projectile) 
    {
        StartCoroutine(Stun(StunDurationOnHit));
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        knockbackMovement = projectileRb.mass/rb.mass * projectileRb.velocity;
    }

    public IEnumerator Stun(float duration) 
    {
        stunned = true;
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.main.TakeDamage(main.Damage * Time.deltaTime);
            print($"Player took {main.Damage} damage");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.main.TakeDamage(main.Damage * Time.deltaTime);
            print($"Player took {main.Damage} damage");
        }
    }
}
