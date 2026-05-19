using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "RPG Setup/Stat/Character Stat")]
public class Stat_SO : ScriptableObject
{
    [Header("Resource Info")]
    public float maxHealth;
    public float healthRegen;

    [Header("Offense Info - Physical Damage")]
    public float attackSpeed;
    public float speed;
    public float damage;
    public float critChance;
    public float critDamage;
    public float armorReduction;

    //[Header("Offense Info - Elemental Damage")]
    //public float fireDamage;
    //public float iceDamage;
    //public float lightningDamage;

    [Header("Defense Info - Physical Damage")]
    public float armor;
    public float evasion;

    //[Header("Defense Info - Elemental Damage")]
    //public float fireResistance;
    //public float iceResistance;
    //public float lightningResistance;

    [Header("Major Info")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
