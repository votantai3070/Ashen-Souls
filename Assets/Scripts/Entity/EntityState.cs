using UnityEngine;

public class EntityState : IState
{
    protected StateMachine<EntityState> stateMachine;
    protected string animBoolName;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Collider2D col;

    protected float stateTimer;

    public EntityState(StateMachine<EntityState> stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
