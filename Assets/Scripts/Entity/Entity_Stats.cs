using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public event Action OnStatChanged;

    public Stat_SO defaultStatSetup;

    [Header("Stat")]
    public Stat_ResourceGroup resource;
    public Stat_MajorGroup major;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

    private void Start()
    {
        //ApplyDefaultStatSetup();
    }

    //public float GetElementalDamage(out ElementType element, float scaleFactor)
    //{
    //    float fireDamage = offense.fireDamage.GetValue();
    //    float iceDamage = offense.iceDamage.GetValue();
    //    float lightningDamage = offense.lightningDamage.GetValue();
    //    float elementalDamageBonus = major.intelligence.GetValue() * 1f; // Assuming each point of INT gives 1 additional elemental damage

    //    float hightestElementalDamage = fireDamage;
    //    element = ElementType.Fire;

    //    if (iceDamage > hightestElementalDamage)
    //    {
    //        hightestElementalDamage = iceDamage;
    //        element = ElementType.Ice;
    //    }

    //    if (lightningDamage > hightestElementalDamage)
    //    {
    //        hightestElementalDamage = lightningDamage;
    //        element = ElementType.Lightning;
    //    }

    //    if (hightestElementalDamage <= 0)
    //    {
    //        element = ElementType.None;
    //        return 0;
    //    }

    //    float bonusFire = element == ElementType.Fire ? 0 : fireDamage * 0.5f; // Weaker elements contribute 50% of their damage as bonus
    //    float bonusIce = element == ElementType.Ice ? 0 : iceDamage * 0.5f;
    //    float bonusLightning = element == ElementType.Lightning ? 0 : lightningDamage * 0.5f;

    //    float weakerElementalDamageBonus = bonusFire + bonusIce + bonusLightning;

    //    float totalElementalDamage = hightestElementalDamage + weakerElementalDamageBonus + elementalDamageBonus;

    //    return totalElementalDamage * scaleFactor;
    //}

    public float GetSpeed()
    {
        float baseSpeed = offense.speed.GetValue();
        float bonusSpeed = major.agility.GetValue() * .5f; // Bonus speed from AGI: +0.5 per AGI

        float finalSpeed = baseSpeed + bonusSpeed;

        return finalSpeed;
    }

    public float GetPhysicalDamage(out bool isCriticalHit, float scaleFactor = 1)
    {
        float baseDamage = GetBasePhysicalDamage();
        float baseCritChance = GetCritChance();
        float baseCritDamage = GetCritDamage();

        isCriticalHit = UnityEngine.Random.Range(0, 100) < baseCritChance;
        float finalDamage = isCriticalHit ? baseDamage * baseCritDamage : baseDamage;

        return finalDamage * scaleFactor;
    }

    // Bonus damage from Strength: +1 per STR
    public float GetBasePhysicalDamage() => offense.damage.GetValue() + major.strength.GetValue();
    // Assuming each point of AGI gives 0.3% additional crit chance
    public float GetCritChance() => offense.critChance.GetValue() + (major.agility.GetValue() * .3f);
    // Assuming each point of STR gives 0.5% additional crit damage
    public float GetCritDamage() => offense.critDamage.GetValue() + (major.strength.GetValue() * .5f);

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f;

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 0.85f;

        return Mathf.Clamp(totalEvasion, 0f, evasionCap);
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = GetBaseArmor();

        float reductionMultiplier = Mathf.Clamp(1f - armorReduction, 0, 1); // Apply armor reduction to the total armor
        float effectiveArmor = baseArmor * reductionMultiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100); // Percentage damage reduction formula
        float mitigationCap = 0.85f; // Cap mitigation at 85%

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    // Assuming each point of vitality gives 1 additional armor
    public float GetBaseArmor() => defense.armor.GetValue() + major.vitality.GetValue();
    public float GetArmorReduction()
    {
        float finalArmorReduction = offense.armorReduction.GetValue() / 100; // Convert percentage to decimal

        return finalArmorReduction;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resource.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5; // Assuming each point of vitality gives 5 additional health

        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    /// <summary>
    /// Calculate skill damage using a separate ScaleFactor for each skill.
    /// scaleFactor = 1.5 → damage equal to 150% base damage
    /// Normal × (1 + upgradeBonus%)
    /// </summary>
    public float GetSkillDamage(SkillUpgradeType skillType, out bool isCriticalHit)
    {
        float scaleFactor = GetSkillScaleFactor(skillType);
        return GetPhysicalDamage(out isCriticalHit, scaleFactor);
    }

    private static readonly Dictionary<SkillUpgradeType, float> skillScaleFactors = new()
{
    // ------ Skill Attack ------
    { SkillUpgradeType.SpinningSword,        1.40f },
    { SkillUpgradeType.SpinningSwordUpgrade, 1.75f },

    { SkillUpgradeType.FireSoul,             1.50f },
    { SkillUpgradeType.FireSoulUpgrade,      1.95f },

    //{ SkillUpgradeType.SoulCleave,           1.30f },
    //{ SkillUpgradeType.SoulCleaveUpgrade,    1.56f },

    //{ SkillUpgradeType.SpriritArrow,         1.60f },
    //{ SkillUpgradeType.SpriritArrowUpgrade,  2.00f },

    { SkillUpgradeType.SoulBurst,            1.80f },
    { SkillUpgradeType.SoulBurstUpgrade,     2.34f },

    { SkillUpgradeType.DeathDash,            1.20f },
    { SkillUpgradeType.DeathDashUpgrade,     1.44f },

    // ------ Ultimate ------
    { SkillUpgradeType.BlackHole,            2.50f },
    { SkillUpgradeType.BlackHoleUpgrade,     3.25f },

    { SkillUpgradeType.SoulEruption,         3.00f },
    { SkillUpgradeType.SoulEruptionUpgrade,  4.05f },
};

    private float GetSkillScaleFactor(SkillUpgradeType skillType)
    {
        return skillScaleFactors.TryGetValue(skillType, out float factor) ? factor : 1.0f;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                return resource.maxHealth;
            case StatType.RegenHealth:
                return resource.regenHealth;
            case StatType.Strength:
                return major.strength;
            case StatType.Agility:
                return major.agility;
            case StatType.Intelligence:
                return major.intelligence;
            case StatType.Vitality:
                return major.vitality;
            case StatType.Armor:
                return defense.armor;
            case StatType.Evasion:
                return defense.evasion;
            //case StatType.FireResistance:
            //    return defense.fireResistance;
            //case StatType.IceResistance:
            //    return defense.iceResistance;
            //case StatType.LightningResistance:
            //    return defense.lightninghResistance;
            case StatType.Damage:
                return offense.damage;
            case StatType.Speed:
                return offense.speed;
            case StatType.CriticalChance:
                return offense.critChance;
            case StatType.CriticalDamage:
                return offense.critDamage;
            case StatType.AttackSpeed:
                return offense.attackSpeed;
            case StatType.ArmorReduction:
                return offense.armorReduction;
            //case StatType.FireDamage:
            //    return offense.fireDamage;
            //case StatType.IceDamage:
            //    return offense.iceDamage;
            //case StatType.LightningDamage:
            //    return offense.lightningDamage;
            default:
                Debug.LogWarning("Stat type not found: " + type);
                return null;
        }
    }

    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.LogWarning("Default stat setup is not assigned.");
            return;
        }

        // Default resource stats
        resource.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resource.regenHealth.SetBaseValue(defaultStatSetup.healthRegen);

        // Default offense stats
        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.speed.SetBaseValue(defaultStatSetup.speed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critDamage.SetBaseValue(defaultStatSetup.critDamage);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
        //offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        //offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        //offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        // Default defense stats
        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);
        //defense.fireResistance.SetBaseValue(defaultStatSetup.fireResistance);
        //defense.iceResistance.SetBaseValue(defaultStatSetup.iceResistance);
        //defense.lightninghResistance.SetBaseValue(defaultStatSetup.lightningResistance);

        // Default major stats
        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);
    }

    public void OnStatChangedInvoke() => OnStatChanged?.Invoke();
}
