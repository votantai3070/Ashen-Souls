using UnityEngine;

public class SkillObjectEnemy_EnergyBall : SkillObject_Base
{
    [Header("Energy Ball Settings")]
    private SkillEnemy_Base energyBallManager;
    public float speed { get; private set; }
    public Transform target { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetupEnergyBall(SkillEnemy_Base energyBallManager, float duration, Vector2 direction, LayerMask enemyLayer)
    {
        this.energyBallManager = energyBallManager;
        whatIsEnemy = enemyLayer;
        entity = energyBallManager.entity;
        SetPhysicsActive(true);

        speed = energyBallManager.speed;
        target = energyBallManager.target;
        checkEnemyRadius = energyBallManager.checkEnemyRadius;
        checkDamageRadius = energyBallManager.checkDamageRadius;
        this.duration = duration;

        rb.linearVelocity = new(direction.x * speed, direction.y * speed);

        spawnTime = Time.time;
    }

    public void OnHit()
    {
        target = null;
        energyBallManager.OnSoulBurstExpired();
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
