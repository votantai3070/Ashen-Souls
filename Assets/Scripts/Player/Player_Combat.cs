using UnityEngine;

public class Player_Combat : Entity_Combat
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override void PerformAttack(Entity dealer)
    {
        base.PerformAttack(dealer);
    }

    public override Collider2D[] FindAttackTarget(Transform attackArea)
    {
        attackRadius = player.attackRadius;
        return base.FindAttackTarget(attackArea);
    }
}
