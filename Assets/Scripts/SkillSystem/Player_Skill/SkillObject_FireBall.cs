using System;
using UnityEngine;

public class SkillObject_FireBall : SkillObject_Base
{
    public Action OnHitSkill;

    private Skill_FireBall fireBallManager;
    public Transform target { get; private set; }

    public FireBall_ShotState shotState { get; private set; }
    public FireBall_ExplodeState explodeState { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        shotState = new(this, stateMachine, "Shot");
        explodeState = new(this, stateMachine, "Explode");
    }

    protected override void Start()
    {
        stateMachine.Initialize(shotState);

        OnHitSkill += OnHit;
    }

    private void OnDestroy()
    {
        OnHitSkill -= OnHit;
    }

    protected override void Update()
    {
        base.Update();

        CheckDuration();
    }

    public void SetupFireBall(Skill_FireBall fireBall, float duration, LayerMask whatIsEnemy)
    {
        fireBallManager = fireBall;
        this.whatIsEnemy = whatIsEnemy;
        entity = fireBallManager.entity;
        skillType = fireBall.skillType;

        speed = fireBallManager.speedSkill.GetValue();
        damage = fireBallManager.damageSkill.GetValue();
        size = fireBallManager.sizeSkill.GetValue();

        target = fireBallManager.target;
        checkEnemyRadius = fireBallManager.checkEnemyRadius;
        checkDamageRadius = size * 0.4f; // The explosion radius is smaller than the visual size of the fire soul, so we use a fraction of the size for the damage radius.
        attackCooldownGuard = fireBallManager.attackCooldownGuard;
        this.duration = duration;

        spawnTime = Time.time;

        transform.localScale = Vector3.one * size;
        SetPhysicsActive(true);
    }

    public void OnHit()
    {
        stateMachine.ChangeState(shotState);
        target = null;
        fireBallManager.OnBallBurstExpired();
        ObjectPool.instance.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Breakable"))
        {
            DamageEnemiesInRadius(collision.transform, entity.transform, OnHitSkill);
        }
    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            OnHit();
        }
    }
}
