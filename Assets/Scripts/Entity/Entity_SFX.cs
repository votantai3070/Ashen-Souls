using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;

    public virtual void PlayFootstep()
    {
    }

    public virtual void PlayAttack()
    {
    }

    public virtual void PlayAttackMiss()
    {
    }

    public virtual void PlayHit()
    {
    }
}
