using UnityEngine;

public class Player : Entity
{
    public ControlsManager controls { get; private set; }
    public UI ui { get; private set; }
    public Player_Combat combat { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_Health health { get; private set; }
    public Player_Stats stats { get; private set; }
    public Player_VFX vFX { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_AttackState attackState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_SprintState sprintState { get; private set; }

    [Space]

    public bool canLookAttack;

    [Header("Dash & Sprint")]
    public float dashSpeed = 20f;
    public float sprintSpeed = 10f;
    public float holdTimer = 0f;
    public float holdThreshold = 0.2f;

    protected override void Awake()
    {
        base.Awake();

        controls = ControlsManager.instance;
        ui = FindFirstObjectByType<UI>();
        combat = GetComponent<Player_Combat>();
        skillManager = GetComponentInChildren<Player_SkillManager>();
        health = GetComponent<Player_Health>();
        stats = GetComponent<Player_Stats>();
        vFX = GetComponent<Player_VFX>();

        controls.Init(this);

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        attackState = new(this, stateMachine, "Attack");
        dashState = new(this, stateMachine, "Dash");
        sprintState = new(this, stateMachine, "Sprint");
    }

    protected override void Start()
    {
        base.Start();

        UI.instance.SetPlayer(this);

        stateMachine.Initialize(idleState);

    }

    protected override void Update()
    {
        base.Update();
    }

    public void LookAttackIfNeeded()
    {
        if (canLookAttack == false) return;

        Vector2 lookDir = ControlsManager.instance.lookInput;
        xIdleAndAttack = lookDir.x;
        yIdleAndAttack = lookDir.y;
    }
}
