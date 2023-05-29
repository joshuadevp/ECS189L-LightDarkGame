using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterBase baseStats;
    public ModStat MaxHP { get; private set; }
    public ModStat Speed { get; private set; }

    [field: SerializeField] public float Hp { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MaxHP = new ModStat(baseStats.hp);
        Speed = new ModStat(baseStats.speed);
        Hp = MaxHP.Value;
    }

    public void TakeDamage(float dmg)
    {
        Hp -= dmg;
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
