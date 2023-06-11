using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritChancehUpgrade : ScriptableObject, IUpgrade
{
    [SerializeField] float critChanceUpgradeModifier;

    public void applyUpgrade()
    {
        var playerStats = FindObjectOfType<Player>();
        playerStats.CritChance.AddModifier(new StatModifier(ModifierType.multiplicative, critChanceUpgradeModifier, "Crit Chance Up!"));
    }
}