using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : ScriptableObject, IUpgrade
{
    [SerializeField] float speedUpgradeModifier;

    public void applyUpgrade()
    {
        var playerStats = FindObjectOfType<Player>();
        playerStats.Speed.AddModifier(new StatModifier(ModifierType.multiplicative, speedUpgradeModifier, "Speed Up!"));
    }
}