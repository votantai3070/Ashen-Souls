public enum SkillUpgradeType
{
    None,

    // ------ Passive Ability Upgrades ------
    // Passive Ability Upgrades
    AbsorbSoul,
    AbsorbSoulUpgrade, // Increase soul absorption from defeated enemies by 20%.
    UndyingWill,
    UndyingWillUpgrade, // Reduce cooldown of revival by 20 seconds.
    SoulSiphon,
    SoulSiphonUpgrade, // Increase health stolen by 15% of the damage dealt.
    SpiritLink,
    SpiritLinkUpgrade, // Increase attack power boost by 10% and reduce damage sharing by 10%.

    // ------ Active Ability Upgrades ------
    // Skill Attack Upgrades
    SpinningSword,
    SpinningSwordUpgrade, // Increase damage by 25% and reduce cooldown by 1 second.

    FireSoul,
    FireSoulUpgrade, // Increase damage by 30% and reduce cooldown by 1.5 seconds.

    SoulCleave,
    SoulCleaveUpgrade, // Increase damage by 20% and reduce cooldown by 1 second.
    SpriritArrow,
    SpriritArrowUpgrade, // Increase damage by 25% and reduce cooldown by 1 second.
    SoulBurst,
    SoulBurstUpgrade, // Increase damage by 30% and reduce cooldown by 1.5 seconds.
    DeathDash,
    DeathDashUpgrade, // Increase damage by 20% and reduce cooldown by 1 second.

    // Skill Buff Upgrades
    SoulRage,
    SoulRageUpgrade, // Increase attack speed and damage boost by 15%, but further reduce defense by 5%.
    WraithSpeed,
    WraithSpeedUpgrade, // Increase movement speed and evasion boost by 20%, but further reduce attack power by 5%.
    SoulShield,
    SoulShieldUpgrade, // Increase shield strength by 25% and reduce cooldown by 1 second, but further reduce movement speed by 5%.
    DarkVitality,
    DarkVitalityUpgrade, // Increase maximum health and health regeneration boost by 20%, but further reduce attack speed by 5%.
    DeathArmor,
    DeathArmorUpgrade, // Increase damage reduction by 30% and reduce cooldown by 1.5 seconds, but further reduce movement speed and attack power by 5%.

    // Skill Defense Upgrades
    BlockSoul,
    BlockSoulUpgrade, // Increase block success rate by 15% and increase counterattack damage by 20%.
    ParrySoul,
    ParrySoulUpgrade, // Increase parry success rate by 20% and increase stun duration by 0.5 seconds.
    DodgeSoul,
    DodgeSoulUpgrade, // Increase dodge success rate by 20% and increase invulnerability duration by 0.5 seconds.
    ShieldSoul,
    ShieldSoulUpgrade, // Increase shield strength by 25% and increase reflected damage by 15%.

    // Ultimate Skill Upgrades
    BlackHole,
    BlackHoleUpgrade, // Increase damage over time by 30% and increase pull strength by 20%.
    SoulEruption,
    SoulEruptionUpgrade, // Increase damage by 35% and increase stun duration by 1 second.


    Damage,
    Speed,
    Size,
    Cooldown,
}
