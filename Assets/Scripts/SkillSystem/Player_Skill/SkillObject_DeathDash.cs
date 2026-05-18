using UnityEngine;

public class SkillObject_DeathDash : SkillObject_Base
{
    private Skill_DeathDash deathDashManager;

    public void SetDeathDash(Skill_DeathDash deathDashManager, float duration, LayerMask enemyLayer)
    {
        this.deathDashManager = deathDashManager;
        whatIsEnemy = enemyLayer;
        entity = deathDashManager.entity;

        checkDamageRadius = deathDashManager.checkDamageRadius;
        checkEnemyRadius = deathDashManager.checkEnemyRadius;
        attackCooldownGuard = deathDashManager.attackCooldownGuard;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        DamageEnemiesInRadius(transform, entity.transform);
    }
}
