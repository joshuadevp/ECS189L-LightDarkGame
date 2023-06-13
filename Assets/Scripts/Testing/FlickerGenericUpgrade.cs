using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void ApplyUpgrade()
    {
        var player = FindObjectOfType<Player>();
        // Pseudocode for how it could be increased depending on how the damage is stored/implemented
        // player.flickerDamage *= increaseDamageMultiplier;
        // player.flickerDamge.AddModifier(new StatModifier...) other way

        if(damageModifier != 0)
        {
            player.Damage.AddModifier(new StatModifier(
                ModifierType.flat,
                damageModifier,
                "Damage Up"
            )
            );
        }

        if (critChanceModifier != 0)
        {
            player.CritChance.AddModifier(new StatModifier(
                ModifierType.flat,
                critChanceModifier,
                "Crit Chance Up"
            )
            );
        }

        if (critMultiplierModifier != 0)
        {
            player.CritDamage.AddModifier(new StatModifier(
                ModifierType.flat,
                critMultiplierModifier,
                "Crit Damage Up"
            )
            );
        }

        if (sizeModifier != 0)
        {
            player.ProjectileSize.AddModifier(new StatModifier(
                ModifierType.flat,
                sizeModifier,
                "Projectile Size Up"
            )
            );
        }

        if (projectileLifetimeModifier != 0)
        {
            player.ProjectileLifetime.AddModifier(new StatModifier(
                ModifierType.flat,
                projectileLifetimeModifier,
                "Projectile Liftime Up"
            )
            );
        }

        if (projectileSpeedModifier != 0)
        {
            player.ProjectileSpeed.AddModifier(new StatModifier(
                ModifierType.flat,
                projectileSpeedModifier,
                "Projectile Speed Up"
            )
            );
        }

        if (shotIntervalModifier != 0)
        {
            player.ShotInterval.AddModifier(new StatModifier(
                ModifierType.flat,
                shotIntervalModifier,
                "Firerate Up"
            )
            );
        }

        if (knockbackModifier != 0)
        {
            player.ProjectileKnockback.AddModifier(new StatModifier(
                ModifierType.flat,
                knockbackModifier,
                "Knockback Up"
            )
            );
        }

        if (pierceModifier != 0)
        {
            player.ProjectilePierce.AddModifier(new StatModifier(
                ModifierType.flat,
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
        return "[" + upgradeName + "]"
             + "\n+" + damageModifier + " dmg"
             + "\n+" + sizeModifier + " size"
             + "\n+" + projectileLifetimeModifier + " dur"
             + "\n+" + projectileSpeedModifier + " vel"
             + "\n+" + shotIntervalModifier + " rate"
             + "\n+" + knockbackModifier + " knock"
             + "\n+" + pierceModifier + " pierce";
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