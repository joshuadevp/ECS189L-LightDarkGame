using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class FlickerGenericUpgrade : ScriptableObject, IUpgrade
{
    public string upgradeName;
    public Sprite upgradeIcon;

    [Header("Damage Stat Modifiers")]
    [SerializeField] private float damageModifier;
    [SerializeField] [Range(0, 1)] private float critChanceModifier;
    [SerializeField] private float critMultiplierModifier;

    [Header("Characteristic Modifiers")]
    [SerializeField] private float sizeModifier;
    //[SerializeField] private float lightRadiusModifier;

    [Header("Behaviorial Modifiers")]
    [SerializeField] private float projectileLifetimeModifier;
    [SerializeField] private float projectileSpeedModifier;
    [SerializeField] private float shotIntervalModifier;
    [SerializeField] private float knockbackModifier;
    [SerializeField] private float pierceModifier;

    private ModifierType mt = ModifierType.multiplicative;

    public void ApplyUpgrade()
    {
        var player = FindObjectOfType<Player>();
        // Pseudocode for how it could be increased depending on how the damage is stored/implemented
        // player.flickerDamage *= increaseDamageMultiplier;
        // player.flickerDamge.AddModifier(new StatModifier...) other way

        if(damageModifier != 0)
        {
            player.Damage.AddModifier(new StatModifier(
                mt,
                damageModifier,
                "Damage Up"
            )
            );
        }

        if (critChanceModifier != 0)
        {
            player.CritChance.AddModifier(new StatModifier(
                mt,
                critChanceModifier,
                "Crit Chance Up"
            )
            );
        }

        if (critMultiplierModifier != 0)
        {
            player.CritDamage.AddModifier(new StatModifier(
                mt,
                critMultiplierModifier,
                "Crit Damage Up"
            )
            );
        }

        if (sizeModifier != 0)
        {
            player.ProjectileSize.AddModifier(new StatModifier(
                mt,
                sizeModifier,
                "Projectile Size Up"
            )
            );
        }

        if (projectileLifetimeModifier != 0)
        {
            player.ProjectileLifetime.AddModifier(new StatModifier(
                mt,
                projectileLifetimeModifier,
                "Projectile Liftime Up"
            )
            );
        }

        if (projectileSpeedModifier != 0)
        {
            player.ProjectileSpeed.AddModifier(new StatModifier(
                mt,
                projectileSpeedModifier,
                "Projectile Speed Up"
            )
            );
        }

        if (shotIntervalModifier != 0)
        {
            player.ShotInterval.AddModifier(new StatModifier(
                mt,
                shotIntervalModifier,
                "Firerate Up"
            )
            );
        }

        if (knockbackModifier != 0)
        {
            player.ProjectileKnockback.AddModifier(new StatModifier(
                mt,
                knockbackModifier,
                "Knockback Up"
            )
            );
        }

        if (pierceModifier != 0)
        {
            player.ProjectilePierce.AddModifier(new StatModifier(
                mt,
                pierceModifier,
                "Pierce Up"
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
        if (damageModifier != 0)
        {
            if (damageModifier > 1)
                details += "\n+" + (damageModifier - 1) * 100 + "% dmd";
            else
                details += "\n<color=red>" + (damageModifier - 1) * 100 + "% dmg</color>";
        }

        if (sizeModifier != 0)
        {
            if (sizeModifier > 1)
                details += "\n+" + (sizeModifier - 1) * 100 + "% size";
            else
                details += "\n<color=red>" + (sizeModifier - 1) * 100 + "% size</color>";
        }

        if (projectileLifetimeModifier != 0)
        {
            if (projectileLifetimeModifier > 1)
                details += "\n+" + (projectileLifetimeModifier - 1) * 100 + "% dur";
            else
                details += "\n<color=red>" + (projectileLifetimeModifier - 1) * 100 + "% dur</color>";
        }

        if (projectileSpeedModifier != 0)
        {
            if (projectileSpeedModifier > 1)
                details += "\n+" + (projectileSpeedModifier - 1) * 100 + "% vel";
            else
                details += "\n<color=red>" + (projectileSpeedModifier - 1) * 100 + "% vel</color>";
        }

        if (shotIntervalModifier != 0)
        {
            // Different here because lower is faster shots = better
            if (shotIntervalModifier < 1)
                details += "\n" + (shotIntervalModifier - 1) * 100 + "% rate";
            else
                details += "\n<color=red>+" + (shotIntervalModifier - 1) * 100 + "% rate</color>";
        }

        if (knockbackModifier != 0)
        {
            if (knockbackModifier > 1)
                details += "\n+" + (knockbackModifier - 1) * 100 + "% knock";
            else
                details += "\n<color=red>" + (knockbackModifier - 1) * 100 + "% knock</color>";
        }

        if (pierceModifier != 0)
        {
            if (pierceModifier > 1)
                details += "\n+" + (pierceModifier - 1) * 100 + "% pierce";
            else
                details += "\n<color=red>" + (pierceModifier - 1) * 100 + "% pierce</color>";
        }

        return details;
    }

    public float DamageModifier => damageModifier;
    public float CritMultiplierModifier => critMultiplierModifier;
    public float CritChanceModifier => critChanceModifier;
    public float SizeModifier => sizeModifier;
    public float ProjectileLifeTimeModifier => projectileLifetimeModifier;
    public float ProjectileSpeedModifier => projectileSpeedModifier;
    public float ShotIntervalModifier => shotIntervalModifier;
    public float KnockbackModifier => knockbackModifier;
    public float PierceModifier => pierceModifier;
}