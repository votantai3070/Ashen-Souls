using UnityEngine;

public class SkillObject_FireSoul : SkillObject_Base
{
    public FireSoul_CreateState createState { get; private set; }
    public FireSoul_IdleState idleState { get; private set; }
    public FireSould_ShootAntecipationState shootAntecipationState { get; private set; }
    public FireSoul_ShotState shotState { get; private set; }
    public FireSoul_ExplodeState explodeState { get; private set; }

    [Header("Fire Soul Settings")]
    public float speed { get; private set; }
    private Skill_FireSoul fireSoulManager;
    public Transform target { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        createState = new(this, stateMachine, "Create");
        idleState = new(this, stateMachine, "Idle");
        shootAntecipationState = new(this, stateMachine, "ShootAntecipation");
        shotState = new(this, stateMachine, "Shot");
        explodeState = new(this, stateMachine, "Explode");
    }

    protected override void Start()
    {
        stateMachine.Initialize(createState);
    }

    protected override void Update()
    {
        base.Update();

        CheckDuration();
    }

    public void SetupFireSoul(Skill_FireSoul fireSoulManager, float duration, LayerMask enemyLayer)
    {
        this.fireSoulManager = fireSoulManager;
        whatIsEnemy = enemyLayer;
        entity = fireSoulManager.entity;

        speed = fireSoulManager.speed;
        target = fireSoulManager.target;
        checkEnemyRadius = fireSoulManager.checkEnemyRadius;
        checkDamageRadius = fireSoulManager.checkDamageRadius;
        attackCooldownGuard = fireSoulManager.attackCooldownGuard;
        this.duration = duration;

        spawnTime = Time.time;
    }

    public void OnHit()
    {
        stateMachine.ChangeState(createState);
        target = null;
        fireSoulManager.OnSoulBurstExpired();
        ObjectPool.instance.Despawn(gameObject);

        SetPhysicsActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Enemy")) return;

        DamageEnemiesInRadius(transform, entity.transform);
    }

    protected override void CheckDuration()
    {
        if (Time.time > spawnTime + duration)
        {
            fireSoulManager.OnSoulBurstExpired();
            ObjectPool.instance.Despawn(gameObject);
        }
    }
}
