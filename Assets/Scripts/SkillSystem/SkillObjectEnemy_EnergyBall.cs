using UnityEngine;

public class SkillObjectEnemy_EnergyBall : SkillObject_Base
{
    [Header("Energy Ball Settings")]
    private SkillEnemy_Base energyBallManager;
    public Transform target { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        CheckDuration();
    }

    public void SetupEnergyBall(SkillEnemy_Base energyBallManager, float duration, Vector2 direction, LayerMask enemyLayer)
    {
        this.energyBallManager = energyBallManager;
        whatIsEnemy = enemyLayer;
        entity = energyBallManager.entity;
        SetPhysicsActive(true);

        damage = energyBallManager.damage;
        speed = energyBallManager.speed;
        target = energyBallManager.target;
        checkEnemyRadius = energyBallManager.checkEnemyRadius;
        checkDamageRadius = energyBallManager.checkDamageRadius;
        attackCooldownGuard = energyBallManager.attackCooldownGuard;
        this.duration = duration;

        rb.linearVelocity = direction * speed;

        spawnTime = Time.time;
    }

    public void OnHit()
    {
        target = null;
        energyBallManager.OnSkillExpired();
        ObjectPool.instance.Despawn(gameObject);

        SetPhysicsActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Player")) return;

        DamageEnemiesInRadius(transform, entity.transform);
        OnHit();
    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            OnHit();
        }
    }
}
