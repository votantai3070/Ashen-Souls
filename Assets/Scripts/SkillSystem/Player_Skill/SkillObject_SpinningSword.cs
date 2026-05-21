using UnityEngine;

public class SkillObject_SpinningSword : SkillObject_Base
{
    private Skill_SpinningSword swordManager;
    private Transform centerTarget;

    private float orbitRadius;
    private float orbitSpeed;
    private float currentAngle;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetupSword(Skill_SpinningSword spinSwordManager, Entity owner, float radius, float dur, LayerMask whatIsEnemy, float startAngle = 0f)
    {
        swordManager = spinSwordManager;
        entity = owner;
        centerTarget = owner.transform;
        this.whatIsEnemy = whatIsEnemy;

        speed = spinSwordManager.speedSkill.GetValue();
        damage = spinSwordManager.damageSkill.GetValue();
        size = spinSwordManager.sizeSkill.GetValue();

        orbitRadius = radius;
        orbitSpeed = speed;
        currentAngle = startAngle;
        duration = dur;

        checkDamageRadius = size * 0.6f;
        checkEnemyRadius = size * 0.6f;
        attackCooldownGuard = spinSwordManager.attackCooldownGuard;

        transform.localScale = Vector3.one * size;

        spawnTime = Time.time;
    }

    protected override void Update()
    {
        if (centerTarget == null) return;

        Orbit();
        CheckDuration();
        DamageEnemiesInRadius(transform, entity.transform);
    }

    private void Orbit()
    {
        currentAngle -= orbitSpeed * Time.deltaTime;

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(
            Mathf.Cos(rad) * orbitRadius,
            Mathf.Sin(rad) * orbitRadius
        );

        rb.MovePosition(centerTarget.position + (Vector3)offset);

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected override void CheckDuration()
    {
        if (Time.time >= spawnTime + duration)
        {
            swordManager?.OnSwordExpired(this);
            ObjectPool.instance.Despawn(gameObject);
        }
    }
}