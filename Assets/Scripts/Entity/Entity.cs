using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Entity_Combat entityCombat { get; private set; }
    public Entity_Stats entityStats { get; private set; }
    public Entity_Health entityHealth { get; private set; }
    public Entity_VFX entityVFX { get; private set; }
    public StateMachine<EntityState> stateMachine { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public Collider2D col { get; private set; }

    public float xIdleAndAttack { get; set; }
    public float yIdleAndAttack { get; set; }

    [Header("Entity Stat")]
    public float moveSpeed = 5;
    public float attackSpeed = 1;
    public float attackRange = 2;
    public float attackRadius = 1f;

    [Header("Knockback")]
    [SerializeField] private Vector2 knockBackPower = new Vector2(5f, 5f);
    [SerializeField] private Vector2 heavyKnockBackPower = new Vector2(10f, 10f);
    [SerializeField] private float knockBackDuration = .1f;
    public Coroutine knockbackCo;
    [SerializeField] private float heavyKnockBackThreshold = .3f;
    public bool isKnockBack;

    public bool canTrigger;

    protected virtual void Awake()
    {
        entityStats = GetComponent<Entity_Stats>();
        entityCombat = GetComponent<Entity_Combat>();
        entityHealth = GetComponent<Entity_Health>();
        entityVFX = GetComponent<Entity_VFX>();

        stateMachine = new();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0f;
    }

    protected virtual void Start()
    {
        RefreshStats();
        entityStats.OnStatChanged += RefreshStats;
    }

    private void OnDestroy()
    {
        entityStats.OnStatChanged -= RefreshStats;
    }

    protected virtual void Update()
    {
        stateMachine.currentState?.Update();
    }

    protected virtual void OnEnable()
    {
        if (rb != null && rb.simulated == false)
            rb.simulated = true;

        if (col != null && col.enabled == false)
            col.enabled = true;

        if (isKnockBack == true)
            isKnockBack = false;
    }

    public virtual void TryToDieState()
    {

    }

    private void RefreshStats()
    {
        moveSpeed = entityStats.GetSpeed();
        attackSpeed = entityStats.GetStatByType(StatType.AttackSpeed).GetValue();
    }

    public void SetVelocity(float x, float y)
    {
        if (isKnockBack)
            return;

        rb.linearVelocity = new(x, y);
    }

    public void KnockBack(Transform damagedDealer, float averangeDamage)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);

        Vector2 knockbackDir = KnockBackDir(damagedDealer, averangeDamage);

        knockbackCo = StartCoroutine(KnockbackCo(knockbackDir, knockBackDuration));
    }

    private IEnumerator KnockbackCo(Vector2 knockbackDir, float duration)
    {
        isKnockBack = true;
        rb.linearVelocity = new(knockbackDir.x, knockbackDir.y);
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
        isKnockBack = false;
    }

    private Vector2 KnockBackDir(Transform damagedDealer, float averageDamage)
    {
        Vector2 direction = ((Vector2)(transform.position - damagedDealer.position)).normalized;

        Vector2 knockback = averageDamage > heavyKnockBackThreshold ? heavyKnockBackPower : knockBackPower;

        knockback *= direction;

        return knockback;
    }

    public void SetAnimIdleAndAttackAnimation()
    {
        anim.SetFloat("xIdleAndAttack", xIdleAndAttack);
        anim.SetFloat("yIdleAndAttack", yIdleAndAttack);
    }

    public void MovementAnimation(Vector2 direction)
    {
        anim.SetFloat("xMove", Mathf.Round(direction.x));
        anim.SetFloat("yMove", Mathf.Round(direction.y));
    }

    public void SetValueIdleAndAttackAnimation(Vector2 direction)
    {
        xIdleAndAttack = direction.x;
        yIdleAndAttack = direction.y;
    }
}
