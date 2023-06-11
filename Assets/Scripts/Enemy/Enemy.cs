using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("In Editor: Base Stats\nIn Game: Final Stats")]
    public EnemyController controller;
    [field: SerializeField] public float MaxHp { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [Tooltip("Enemy's DPS while colliding with player")]
    [field: SerializeField] public float Damage { get; private set; }

    [SerializeField] EnemyHPBar hPBar;
    private float hp;

    // Start is called before the first frame update
    void Start()
    {
        // Possibly modify the hp/speed/damage here by some global modifier such as time/stage
        float modifier = GameManager.instance.CalculateEnemyModifier();
        MaxHp *= modifier;
        Damage *= modifier;
        controller = GetComponent<EnemyController>();
        hPBar = GetComponentInChildren<EnemyHPBar>();
        hp = MaxHp;
    }

    public void HitBy(GameObject projectile, float damage) 
    {
        controller.HitBy(projectile);
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        hPBar.SetHP(hp / MaxHp);
        GameManager.instance.SpawnDamageInfo(transform.position, damage);
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
