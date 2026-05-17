using UnityEngine;

public class EntityAnimationEvents : MonoBehaviour
{
    protected Entity entity;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    protected virtual void TriggerEvent()
    {
        entity.canTrigger = true;
        entity.entityCombat.SetAttackWindow(false);
    }

    protected virtual void AttackTrigger()
    {
        entity.entityCombat.ResetHitList();
        //entity.entityCombat.Attack(entity);
        entity.entityCombat.SetAttackWindow(true);
    }
}