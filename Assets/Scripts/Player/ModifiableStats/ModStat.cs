using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

// Followed and modified from the tutorial by Kryzarel on the Unity forum at
// https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/

[System.Serializable]
public class ModStat
{
    [SerializeField] float baseValue;
    List<StatModifier> modifiers;

    bool dirty = true;
    private float finalValue;
    public float Value { get => GetValue(); }

    public ModStat() : this(0)
    {
    }

    public ModStat(float baseVal)
    {
        baseValue = baseVal;
        modifiers = new List<StatModifier>();
    }

    public void SetBaseValue(float baseVal)
    {
        baseValue = baseVal;
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
    public StatModifier AddModifier(ModifierType type, float value, string name = "")
    {
        dirty = true;
        var modifier = new StatModifier(type, value, name);
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
            float finalMultAddValue = 1;
            foreach (StatModifier modifier in modifiers)
            {
                if (modifier.type == ModifierType.flat)
                {
                    finalValue += modifier.value;
                }
                else if (modifier.type == ModifierType.multAdd)
                {
                    finalMultAddValue += modifier.value;
                }
                else if (modifier.type == ModifierType.multiplicative)
                {
                    finalValue *= modifier.value;
                }
            }
            finalValue *= finalMultAddValue;

            dirty = false;
            return finalValue;
        }
    }

    public override string ToString()
    {
        string str = $"Base: {baseValue}\n";
        foreach (StatModifier modifier in modifiers)
        {
            str += $"{modifier.modifierName}: ";
            switch (modifier.type)
            {
                case ModifierType.flat:
                    str += $"+{modifier.value}\n";
                    break;
                case ModifierType.multAdd:
                    str += $"+{modifier.value * 100}%\n";
                    break;
                case ModifierType.multiplicative:
                    // \u00D7 = multiplication sign
                    str += $"{modifier.value * 100}%\n";
                    break;
            }
        }
        str += $"= {Value}\n";
        return str;
    }
}
