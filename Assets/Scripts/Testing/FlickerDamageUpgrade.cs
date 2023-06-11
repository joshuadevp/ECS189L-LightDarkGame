using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FlickerCommandUpgrade : ScriptableObject, IUpgradeTest
{
    [SerializeField]
    float increaseDamageMultiplier;

    public void ApplyUpgrade()
    {
        var player = FindObjectOfType<Player>();
        // Pseudocode for how it could be increased depending on how the damage is stored/implemented
        // player.flickerDamage *= increaseDamageMultiplier;
        // player.flickerDamge.AddModifier(new StatModifier...) other way
    }
}