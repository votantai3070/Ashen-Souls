public enum EnemyMeleeType
{
    Normal, Special
}

public class Enemy_Melee : Enemy
{
    public EnemyMeleeType enemyType;
    public EnemyMeleeAttackTelegraph telegraph { get; private set; }

    public EnemyMelee_IdleState idleState { get; private set; }
    public EnemyMelee_MoveState moveState { get; private set; }
    public EnemyMelee_ChaseState chaseState { get; private set; }
    public EnemyMelee_AttackState attackState { get; private set; }
    public EnemyMelee_DeadState deadState { get; private set; }
    public EnemyMelee_PrepareAttackState prepareAttackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        telegraph = GetComponentInChildren<EnemyMeleeAttackTelegraph>();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        chaseState = new(this, stateMachine, "Chase");
        attackState = new(this, stateMachine, "Attack");
        deadState = new(this, stateMachine, "Dead");
        prepareAttackState = new(this, stateMachine, "Prepare");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TryToIdleState()
    {
        if (stateMachine.currentState == idleState)
            return;

        stateMachine.ChangeState(idleState);
    }

    public override void TryToDieState()
    {
        if (stateMachine.currentState == deadState)
            return;
        stateMachine.ChangeState(deadState);
    }
}

