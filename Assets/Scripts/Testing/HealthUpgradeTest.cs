using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthUpgradeTest : ScriptableObject, IUpgrade
{
    [SerializeField]
    float healthMultiplier;
    [SerializeField]
    Sprite upgradeIcon;
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

    public Sprite GetIcon()
    {
        return upgradeIcon;
    }

    public string GetDetails()
    {
        return "Health lol";
    }
}
