using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("In Editor: Base Stats\nIn Game: Final Stats")]
    public EnemyController controller;
    [field: SerializeField] public float hp { get; private set; }
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float damage { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Possibly modify the hp/speed/damage here by some global modifier such as time/stage
        // float modifier = GameManager.instance.CalculateEnemyModifier();
        // hp *= modifier;
        // damage *= modifier;
        hp = 100;
        damage = 1;
        controller = GetComponent<EnemyController>();
    }

    public void HitBy(GameObject projectile, float damage) 
    {
        controller.HitBy(projectile);
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnDeath();
        }
    }

    // Do something when dying
    void OnDeath()
    {
        Destroy(gameObject);
    }
}
