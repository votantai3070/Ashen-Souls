public class Enemy_AnimationEvents : EntityAnimationEvents
{
    private Enemy_Range enemyRange;

    protected override void Awake()
    {
        base.Awake();

        enemyRange = GetComponentInParent<Enemy_Range>();
    }

    //private void FinishDeadState() => entity.entityHealth.Die();

    private void TryUseEnergyBall()
    {
        enemyRange.skillManager.GetSkillEnemyByType(SkillEnemyType.EnergyBall).TryUseSkill();
    }
}
