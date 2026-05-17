public class Player_AttackState : PlayerState
{
    public Player_AttackState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.canLookAttack = true;
        player.canTrigger = false;

        player.LookAttackIfNeeded();

        anim.SetFloat("AttackSpeed", player.attackSpeed);
        anim.SetFloat("xIdleAndAttack", player.xIdleAndAttack);
        anim.SetFloat("yIdleAndAttack", player.yIdleAndAttack);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0);

        player.combat.PerformAttack(player);

        if (player.canTrigger)
        {
            player.canLookAttack = false;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
