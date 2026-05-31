using UnityEngine;

public class Player : Entity, ITotalSummary
{
    public ControlsManager controls { get; private set; }
    public Player_Combat combat { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public Player_Health health { get; private set; }
    public Player_Stats stats { get; private set; }
    public Player_VFX vFX { get; private set; }
    public Player_SFX sFX { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_AttackState attackState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_SprintState sprintState { get; private set; }
    public Player_DeadState deadState { get; private set; }

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
        skillManager = GetComponentInChildren<Player_SkillManager>();
        combat = GetComponent<Player_Combat>();
        health = GetComponent<Player_Health>();
        stats = GetComponent<Player_Stats>();
        vFX = GetComponent<Player_VFX>();
        sFX = GetComponentInChildren<Player_SFX>();

        controls.Init(this);

        idleState = new(this, stateMachine, "Idle");
        moveState = new(this, stateMachine, "Move");
        attackState = new(this, stateMachine, "Attack");
        dashState = new(this, stateMachine, "Dash");
        sprintState = new(this, stateMachine, "Sprint");
        deadState = new(this, stateMachine, "Dead");
    }

    protected override void Start()
    {
        base.Start();

        UI.instance.SetPlayer(this);
        AudioManager.instance.SetPlayer(this);
        GameManager.instance.SetPlayer(this);

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    //public void SetupPlayer(PlayerData data)
    //{
    //    stats.DefaultStatSetup(data.defaultCharacterStat);
    //    anim.runtimeAnimatorController = data.animator;
    //}

    public override void TryToDieState()
    {
        if (stateMachine.currentState == deadState)
            return;
        stateMachine.ChangeState(deadState);
    }

    public void LookAttackIfNeeded()
    {
        if (canLookAttack == false) return;

        Vector2 lookDir = ControlsManager.instance.lookInput;
        xIdleAndAttack = lookDir.x;
        yIdleAndAttack = lookDir.y;
    }

    public void AddDamageDealt(float damage)
    {
        GameManager.instance.TotalDamageDealt += damage;
    }

    public void AddSoulsGained()
    {
        GameManager.instance.SoulsGained++;
    }

    public void AddEnemiesKilled()
    {
        GameManager.instance.TotalEnemiesKilled++;
    }
}
