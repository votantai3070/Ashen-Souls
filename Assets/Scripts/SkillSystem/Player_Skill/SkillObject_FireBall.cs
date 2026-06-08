using UnityEngine;

public class SkillObject_FireBall : SkillObject_Base
{
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
    }

    protected override void Update()
    {
        base.Update();

        CheckDuration();
    }

    public void SetupFireBall(Skill_FireBall skill, float duration, LayerMask whatIsEnemy)
    {
        fireBallManager = skill;
        this.whatIsEnemy = whatIsEnemy;
        entity = fireBallManager.entity;

        speed = fireBallManager.speedSkill.GetValue();
        damage = fireBallManager.damageSkill.GetValue();
        size = fireBallManager.sizeSkill.GetValue();

        target = fireBallManager.target;
        checkEnemyRadius = fireBallManager.checkEnemyRadius;
        checkDamageRadius = size * .26f; // The explosion radius is smaller than the visual size of the fire soul, so we use a fraction of the size for the damage radius.
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

        SetPhysicsActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Breakable"))
        {
            DamageEnemiesInRadius(transform, entity.transform);
            OnHit();
        }
    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            fireBallManager.OnBallBurstExpired();
            ObjectPool.instance.Despawn(gameObject);
        }
    }
}
