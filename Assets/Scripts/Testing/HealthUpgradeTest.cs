using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthUpgradeTest : ScriptableObject, IUpgradeTest
{
    [SerializeField]
    float healthMultiplier;
    public void ApplyUpgrade()
    {
        var player = FindObjectOfType<Player>();
        
        player.MaxHP.AddModifier(new StatModifier(
            ModifierType.multiplicative,
            healthMultiplier,
            "Test health multiply"
        )
        );
    }
}
