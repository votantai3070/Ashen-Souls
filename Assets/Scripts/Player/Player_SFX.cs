public class Player_SFX : Entity_SFX
{
    public void PlayLevelUp()
    {
        AudioManager.instance.PlaySFX("level_up", audioSource);
    }

    public override void PlayAttack()
    {
        AudioManager.instance.PlaySFX("attack", audioSource);
    }

    public override void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX("attack_miss", audioSource);
    }

    public override void PlayHit()
    {
        AudioManager.instance.PlaySFX("hit", audioSource);
    }

    public override void PlayDeath()
    {
        AudioManager.instance.PlaySFX("death", audioSource);
    }
}
