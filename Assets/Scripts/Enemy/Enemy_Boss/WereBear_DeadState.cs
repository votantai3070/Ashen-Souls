using UnityEngine;

public class WereBear_DeadState : WereBearBoss_State
{
    private AnimatorStateInfo info;
    private bool despawned;

    public WereBear_DeadState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        despawned = false;

        info = anim.GetCurrentAnimatorStateInfo(0);
        stateTimer = info.length;

        wereBear.SetVelocity(0, 0);
        wereBear.rb.linearVelocity = Vector2.zero;
        wereBear.rb.simulated = false;

        if (col != null)
            col.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();

        wereBear.xIdleAndAttack = 0f;

        wereBear.rb.simulated = true;

        Collider2D col = wereBear.GetComponent<Collider2D>();
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
            ObjectPool.instance.Despawn(wereBear.gameObject);
        }
    }
}
