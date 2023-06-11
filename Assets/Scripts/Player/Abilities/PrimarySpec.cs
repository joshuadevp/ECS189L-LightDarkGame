using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightDarkGame;

[CreateAssetMenu(fileName = "PrimarySpec", menuName = "Ability/PrimarySpec")]
public class PrimarySpec : ScriptableObject
{
    //[SerializeField] private AbilityTypes abilityType;

    [Header("Damage Stat Settings")]
    [SerializeField] private float baseDamage;
    [SerializeField] [Range(0, 1)] private float baseCriticalChance;
    [SerializeField] private float baseCriticalMultiplier;

    [Header("Characteristic Settings")]
    [SerializeField] private float baseSize;
    [SerializeField] private float lightRadius;

    [Header("Behaviorial Settings")]
    [SerializeField] private float projectileLifetime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shotInterval;
    [SerializeField] private float knockback;
    [SerializeField] private int pierce;

    // Getter functions for the values.
    public float BaseDamage => baseDamage;

    public float BaseCriticalChance => baseCriticalChance;

    public float BaseCriticalMultipler => baseCriticalMultiplier;

    public float BaseSize => baseSize;

    public float LightRadius => lightRadius;

    public float ProjectileLifetime => projectileLifetime;

    public float ShotInterval => shotInterval;

    public float ProjectileSpeed => projectileSpeed;

    public float Knockback => knockback;

    public int Pierce => pierce;



}
