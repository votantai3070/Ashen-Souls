using UnityEngine;

[CreateAssetMenu(fileName = "Skill - ", menuName = "RPG Setup/Skill/Skill Data")]
public class Skill_DataSO : Skill_BaseSO
{
    public GameObject skillObjectPrefab;

    [Header("Spinning Sword Skill")]
    public float orbitRadius = 1.5f;
    public int swordCount = 3;

    [Header("Default Skill")]
    public float speed;
    public float size;
    public float cooldown;
    public float distanceToAttack;
    public float damage;

    [Header("Upgrade")]
    [Range(0, 100)]
    public float upgradeBoostChance = 30f;
    public float attackCooldownGuard = .5f;

    public override string GetUpgradeDescription()
    {
        return description;
    }
}
