using UnityEngine;

public class TornadoObject_CheckWallAndObstacle : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 moveDir;
    [SerializeField] private float speed = 3f;
    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        moveDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = moveDir * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Obstacle"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            moveDir = Vector2.Reflect(moveDir, contact.normal).normalized;
            rb.linearVelocity = moveDir * speed;
        }
    }
}
