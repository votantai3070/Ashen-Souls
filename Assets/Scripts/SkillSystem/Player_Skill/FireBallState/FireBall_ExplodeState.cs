using System.Collections;
using UnityEngine;

public class FireBall_ExplodeState : FireBall_State
{
    public FireBall_ExplodeState(SkillObject_FireBall spellSkill, StateMachine<SpellState> stateMachine, string animBoolName) : base(spellSkill, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = anim.GetCurrentAnimatorStateInfo(0).length;
        fireBall.SetPhysicsActive(true);

        fireBall.transform.localScale = Vector3.zero;
        fireBall.StartCoroutine(ScaleUp());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            fireBall.OnHit();
    }

    private IEnumerator ScaleUp()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one * fireBall.checkDamageRadius;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fireBall.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / duration);
            yield return null;
        }

        fireBall.transform.localScale = targetScale;
    }
}
