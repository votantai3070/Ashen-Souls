public class EnemyMelee_AttackState : EnemyMelee_State
{
    private bool specialAttackStarted;
    private bool specialAttackFinished;

    public EnemyMelee_AttackState(Enemy enemy, StateMachine<EntityState> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemyMelee.canTrigger = false;
        enemyMelee.SetVelocity(0f, 0f);
        enemyMelee.SetAnimIdleAndAttackAnimation();

        specialAttackStarted = false;
        specialAttackFinished = false;

        if (enemyMelee.enemyType == EnemyMeleeType.Special)
        {
            specialAttackStarted = true;
            enemyMelee.telegraph.StartDash(OnSpecialAttackFinished);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemyMelee.combat.PerformAttack(enemyMelee);

        switch (enemyMelee.enemyType)
        {
            case EnemyMeleeType.Normal:
                HandleNormalEnemy();
                break;

            case EnemyMeleeType.Special:
                HandleSpecialEnemy();
                break;
        }
    }

    public void HandleNormalEnemy()
    {
        if (enemyMelee.canTrigger)
            stateMachine.ChangeState(enemyMelee.idleState);
    }

    public void HandleSpecialEnemy()
    {
        if (specialAttackStarted && specialAttackFinished && enemyMelee.canTrigger)
            stateMachine.ChangeState(enemyMelee.idleState);
    }

    private void OnSpecialAttackFinished()
    {
        enemyMelee.canTrigger = true;
        specialAttackFinished = true;
        enemyMelee.combat.SetAttackWindow(false);
    }
}