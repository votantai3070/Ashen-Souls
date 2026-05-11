using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    [SerializeField] private Image barFill;
    [SerializeField] private Image barDelayed;

    [SerializeField] private float delayTime = 0.4f;
    [SerializeField] private float lerpSpeed = 3f;

    private Coroutine delayCoroutine;

    // Apply Health Bar -> Only Reduce
    public void SetFill(float percent)
    {
        percent = Mathf.Clamp01(percent);
        barFill.fillAmount = percent;

        if (barDelayed == null) return;

        if (delayCoroutine != null)
            StopCoroutine(delayCoroutine);

        delayCoroutine = StartCoroutine(DelayedBarRoutine(percent));
    }

    private IEnumerator DelayedBarRoutine(float targetFill)
    {
        yield return new WaitForSeconds(delayTime);

        while (Mathf.Abs(barDelayed.fillAmount - targetFill) > 0.001f)
        {
            barDelayed.fillAmount = Mathf.Lerp(
                barDelayed.fillAmount,
                targetFill,
                Time.deltaTime * lerpSpeed
            );
            yield return null;
        }

        barDelayed.fillAmount = targetFill;
        delayCoroutine = null;
    }
}