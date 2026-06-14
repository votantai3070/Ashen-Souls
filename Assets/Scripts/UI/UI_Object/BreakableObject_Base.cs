using UnityEngine;

public class BreakableObject_Base : MonoBehaviour, IDamageable
{
    public Transform player { get; private set; }
    private DropSystem dropSystem;


    [SerializeField] private Stat health;
    [SerializeField] private float currentHealth;

    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float detectionRadius = 10f;

    private void Awake()
    {
        dropSystem = GetComponent<DropSystem>();
        ResetState();
    }

    private void Update()
    {
        if (player == null)
            FindPlayer();
    }

    private void OnEnable()
    {
        ResetState();
    }

    private void ResetState()
    {
        if (health != null)
            currentHealth = health.GetValue();
    }


    public bool TakeDamage(bool isCrit, float damage, Transform damagedDealer)
    {
        if (currentHealth <= 0)
            return false;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyObject();
        }

        return true;
    }

    private void DestroyObject()
    {
        dropSystem?.SpawnDrop(transform.position);
        ObjectPool.instance.Despawn(gameObject);
    }

    private void FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, whatIsPlayer);
        if (playerCollider != null)
        {
            player = playerCollider.transform;
        }
    }
}