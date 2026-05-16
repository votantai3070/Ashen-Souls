using UnityEngine;

public class EntityAnimationEvents : MonoBehaviour
{
    private Entity entity;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    protected virtual void TriggerEvent() => entity.canTrigger = true;

    protected virtual void AttackTrigger()
    {
        entity.entityCombat.ResetHitList();
        entity.entityCombat.Attack(entity);
        //entity.entityCombat.SetAttackWindow(true);
    }

    protected virtual void AttackEnd()
    {
        //entity.entityCombat.SetAttackWindow(false);
    }
}