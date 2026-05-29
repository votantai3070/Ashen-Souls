public class EnemySlime_SFX : Entity_SFX
{
    public override void PlayFootstep()
    {
        AudioManager.instance.PlaySFX("slime_footstep", audioSource);
    }
}
