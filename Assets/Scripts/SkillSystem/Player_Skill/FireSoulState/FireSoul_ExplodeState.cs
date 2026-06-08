
using System.Collections;
using UnityEngine;

public class FireSoul_ExplodeState : FireSoul_State
{
    public FireSoul_ExplodeState(SkillObject_FireSoul spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(spellSkill, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;
        spellSkill.SetPhysicsActive(true);

        spellSkill.transform.localScale = Vector3.zero;
        spellSkill.StartCoroutine(ScaleUp());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            spellSkill.OnHit();
    }

    private IEnumerator ScaleUp()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one * spellSkill.checkDamageRadius;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spellSkill.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / duration);
            yield return null;
        }

        spellSkill.transform.localScale = targetScale;
    }
}
