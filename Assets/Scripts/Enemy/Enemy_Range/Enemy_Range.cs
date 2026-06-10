public class Enemy_Range : Enemy
{
    public Enemy_SkillManager skillManager { get; private set; }

    public EnemyRange_IdleState idleState { get; private set; }
    public EnemyRange_MoveState moveState { get; private set; }
    public EnemyRange_ChaseState chaseState { get; private set; }
    public EnemyRange_AttackState attackState { get; private set; }
    public EnemyRange_DeadState deadState { get; private set; }
    public EnemyRange_HitState hitState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        skillManager = GetComponentInChildren<Enemy_SkillManager>();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        chaseState = new(this, stateMachine, "Chase");
        attackState = new(this, stateMachine, "Attack");
        deadState = new(this, stateMachine, "Dead");
        hitState = new(this, stateMachine, "Hit");
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

    public override void TryToHitState()
    {
        if (stateMachine.currentState == hitState)
            return;
        stateMachine.ChangeState(hitState);
    }
}
