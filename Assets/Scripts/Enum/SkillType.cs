public enum SkillType
{
    None,

    // ------ Passive Ability ------
    // Passive Abilities
    AbsorbSoul, // Absorb souls from defeated enemies.
    UndyingWill, // Revive with 1 HP when taking fatal damage, cooldown: 120s
    SoulSiphon, // Steal a portion of the enemy's health with each attack.
    SpiritLink, // Link with a soul, sharing damage taken and increasing attack power.

    // ------ Active Abilities ------
    // Card Skill Attacks
    SpinningSword, // A powerful spinning attack that hits all nearby enemies.
    FireSoul, // Unleash a fiery soul blast that damages enemies in a line.
    FireBall, // Summon a fireball that explodes on impact, dealing area damage.
    SoulSlashSword, // Slash forward with a soul-infused sword, dealing damage to enemies in the path and applying a bleed effect.
    SpriritArrow, // Shoot a spirit arrow that pierces through enemies, dealing damage and applying a debuff.
    SoulBurst, // Unleash a burst of soul energy around the character, damaging and knocking back enemies.
    DeathDash, // Dash forward, dealing damage to enemies in the path and leaving a trail of soul energy that damages enemies over time.

    // Skill Defenses
    BlockSoul, // Block incoming attacks and counter with a soul strike, dealing damage to the attacker.
    ParrySoul, // Parry an incoming attack, stunning the attacker and allowing for a counterattack.
    DodgeSoul, // Dodge an incoming attack, becoming briefly invulnerable and countering with a soul strike.
    ShieldSoul, // Create a temporary shield that absorbs damage and reflects a portion of it back to the attacker.

    // Ultimate Skills
    BlackHole, // Create a black hole that pulls in enemies and deals damage over time.
    SoulEruption, // Unleash a massive eruption of soul energy, dealing damage to all enemies in the area and stunning them.
}
