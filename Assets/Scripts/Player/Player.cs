using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterBase baseStats;
    public ModStat MaxHP { get; private set; }
    public ModStat Speed { get; private set; }
    public ModStat CritChance { get; private set; }
    public ModStat CritDamage { get; private set; }
    public ModStat DamageModifier { get; private set; }
    public ModStat ResourceGainModifier { get; private set; }

    [field: SerializeField] public float Hp { get; private set; }
    [SerializeField] PlayerHPBar hpBar;

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = new ModStat(baseStats.hp);
        Speed = new ModStat(baseStats.speed);
        CritChance = new ModStat(0f);   
        CritDamage = new ModStat(2f);
        DamageModifier = new ModStat(1f);
        ResourceGainModifier = new ModStat(1f);
        Hp = MaxHP.Value;
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
