using UnityEngine;

public class SkillObject_SlashSword : SkillObject_Base
{
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

        speed = slashSword.speedSkill.GetValue();
        damage = slashSword.damageSkill.GetValue();
        size = slashSword.sizeSkill.GetValue();

        target = slashSword.target;
        checkEnemyRadius = slashSword.checkEnemyRadius;
        checkDamageRadius = size * .8f;
        this.duration = duration;

        spawnTime = Time.time;

        SetSpeedAnim(speed);
        transform.localScale = Vector3.one * size;
    }

    protected override void CheckDuration()
    {
        if (Time.time >= spawnTime + duration)
        {
            slashSword.OnSlashSwordExpired();
            ObjectPool.instance.Despawn(gameObject);
        }
    }
}
