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

    public void SetupSword(Skill_SpinningSword manager, Entity owner, float radius, float speed, float dur, float startAngle = 0f)
    {
        swordManager = manager;
        entity = owner;
        centerTarget = owner.transform;
        upgradeType = manager.upgradeType;

        orbitRadius = radius;
        orbitSpeed = speed;
        duration = dur;
        spawnTime = Time.time;
        currentAngle = startAngle;

        checkDamageRadius = manager.checkDamageRadius;
        checkEnemyRadius = manager.checkEnemyRadius;

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
        currentAngle += orbitSpeed * Time.deltaTime;

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