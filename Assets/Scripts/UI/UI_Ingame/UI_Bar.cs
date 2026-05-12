using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    [SerializeField] private Image barFill;
    [SerializeField] private Image barDelayed;

    [SerializeField] private float delayTime = 0.4f;

    public void SetFill(float percent)
    {
        percent = Mathf.Clamp01(percent);

        barFill.DOFillAmount(percent, delayTime)
            .SetEase(Ease.OutCubic);

        if (barDelayed == null) return;

        barDelayed.DOKill();
        barDelayed.DOFillAmount(percent, 0.6f)
            .SetEase(Ease.OutCubic)
            .SetDelay(delayTime);
    }
}