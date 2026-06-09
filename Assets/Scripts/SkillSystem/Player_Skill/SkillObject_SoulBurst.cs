
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
        entity = soulBurstManager.entity;
        this.soulBurstManager = soulBurstManager;
        whatIsEnemy = enemyLayer;

        speed = soulBurstManager.speedSkill.GetValue();
        damage = soulBurstManager.damageSkill.GetValue();
        size = soulBurstManager.sizeSkill.GetValue();

        Debug.Log($"SoulBurst - Speed: {speed}, Damage: {damage}, Size: {size}");

        SetSpeedAnim(speed);
        float clipLength = GetClipLength();
        float actualDuration = clipLength / speed;

        //transform.GetComponentInChildren<Image>().color = GameColors.Soul;

        checkDamageRadius = size * 3f;
        checkEnemyRadius = size * 2f;
        attackCooldownGuard = soulBurstManager.attackCooldownGuard;
        duration = actualDuration;

        spawnTime = Time.time;

        transform.localScale = Vector3.one * size;
        SetPhysicsActive(true);
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
