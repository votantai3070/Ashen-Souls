
using UnityEngine;

public class SkillObject_SoulBurst : SkillObject_Base
{
    private Skill_SoulBurst soulBurstManager;

    protected override void Start()
    {
    }

    protected override void Update()
    {
        CheckDuration();
        DamageEnemiesInRadius(transform, entity.transform);
    }

    public void SetSoulBurst(Skill_SoulBurst soulBurstManager, LayerMask enemyLayer)
    {
        this.soulBurstManager = soulBurstManager;
        entity = soulBurstManager.entity;
        whatIsEnemy = enemyLayer;

        SetSpeedAnim(soulBurstManager.speed);
        float clipLength = GetClipLength();
        float actualDuration = clipLength / soulBurstManager.speed;

        checkDamageRadius = soulBurstManager.checkDamageRadius;
        checkEnemyRadius = soulBurstManager.checkEnemyRadius;
        duration = actualDuration;

        Debug.Log("Duration:  " + duration);

        transform.localScale = Vector3.one * checkEnemyRadius;
        spawnTime = Time.time;
    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            soulBurstManager.OnSoulBurstExpired();
            ObjectPool.instance.Despawn(gameObject);
        }
    }

    private float GetClipLength()
    {
        AnimatorClipInfo[] clipInfos = anim.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
            return clipInfos[0].clip.length;

        return 1f; // fallback
    }
}
