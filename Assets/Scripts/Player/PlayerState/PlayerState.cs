public class PlayerState : EntityState
{
    protected Player player;
    protected ControlsManager controls;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine<EntityState> stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        skillManager = player.skillManager;
        anim = player.anim;
        rb = player.rb;
        controls = player.controls;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (controls.inputActions.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);

        //if (controls.inputActions.Player.Sprint.IsPressed())
        //{
        //    stateMachine.ChangeState(player.sprintState);
        //}
    }
    private bool CanDash()
    {
        if (skillManager.deathDash.CanUseSkill() == false)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}
