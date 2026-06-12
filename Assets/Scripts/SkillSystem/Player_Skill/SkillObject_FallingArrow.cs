using System;
using UnityEngine;

public class SkillObject_FallingArrow : SkillObject_Base
{
    public Action OnHitSkill;

    private Skill_FallingArrow fallingArrowManager;
    public Transform target { get; private set; }

    private void OnEnable()
    {
        OnHitSkill = null;
        OnHitSkill += OnHit;
    }

    private void OnDisable()
    {
        OnHitSkill -= OnHit;
    }

    protected override void Update()
    {
        CheckDuration();

        if (target == null)
        {
            OnHit();
            return;
        }

        MoveToEnemy();
        RotationToEnemy();
    }

    public void SetupArrow(Skill_FallingArrow fallingArrow, float duration, LayerMask whatIsTarget)
    {
        fallingArrowManager = fallingArrow;
        whatIsEnemy = whatIsTarget;
        entity = fallingArrow.entity;
        skillType = fallingArrow.skillType;

        speed = fallingArrowManager.speedSkill.GetValue();
        damage = fallingArrowManager.damageSkill.GetValue();
        size = fallingArrowManager.sizeSkill.GetValue();

        target = fallingArrowManager.target;

        checkEnemyRadius = fallingArrowManager.checkEnemyRadius;
        checkDamageRadius = size * 0.4f;
        attackCooldownGuard = fallingArrowManager.attackCooldownGuard;
        this.duration = duration;

        spawnTime = Time.time;
        transform.localScale = Vector3.one * size;

        SetPhysicsActive(true);
    }

    private void MoveToEnemy()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    private void RotationToEnemy()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnHit()
    {
        target = null;

        if (fallingArrowManager != null)
            fallingArrowManager.OnArrowExpired(this);

        ObjectPool.instance.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Breakable"))
            return;

        if (entity == null)
        {
            OnHit();
            return;
        }

        if (target.GetComponent<Collider2D>() == collision)
            DamageEnemiesInRadius(target.transform, entity.transform, OnHitSkill);

    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            OnHit();
        }
    }
}