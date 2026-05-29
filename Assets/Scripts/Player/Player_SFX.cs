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
}
