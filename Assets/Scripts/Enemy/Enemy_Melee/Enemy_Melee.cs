public class Enemy_Melee : Enemy
{
    public EnemyMelee_IdleState idleState { get; private set; }
    public EnemyMelee_MoveState moveState { get; private set; }
    public EnemyMelee_ChaseState chaseState { get; private set; }
    public EnemyMelee_AttackState attackState { get; private set; }
    public EnemyMelee_DeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        chaseState = new(this, stateMachine, "Chase");
        attackState = new(this, stateMachine, "Attack");
        deadState = new(this, stateMachine, "Dead");
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

