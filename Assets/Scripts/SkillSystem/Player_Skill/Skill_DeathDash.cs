using System.Collections;
using UnityEngine;

public class Skill_DeathDash : Skill_Base
{
    private SpriteRenderer sr;

    [Header("Death Dash Settings")]
    [SerializeField] private GameObject deadDashPrefab;
    private Coroutine dashEffectCo;

    [SerializeField] private float dashInterval = .2f;

    protected override void Awake()
    {
        base.Awake();

        sr = entity.GetComponentInChildren<SpriteRenderer>();
    }

    public override void SetSkill(Skill_DataSO skillData)
    {
        base.SetSkill(skillData);

        deadDashPrefab = skillData.skillObjectPrefab;
    }

    public override bool TryUseSkill()
    {
        if (skillType == SkillType.DeathDash)
        {
            DoDashEffect();
            SetSkillOnCooldown();
        }

        return true;
    }

    private void DoDashEffect()
    {
        if (dashEffectCo != null)
            StopCoroutine(dashEffectCo);

        dashEffectCo = StartCoroutine(DashEffectCo());
    }

    private IEnumerator DashEffectCo()
    {
        float elapse = 0;

        while (elapse < duration)
        {
            CreateDash(deadDashPrefab);

            yield return new WaitForSeconds(dashInterval);
            elapse += dashInterval;
        }
    }

    private void CreateDash(GameObject dashPrefab)
    {
        GameObject dash = ObjectPool.instance.Spawn(dashPrefab.name, transform.position, Quaternion.identity);
        dash.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
        dash.GetComponent<SkillObject_DeathDash>().SetDeathDash(this, duration, whatIsEnemy);
    }
}
