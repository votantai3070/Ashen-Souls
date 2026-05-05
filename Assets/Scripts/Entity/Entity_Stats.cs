using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SO defaultStatSetup;

    [Header("Stat")]
    public Stat_ResourceGroup resource;
    public Stat_MajorGroup major;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

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

    public float GetPhysicalDamage(out bool isCriticalHit, float scaleFactor = 1)
    {
        float baseDamage = GetBasePhysicalDamage();
        float baseCritChance = GetCritChance();
        float baseCritDamage = GetCritDamage();

        isCriticalHit = Random.Range(0, 100) < baseCritChance;
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
        float evasionCap = 0.85f; // Cap evasion at 85%

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resource.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5; // Assuming each point of vitality gives 5 additional health

        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    // ===================== SKILL DAMAGE =====================

    /// <summary>
    /// Tính damage cho skill với scaleFactor riêng mỗi skill
    /// scaleFactor = 1.5 → damage bằng 150% base damage
    /// Normal × (1 + upgradeBonus%)
    /// </summary>
    public float GetSkillDamage(SkillUpgradeType skillType, out bool isCriticalHit)
    {
        float scaleFactor = GetSkillScaleFactor(skillType);
        return GetPhysicalDamage(out isCriticalHit, scaleFactor);
    }

    private float GetSkillScaleFactor(SkillUpgradeType skillType)
    {
        switch (skillType)
        {
            // ------ Skill Attack ------
            case SkillUpgradeType.SpinningSword: return 1.4f;
            case SkillUpgradeType.SpinningSwordUpgrade: return 1.75f; // +25% damage

            case SkillUpgradeType.FireSoul: return 1.5f;
            case SkillUpgradeType.FireSoulUpgrade: return 1.95f; // +30% damage

            case SkillUpgradeType.SoulCleave: return 1.3f;
            case SkillUpgradeType.SoulCleaveUpgrade: return 1.56f; // +20% damage

            case SkillUpgradeType.SpriritArrow: return 1.6f;
            case SkillUpgradeType.SpriritArrowUpgrade: return 2.0f;  // +25% damage

            case SkillUpgradeType.SoulBurst: return 1.8f;
            case SkillUpgradeType.SoulBurstUpgrade: return 2.34f; // +30% damage

            case SkillUpgradeType.DeathDash: return 1.2f;
            case SkillUpgradeType.DeathDashUpgrade: return 1.44f; // +20% damage

            // ------ Ultimate ------
            case SkillUpgradeType.BlackHole: return 2.5f;
            case SkillUpgradeType.BlackHoleUpgrade: return 3.25f; // +30% DoT

            case SkillUpgradeType.SoulEruption: return 3.0f;
            case SkillUpgradeType.SoulEruptionUpgrade: return 4.05f; // +35% damage

            default: return 1.0f;
        }
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
            case StatType.FireResistance:
                return defense.fireResistance;
            case StatType.IceResistance:
                return defense.iceResistance;
            case StatType.LightningResistance:
                return defense.lightninghResistance;
            case StatType.Damage:
                return offense.damage;
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
}
