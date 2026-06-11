using UnityEngine;

[CreateAssetMenu(fileName = "Skill - ", menuName = "RPG Setup/Skill/Skill Data")]
public class Skill_DataSO : Skill_BaseSO
{
    public GameObject skillObjectPrefab;

    [Header("Special Skill")]
    public float radius = 1.5f;
    public int count = 3;

    [Header("Default Skill")]
    public float speed;
    public float size;
    public float cooldown;
    public float distanceToAttack;
    public float damage;
    [Space]
    public float attackCooldownGuard = .5f;

    public override string GetUpgradeDescription()
    {
        return description;
    }
}
