using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void Update()
    {
        Vector2 direction = new Vector2(entity.xIdleAndAttack, entity.yIdleAndAttack).normalized;
        transform.position = entity.transform.position + (Vector3)(direction * entity.attackRange);
    }

    private void OnValidate()
    {
        gameObject.name = transform.root.name + " - " + "AttackArea";
    }
}
