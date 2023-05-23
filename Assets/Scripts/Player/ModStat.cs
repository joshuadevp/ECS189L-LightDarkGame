using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModStat
{
    [SerializeField] float baseValue;
    List<StatModifier> modifiers;

    bool dirty = true;
    private float finalValue;
    public float Value { get => GetValue(); }

    public ModStat()
    {
        baseValue = 0;
        modifiers = new List<StatModifier>();
    }

    public ModStat(float baseVal) {
        baseValue = baseVal;
        modifiers = new List<StatModifier>();
    }

    public void AddModifier(StatModifier modifier)
    {
        dirty = true;
        modifiers.Add(modifier);
        modifiers.Sort((StatModifier mod1, StatModifier mod2) => mod1.type.CompareTo(mod2.type));
    }

    public void RemoveModifier(StatModifier modifier)
    {
        dirty = true;
        modifiers.Remove(modifier);
    }

    /// <summary>
    /// Add modifier to a stat. Remember to save the returned reference if you plan to remove the modifier later.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public StatModifier AddModifier(ModifierType type, float value)
    {
        dirty = true;
        var modifier = new StatModifier(type, value);
        modifiers.Add(modifier);
        return modifier;
    }

    // Lazy evaluation of the modified stat.
    private float GetValue()
    {
        if (!dirty)
        {
            return finalValue;
        }
        else 
        {
            finalValue = baseValue;
            foreach (StatModifier modifier in modifiers) {
                if (modifier.type == ModifierType.flat)
                {
                    finalValue += modifier.value;
                }
                else if (modifier.type == ModifierType.multiplicative) 
                {
                    finalValue *= modifier.value;
                }
            }
            dirty = false;
            return finalValue;
        }
    }
}
