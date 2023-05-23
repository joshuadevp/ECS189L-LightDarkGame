using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterBase baseStats;
    private ModStat maxHP;
    private ModStat speed;

    [field: SerializeField] public float Hp { get; private set; }
    public float Speed { get => speed.Value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHP = new ModStat(baseStats.hp);
        speed = new ModStat(baseStats.speed);
        Hp = maxHP.Value;
    }

    public void TakeDamage(float dmg) 
    {
        Hp -= dmg;
    }
}
