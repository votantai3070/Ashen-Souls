using UnityEngine;

public class Enemy_Range : Enemy
{
    public EnemyRange_IdleState idleState { get; private set; }
    public Enemy_RangeMoveState moveState { get; private set; }
    public EnemyRange_ChaseState chaseState { get; private set; }
    public Enemy_RangeAttackState attackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        chaseState = new(this, stateMachine, "Chase");
        attackState = new(this, stateMachine, "Attack");
        //deadState = new(this, stateMachine, "Dead");
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
        //if (stateMachine.currentState == deadState)
        //    return;
        //stateMachine.ChangeState(deadState);
    }

    public void Flip(Vector2 direction)
    {
        if (direction.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
