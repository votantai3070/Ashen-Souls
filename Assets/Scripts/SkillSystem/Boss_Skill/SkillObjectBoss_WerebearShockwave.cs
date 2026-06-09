using System;
using UnityEngine;

public class SkillObjectBoss_WerebearShockwave : SkillObject_Base
{
    public Action OnHitSkill;

    private SkillEnemy_Base shockwaveManager;
    public Transform target { get; private set; }

    private Coroutine delayDespawnCo;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        OnHitSkill += OnHit;
    }

    private void OnDestroy()
    {
        OnHitSkill -= OnHit;
    }

    protected override void Update()
    {
        CheckDuration();
    }

    public void SetupShockWave(SkillEnemy_Base werebearShockwave, float duration, Vector2 direction, LayerMask whatIsTarget)
    {
        shockwaveManager = werebearShockwave;
        whatIsEnemy = whatIsTarget;
        entity = werebearShockwave.entity;
        SetPhysicsActive(true);

        damage = werebearShockwave.damage;
        speed = werebearShockwave.speed;
        target = werebearShockwave.target;
        checkEnemyRadius = werebearShockwave.checkEnemyRadius;
        checkDamageRadius = werebearShockwave.checkDamageRadius;
        attackCooldownGuard = werebearShockwave.attackCooldownGuard;
        this.duration = duration;

        rb.linearVelocity = direction * speed;

        spawnTime = Time.time;
    }

    public void OnHit()
    {
        target = null;
        shockwaveManager.OnSkillExpired();
        ObjectPool.instance.Despawn(gameObject);

        SetPhysicsActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Player")) return;

        DamageEnemiesInRadius(transform, entity.transform, OnHitSkill);
    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            OnHit();
        }
    }
}
