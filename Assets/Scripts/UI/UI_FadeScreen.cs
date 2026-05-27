using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    public Coroutine fadeEffectCo { get; private set; }
    private Image fadeImage;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        //fadeImage.color = new Color(0, 0, 0, 1);
    }

    public void FadeIn(float duration = 1)
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        FadeEffect(0f, duration);
    }

    public void FadeOut(float duration = 1)
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        FadeEffect(1f, duration);
    }

    private void FadeEffect(float targetAlpha, float duration)
    {
        if (fadeEffectCo != null)
            StopCoroutine(fadeEffectCo);
        fadeEffectCo = StartCoroutine(FadeEffectCo(targetAlpha, duration));
    }

    private IEnumerator FadeEffectCo(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            var newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }
}
