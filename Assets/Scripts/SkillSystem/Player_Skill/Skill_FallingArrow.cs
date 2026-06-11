using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FallingArrow : Skill_Base
{
    [Header("Arrow Config")]
    public Stat arrowCount;
    [Space]
    [SerializeField] private List<SkillObject_FallingArrow> activeArrows = new();
    [SerializeField] private GameObject arrowGo;

    [SerializeField] private float createArrowPos = 20;
    [SerializeField] private float minOffsetX = -2;
    [SerializeField] private float maxOffsetX = 2;

    private Coroutine createArrowCo;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        arrowGo = skillData.skillObjectPrefab;
        arrowCount.SetBaseValue(skillData.count);
    }

    public override bool TryUseSkill()
    {
        if (base.TryUseSkill() == false)
            return false;

        if (!CheckEnemyRadius() || target == null)
            return false;

        CreateArrowByCount();
        SetSkillOnCooldown();
        return true;
    }

    private void CreateArrowByCount()
    {
        if (createArrowCo != null)
            StopCoroutine(createArrowCo);

        createArrowCo = StartCoroutine(CreateArrowCo());
    }

    private IEnumerator CreateArrowCo()
    {
        int arrows = (int)arrowCount.GetValue();

        for (int i = 0; i < arrows; i++)
        {
            CreateArrow();

            if (i < arrows - 1)
                yield return new WaitForSeconds(.5f);
        }
    }

    public void CreateArrow()
    {
        float offsetX = GetOffsetX();

        Vector2 upperTarget = target.position + new Vector3(offsetX, createArrowPos, 0);

        GameObject obj = ObjectPool.instance.Spawn(arrowGo.name, upperTarget, Quaternion.identity);
        var arrow = obj.GetComponent<SkillObject_FallingArrow>();
        arrow.SetupArrow(this, duration, whatIsEnemy);

        activeArrows.Add(arrow);
    }

    protected override Stat GetStat(StatType upgradeType)
    {
        return upgradeType switch
        {
            StatType.Damage => damageSkill,
            StatType.Speed => speedSkill,
            StatType.Size => sizeSkill,
            StatType.AttackSpeed => attackSpeedSkill,
            StatType.Count => arrowCount,
            _ => null
        };
    }

    private float GetOffsetX()
    {
        return Random.Range(minOffsetX, maxOffsetX);
    }

    public void OnArrowExpired(SkillObject_FallingArrow arrow)
    {
        activeArrows.Remove(arrow);
    }
}
