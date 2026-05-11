using UnityEngine;

public class SkillObject_Soul : SkillObject_Base
{
    public Skill_AbsorbSoul absorbSoulManager;
    private Transform target;
    private float speed;


    protected override void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed, Transform newTarget = null)
    {
        target = newTarget == null ? absorbSoulManager.FindClosestTarget() : newTarget;
        this.speed = speed;
    }

    public void SetupSoul(Skill_AbsorbSoul absorbSoul, bool canMove, float soulSpeed, Transform target)
    {
        absorbSoulManager = absorbSoul;
        checkEnemyRadius = absorbSoul.checkEnemyRadius;

        if (canMove)
            MoveTowardsClosestTarget(soulSpeed, target);

    }

    public void AbsorbSoul()
    {
        absorbSoulManager.AbsorbSoul(this);
        ObjectPool.instance.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        AbsorbSoul();
    }
}
