using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritDamageUpgrade : ScriptableObject, IUpgrade
{
    [SerializeField] float critDamageUpgradeModifier;

    public void applyPercentage()
    {
        var playerStats = FindObjectOfType<Player>();
        playerStats.CritDamage.AddModifier(new StatModifier(ModifierType.multiplicative, critDamageUpgradeModifier, "Crit Damage Up!"));
    }
}