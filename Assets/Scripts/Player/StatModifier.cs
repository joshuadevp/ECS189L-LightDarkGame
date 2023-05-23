using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType { flat, multiplicative }
public class StatModifier
{
    public ModifierType type { get; private set; }
    public float value { get; private set; }

    public StatModifier(ModifierType modType, float val) {
        type = modType;
        value = val;
    }
}
