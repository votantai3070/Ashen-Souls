public class UI_CharacterStatSlot : UI_StatSlot
{
    public void UpdateStatValue(Stat_DataSO defaultStatData)
    {
        float value = 0;

        // Get the character's stat value from the defaultStatData
        switch (statSlotType)
        {
            // Major stats
            case StatType.Strength:
                value = defaultStatData.strength;
                break;
            case StatType.Agility:
                value = defaultStatData.agility;
                break;
            case StatType.Intelligence:
                value = defaultStatData.intelligence;
                break;
            case StatType.Vitality:
                value = defaultStatData.vitality;
                break;

            // Offense stats
            case StatType.Damage:
                value = defaultStatData.damage;
                break;
            case StatType.CriticalChance:
                value = defaultStatData.critChance;
                break;
            case StatType.CriticalDamage:
                value = defaultStatData.critDamage;
                break;
            case StatType.ArmorReduction:
                value = defaultStatData.armorReduction;
                break;
            case StatType.AttackSpeed:
                value = defaultStatData.attackSpeed * 100;
                break;
            case StatType.Speed:
                value = defaultStatData.speed;
                break;

            // Defense stats
            case StatType.MaxHealth:
                value = defaultStatData.maxHealth;
                break;
            case StatType.RegenHealth:
                value = defaultStatData.healthRegen;
                break;
            case StatType.Evasion:
                value = defaultStatData.evasion * 100;
                break;
            case StatType.Armor:
                value = defaultStatData.armor;
                break;
        }

        statValue.text = IsPercentageStat(statSlotType) ? $"{value:0.#}%" : $"{value:0.#}";
    }
}
