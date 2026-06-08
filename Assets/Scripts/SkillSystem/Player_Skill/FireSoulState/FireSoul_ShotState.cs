using UnityEngine;

public class FireSoul_ShotState : FireSoul_State
{
    public FireSoul_ShotState(SkillObject_FireSoul spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(spellSkill, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (spellSkill.target == null)
        {
            spellSkill.OnHit();
            return;
        }

        RotationToEnenmy();

        spellSkill.transform.position = Vector3.MoveTowards(
            spellSkill.transform.position,
            spellSkill.target.position,
            spellSkill.speed * Time.deltaTime
        );

        if (Vector2.Distance(spellSkill.transform.position, spellSkill.target.position) < .1f)
            stateMachine.ChangeState(spellSkill.explodeState);

    }

    private void RotationToEnenmy()
    {
        Vector2 dir = (spellSkill.target.position - spellSkill.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        spellSkill.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
