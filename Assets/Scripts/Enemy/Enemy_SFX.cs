using UnityEngine;

public class Enemy_SFX : Entity_SFX
{
    [SerializeField] protected string deadGroupName;
    [SerializeField] protected string runGroupName;
    [SerializeField] protected string attackGroupName;

    public override void PlayFootstep()
    {
        AudioManager.instance.PlaySFX(runGroupName, audioSource);
    }

    public override void PlayAttack()
    {
        AudioManager.instance.PlaySFX(attackGroupName, audioSource);
    }

    public override void PlayDeath()
    {
        AudioManager.instance.PlaySFX(deadGroupName, audioSource);
    }
}
