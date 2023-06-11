using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterBase baseStats;
    [SerializeField] PrimarySpec ps;
    public ModStat MaxHP { get; private set; }
    public ModStat Speed { get; private set; }

    // Primary Projectile Based Stats
    public ModStat Damage { get; private set; }
    public ModStat CritChance { get; private set; }
    public ModStat CritDamage { get; private set; }
    public ModStat ProjectileSize { get; private set; }
    public ModStat ProjectileLifetime { get; private set; }
    public ModStat ProjectileSpeed { get; private set; }
    public ModStat ShotInterval { get; private set; }
    public ModStat ProjectileKnockback { get; private set; }
    public ModStat ProjectilePierce { get; private set; }
    public ModStat ResourceGainModifier { get; private set; }

    [field: SerializeField] public float Hp { get; private set; }
    [SerializeField] PlayerHPBar hpBar;

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = new ModStat(baseStats.hp);
        Speed = new ModStat(baseStats.speed);

        // Initialize with PrimarySpec base values.
        Damage = new ModStat(ps.BaseDamage);
        CritChance = new ModStat(ps.BaseCriticalChance);   
        CritDamage = new ModStat(ps.BaseCriticalMultipler);
        ProjectileSize = new ModStat(ps.BaseSize);
        ProjectileLifetime = new ModStat(ps.ProjectileLifetime);
        ProjectileSpeed = new ModStat(ps.ProjectileSpeed);
        ShotInterval = new ModStat(ps.ShotInterval);
        ProjectileKnockback = new ModStat(ps.Knockback);
        ProjectilePierce = new ModStat(ps.Pierce);

        ResourceGainModifier = new ModStat(1f);
        Hp = MaxHP.Value;

        Debug.Log(ShotInterval.Value);
    }

    public void TakeDamage(float dmg)
    {
        Hp -= dmg;
        hpBar.SetHP(Hp / MaxHP.Value);
        if (Hp <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        print("Player died");
        Time.timeScale = 0;
    }

    public void AddTemporaryModifier(ModStat moddedStat, StatModifier modifier, float duration)
    {
        StartCoroutine(AddTemporaryModifierCoroutine(moddedStat, modifier, duration));
    }

    private IEnumerator AddTemporaryModifierCoroutine(ModStat moddedStat, StatModifier modifier, float duration)
    {
        moddedStat.AddModifier(modifier);
        yield return new WaitForSeconds(duration);
        moddedStat.RemoveModifier(modifier);
    }
}
