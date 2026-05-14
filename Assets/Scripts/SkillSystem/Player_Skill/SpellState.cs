using UnityEngine;

public class SpellState : IState
{
    protected StateMachine<SpellState> stateMachine;
    protected string animBoolName;

    protected Rigidbody2D rb;
    protected Animator anim;

    protected float stateTimer;

    public SpellState(StateMachine<SpellState> stateMachine, string animBoolName)
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
