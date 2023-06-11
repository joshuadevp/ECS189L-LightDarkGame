using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGainModifierUpgrade : ScriptableObject, IUpgrade
{
    [SerializeField] float resourceGainModifierUpgradeModifier;

    public void applyUpgrade()
    {
        var playerStats = FindObjectOfType<Player>();
        playerStats.ResourceGainModifier.AddModifier(new StatModifier(ModifierType.multiplicative, resourceGainModifierUpgradeModifier, "Resource Gain Up!"));
    }
}