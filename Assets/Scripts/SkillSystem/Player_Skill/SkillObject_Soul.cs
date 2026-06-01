using UnityEngine;

public class SkillObject_Soul : SkillObject_Base
{
    public Skill_AbsorbSoul absorbSoulManager;

    private Transform target;
    private bool canMoveToTarget;

    protected override void Update()
    {
        if (target == null)
            return;

        if (!canMoveToTarget)
        {
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance <= checkEnemyRadius)
                canMoveToTarget = true;
        }

        if (!canMoveToTarget)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    private void OnEnable()
    {
        target = null;
        canMoveToTarget = false;
    }

    public void SetupSoul(Skill_AbsorbSoul absorbSoul, bool canMove, float soulSpeed, Transform newTarget)
    {
        absorbSoulManager = absorbSoul;
        checkEnemyRadius = absorbSoul.checkEnemyRadius;
        speed = soulSpeed;
        target = newTarget;
        canMoveToTarget = canMove;
    }

    public void AbsorbSoul()
    {
        absorbSoulManager.AbsorbSoul(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        canMoveToTarget = true;
        target = collision.transform;
        AbsorbSoul();
    }
}