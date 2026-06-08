using UnityEngine;

public class FireBall_ShotState : FireBall_State
{
    public FireBall_ShotState(SkillObject_FireBall spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(spellSkill, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (fireBall.target == null)
        {
            fireBall.OnHit();
            return;
        }

        RotationToEnemy();

        fireBall.transform.position = Vector3.MoveTowards(
            fireBall.transform.position,
            fireBall.target.position,
            fireBall.speed * Time.deltaTime
        );

        if (Vector2.Distance(fireBall.transform.position, fireBall.target.position) < .1f)
            stateMachine.ChangeState(fireBall.explodeState);

    }

    private void RotationToEnemy()
    {
        Vector2 dir = (fireBall.target.position - fireBall.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        fireBall.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
