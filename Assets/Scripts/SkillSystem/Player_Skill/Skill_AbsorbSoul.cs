using UnityEngine;

public class Skill_AbsorbSoul : Skill_Base
{
    private Player player;

    [Header("Absorb Soul Settings")]
    public GameObject soul;
    private readonly float distance = 1f;
    public float speedOfSoul = 5f;

    protected override void Awake()
    {
        base.Awake();

        player = entity as Player;
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        soul = skillData.skillObjectPrefab;
    }

    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        SetSkillOnCooldown();

        return true;
    }

    public void AbsorbSoul(SkillObject_Soul soul)
    {
        Debug.Log("Absorb Soul");

        if (Vector3.Distance(soul.transform.position, player.transform.position) > distance)
            return;

        Debug.Log("Absorbed Soul..");

        player.AddSoulsGained();
        ObjectPool.instance.Despawn(soul.gameObject);
    }
}
