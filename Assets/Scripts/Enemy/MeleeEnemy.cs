using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class MeleeEnemy : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Enemy>();

        // Consider refractoring if we end up with a global reference of player
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * main.speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            player.main.TakeDamage(main.damage);
        }
    }
}
