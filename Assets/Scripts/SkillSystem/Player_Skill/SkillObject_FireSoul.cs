using UnityEngine;

public class SkillObject_FireSoul : SkillObject_Base
{
    public FireSoul_CreateState createState { get; private set; }
    public FireSoul_IdleState idleState { get; private set; }
    public FireSould_ShootAntecipationState shootAntecipationState { get; private set; }
    public FireSoul_ShotState shotState { get; private set; }
    public FireSoul_ExplodeState explodeState { get; private set; }

    [Header("Fire Soul Settings")]
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
        skillType = fireSoulManager.skillType;

        speed = fireSoulManager.speedSkill.GetValue();
        damage = fireSoulManager.damageSkill.GetValue();
        size = fireSoulManager.sizeSkill.GetValue();

        target = fireSoulManager.target;
        checkEnemyRadius = fireSoulManager.checkEnemyRadius;
        checkDamageRadius = size * .26f; // The explosion radius is smaller than the visual size of the fire soul, so we use a fraction of the size for the damage radius.
        attackCooldownGuard = fireSoulManager.attackCooldownGuard;
        this.duration = duration;

        spawnTime = Time.time;

        transform.localScale = Vector3.one * size;
        SetPhysicsActive(true);
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
        if (collision == null) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Breakable"))
        {
            DamageEnemiesInRadius(collision.transform, entity.transform);
            OnHit();
        }
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
