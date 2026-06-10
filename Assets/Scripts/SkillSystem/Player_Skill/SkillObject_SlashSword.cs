using System;
using UnityEngine;

public class SkillObject_SlashSword : SkillObject_Base
{
    public Action OnHitSkill;

    private Skill_SlashSword slashSword;
    public Transform target { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        CheckDuration();

        DamageEnemiesInRadius(transform, entity.transform);
    }

    public void SetupSlashSword(Skill_SlashSword slashSword, float duration, LayerMask whatIsEnemy)
    {
        this.slashSword = slashSword;
        this.whatIsEnemy = whatIsEnemy;
        entity = slashSword.entity;
        skillType = slashSword.skillType;

        speed = slashSword.speedSkill.GetValue();
        damage = slashSword.damageSkill.GetValue();
        size = slashSword.sizeSkill.GetValue();

        target = slashSword.target;
        checkEnemyRadius = size * .8f;
        checkDamageRadius = size * .8f;
        this.duration = duration;
        attackCooldownGuard = slashSword.attackCooldownGuard;

        spawnTime = Time.time;

        SetSpeedAnim(speed);
        transform.localScale = Vector3.one * size;
        SetPhysicsActive(true);
    }

    private void OnHit()
    {
        target = null;
        slashSword.OnSlashSwordExpired();
        ObjectPool.instance.Despawn(gameObject);
    }

    protected override void CheckDuration()
    {
        if (Time.time >= spawnTime + duration)
        {
            OnHit();
        }
    }
}
