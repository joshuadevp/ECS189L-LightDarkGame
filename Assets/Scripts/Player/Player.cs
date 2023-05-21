using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterBase baseStats;
    private float maxHP;
    private float hp;
    private float speed;

    public float Speed { get => speed; private set => speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxHP = baseStats.hp;
        hp = maxHP;
        speed = baseStats.speed;
    }

    public void TakeDamage(float dmg) 
    {
        hp -= dmg;
    }
}
