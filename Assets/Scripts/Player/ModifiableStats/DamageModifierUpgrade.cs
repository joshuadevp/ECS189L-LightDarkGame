using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModifierUpgrade : ScriptableObject, IUpgrade
{
    [SerializeField] float damageModifierUpgradeModifier;

    public void applyPercentage()
    {
        var playerStats = FindObjectOfType<Player>();
        playerStats.DamageModifier.AddModifier(new StatModifier(ModifierType.multiplicative, damageModifierUpgradeModifier, "Damage Up!"));
    }
}