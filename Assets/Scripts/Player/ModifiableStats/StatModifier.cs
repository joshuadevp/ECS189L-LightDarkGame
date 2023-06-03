using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { flat, multAdd, multiplicative }

[System.Serializable]
public class StatModifier
{
    [field: SerializeField] public ModifierType type { get; private set; }
    [field: SerializeField] public float value { get; private set; }
    [field: SerializeField] public string modifierName { get; private set; }

    public StatModifier() : this(ModifierType.flat, 0, "")
    {

    }

    public StatModifier(ModifierType modType, float val, string name)
    {
        type = modType;
        value = val;
        modifierName = name;
    }
}
