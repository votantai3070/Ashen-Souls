public class Boss_WereBear : EnemyBoss
{
    public Enemy_SkillManager skillManager { get; private set; }

    public WereBear_IdleState idleState { get; private set; }
    public WereBear_ChaseState chaseState { get; private set; }
    public WereBear_AttackState attackState { get; private set; }
    public WereBear_DeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        skillManager = GetComponentInChildren<Enemy_SkillManager>();

        idleState = new WereBear_IdleState(this, stateMachine, "Idle");
        chaseState = new WereBear_ChaseState(this, stateMachine, "Chase");
        attackState = new WereBear_AttackState(this, stateMachine, "Attack");
        deadState = new WereBear_DeadState(this, stateMachine, "Dead");
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

    public override void TryToDieState()
    {
        if (stateMachine.currentState == deadState)
            return;

        stateMachine.ChangeState(deadState);
    }

    public override void TryToHitState()
    {
    }

    public override void TryToIdleState()
    {
        if (stateMachine.currentState == idleState)
            return;

        stateMachine.ChangeState(idleState);
    }
}
