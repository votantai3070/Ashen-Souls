using UnityEngine;

public class EnemyRange_DeadState : EnemyRange_State
{
    private AnimatorStateInfo info;
    private bool despawned;

    public EnemyRange_DeadState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        despawned = false;

        info = anim.GetCurrentAnimatorStateInfo(0);
        stateTimer = info.length;

        enemyRange.SetVelocity(0, 0);
        enemyRange.rb.linearVelocity = Vector2.zero;
        enemyRange.rb.simulated = false;

        Collider2D col = enemyRange.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemyRange.xIdleAndAttack = 0f;

        enemyRange.rb.simulated = true;

        Collider2D col = enemyRange.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (despawned)
            return;

        if (stateTimer < 0)
        {
            despawned = true;
            ObjectPool.instance.Despawn(enemyRange.gameObject);
        }
    }
}
