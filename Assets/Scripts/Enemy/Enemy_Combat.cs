using UnityEngine;

public class Enemy_Combat : Entity_Combat
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();

        showTargetEnemies = FindAttackTarget(attackArea);
    }

    public bool CanSeePlayer()
    {
        Collider2D playerCol = Physics2D.OverlapCircle(transform.position, enemy.detectionRadius, enemyLayer);

        if (enemy.player == null)
            enemy.SetPlayer(playerCol?.transform);

        if (playerCol == null || enemy.player == null)
            return false;

        Vector2 dirToPlayer = enemy.player.position - transform.position;

        float angle = Vector2.Angle(enemy.facingDirection, dirToPlayer);
        if (angle > enemy.detectionAngle / 2f) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, enemy.detectionRadius, enemyLayer);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public override Collider2D[] FindAttackTarget(Transform attackArea)
    {
        attackRadius = enemy.attackRadius;
        return base.FindAttackTarget(attackArea);
    }
}
