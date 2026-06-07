using UnityEngine;

public class EnemyMelee_DeadState : EnemyMelee_State
{
    private AnimatorStateInfo info;
    private bool despawned;

    public EnemyMelee_DeadState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        despawned = false;

        info = anim.GetCurrentAnimatorStateInfo(0);
        stateTimer = info.length;

        enemyMelee.SetVelocity(0, 0);
        enemyMelee.rb.linearVelocity = Vector2.zero;
        enemyMelee.rb.simulated = false;

        if (col != null)
            col.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemyMelee.xIdleAndAttack = 0f;

        enemyMelee.rb.simulated = true;

        Collider2D col = enemyMelee.GetComponent<Collider2D>();
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
            ObjectPool.instance.Despawn(enemyMelee.gameObject);
        }
    }
}