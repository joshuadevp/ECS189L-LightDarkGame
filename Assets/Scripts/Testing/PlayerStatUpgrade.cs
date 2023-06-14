using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class PlayerStatUpgrade : ScriptableObject, IUpgrade
{
    public string upgradeName;
    public Sprite upgradeIcon;

    [SerializeField]
    float healthModifier;
    [SerializeField]
    float speedModifier;
    [SerializeField]
    float resourceModifier;

    public void ApplyUpgrade()
    {
        var player = FindObjectOfType<Player>();
        
        if(healthModifier != 0)
        {
            player.MaxHP.AddModifier(new StatModifier(
            ModifierType.multiplicative,
            healthModifier,
            "Health Up"
            )
            );
            player.Heal(0);
        }

        if (speedModifier != 0)
        {
            player.Speed.AddModifier(new StatModifier(
            ModifierType.multiplicative,
            speedModifier,
            "Player SPD Up"
            )
            );
        }

        if (resourceModifier != 0)
        {
            player.ResourceGainModifier.AddModifier(new StatModifier(
            ModifierType.multiplicative,
            resourceModifier,
            "Resource Gain Up"
            )
            );
        }
    }

    public Sprite GetIcon()
    {
        return upgradeIcon;
    }

    public string GetDetails()
    {
        var details = "[" + upgradeName + "]";
        if (healthModifier != 0)
        {
            if (healthModifier > 1)
                details += "\n+" + (healthModifier - 1) * 100 + "% health";
            else
                details += "\n<color=red>" + (healthModifier - 1) * 100 + "% health</color>";
        }
        
        if (speedModifier != 0)
        {
            if (speedModifier > 1)
                details += "\n+" + (speedModifier - 1) * 100 + "% movespd";
            else
                details += "\n<color=red>" + (speedModifier - 1) * 100 + "% movespd</color>";
        }

        if (resourceModifier != 0)
        {
            if (resourceModifier > 1)
                details += "\n+" + (resourceModifier - 1) * 100 + "% RESOURCE";
            else
                details += "\n<color=red>" + (resourceModifier - 1) * 100 + "% P RESOURCE</color>";
        }

        return details;
    }
}
