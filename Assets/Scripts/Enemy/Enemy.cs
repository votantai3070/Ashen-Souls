using UnityEngine;

public class Enemy : Entity
{
    public Transform player { get; private set; }
    public Enemy_Combat combat { get; private set; }
    public Enemy_Health health { get; private set; }
    public Enemy_Stats stats { get; private set; }

    public float idleTimer = 2f;
    protected bool isFacingRight = true;

    [Header("Detection")]
    public float detectionRadius = 5f;
    public float detectionAngle = 90f;


    [Header("Chase Info")]
    public float baseChaseSpeed = 5f;
    public float chaseSpeed = 8f;
    public float chaseStopDistance = 1.5f;

    [Header("PerformAttack Info")]
    public Vector2 backOffset = new(10f, 10f);
    public float attackDistanceToPlayer = 1f;

    [Header("Enemy Special Info")]
    public float specialDistance;
    public float specialSpeed;
    public float specialDashDuration;

    public Vector2 facingDirection { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        combat = GetComponent<Enemy_Combat>();
        health = GetComponent<Enemy_Health>();
        stats = GetComponent<Enemy_Stats>();
    }

    protected override void Start()
    {
        base.Start();

        facingDirection = Vector2.down;
    }

    protected override void Update()
    {
        base.Update();

        Vector2 input = new(anim.GetFloat("xMove"), anim.GetFloat("yMove"));
        if (input != Vector2.zero)
            facingDirection = input.normalized;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TryToIdleState();
    }

    public Vector2 GetDirectionPlayer() => (player.position - transform.position).normalized;

    public virtual void TryToIdleState()
    {

    }

    public virtual void TryToHitState()
    {

    }

    protected override void RefreshStats()
    {
        base.RefreshStats();

        chaseSpeed = entityStats.GetSpeed();
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    public Player GetPlayer() => player.GetComponent<Player>();

    public bool IsPlayerInAttackRange()
    {
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= attackDistanceToPlayer;
    }

    public bool CanStopChaseRange() => Vector2.Distance(transform.position, player.position) <= chaseStopDistance;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Vector3 leftDir = Quaternion.Euler(0, 0, detectionAngle / 2f) * facingDirection;
        Vector3 rightDir = Quaternion.Euler(0, 0, -detectionAngle / 2f) * facingDirection;
        Gizmos.DrawRay(transform.position, leftDir * detectionRadius);
        Gizmos.DrawRay(transform.position, rightDir * detectionRadius);
    }
}
