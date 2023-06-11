using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : ScriptableObject, IUpgrade
{
    [SerializeField] float healthUpgradeModifier;

    public void applyUpgrade()
    {
        var playerStats = FindObjectOfType<Player>();
        playerStats.MaxHP.AddModifier(new StatModifier(ModifierType.multiplicative, healthUpgradeModifier, "Health Up!"));
    }
}