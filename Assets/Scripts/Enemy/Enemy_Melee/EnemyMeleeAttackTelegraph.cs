using System;
using System.Collections;
using UnityEngine;

public class EnemyMeleeAttackTelegraph : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform lineStartPoint;
    [SerializeField] private float windupTime = 0.5f;

    private Coroutine telegraphCo;

    public bool isPlaying { get; private set; }

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponentInParent<Enemy>();

        if (line == null)
            line = GetComponent<LineRenderer>();

        line.positionCount = 2;
        line.enabled = false;
    }

    public void StartDash(Action onFinished = null)
    {
        if (telegraphCo != null)
            StopCoroutine(telegraphCo);

        telegraphCo = StartCoroutine(DashTelegraphCo(onFinished));
    }

    private IEnumerator DashTelegraphCo(Action onFinished)
    {
        isPlaying = true;

        line.enabled = true;
        line.positionCount = 2;

        Vector3 startPos = lineStartPoint != null ? lineStartPoint.position : transform.position;
        Vector2 dashDir = enemy.GetDirectionPlayer();
        Vector3 finalEnd = startPos + (Vector3)(dashDir * enemy.specialDistance);

        float timer = 0f;

        while (timer < windupTime)
        {
            float t = timer / windupTime;
            Vector3 currentEnd = Vector3.Lerp(startPos, finalEnd, t);

            line.SetPosition(0, startPos);
            line.SetPosition(1, currentEnd);

            timer += Time.deltaTime;
            yield return null;
        }

        line.SetPosition(0, startPos);
        line.SetPosition(1, finalEnd);

        yield return new WaitForSeconds(.2f);
        line.enabled = false;

        enemy.rb.linearVelocity = dashDir * enemy.specialSpeed;
        yield return new WaitForSeconds(enemy.specialDashDuration);
        enemy.rb.linearVelocity = Vector2.zero;

        isPlaying = false;
        telegraphCo = null;
        onFinished?.Invoke();
    }

    private void OnDisable()
    {
        if (telegraphCo != null)
            StopCoroutine(telegraphCo);

        telegraphCo = null;
        isPlaying = false;

        if (line != null)
            line.enabled = false;
    }
}