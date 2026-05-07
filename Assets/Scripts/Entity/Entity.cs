using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Entity_Combat entityCombat { get; private set; }
    public Entity_Stats entityStats { get; private set; }
    public Entity_VFX entityVFX { get; private set; }
    public StateMachine<EntityState> stateMachine { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

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
        entityVFX = GetComponent<Entity_VFX>();

        stateMachine = new();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();


        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0f;
    }

    protected virtual void Start()
    {
        attackSpeed = entityStats.GetStatByType(StatType.AttackSpeed).GetValue();
    }

    protected virtual void Update()
    {
        stateMachine.currentState?.Update();
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

        knockback.x *= direction.x;
        knockback.y *= direction.y;

        return knockback;
    }


}
