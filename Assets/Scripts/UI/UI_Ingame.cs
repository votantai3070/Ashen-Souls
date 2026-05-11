using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    private UI ui;

    [Header("Health Bar")]
    [SerializeField] private Image healthBarFilter;
    [SerializeField] private Image healthBarDelayed;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private float healthDelayTime = 0.4f;
    [SerializeField] private float lerpSpeed = 3f;
    private float healthTargetFill;
    private Coroutine delayCoroutine;

    [Header("Exp Bar")]
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    private void Start()
    {
        ui.player.health.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBarFilter == null) return;

        int currentHealth = ui.player.health.GetCurrentHealth();
        float maxHealth = ui.player.stats.GetMaxHealth();
        float percent = currentHealth / maxHealth;

        healthText.text = $"{currentHealth}/{maxHealth}";

        healthBarFilter.fillAmount = percent;
        healthTargetFill = percent;

        if (percent > 0.5f)
            healthBarFilter.color = GameColors.HPHigh;
        else if (percent > 0.25f)
            healthBarFilter.color = Color.Lerp(GameColors.HPMid, GameColors.HPHigh, (percent - 0.25f) / 0.25f);
        else
            healthBarFilter.color = Color.Lerp(GameColors.HPLow, GameColors.HPMid, percent / 0.25f);

        if (delayCoroutine != null)
            StopCoroutine(delayCoroutine);
        delayCoroutine = StartCoroutine(DelayedBarRoutine(healthBarDelayed, healthTargetFill, healthDelayTime));
    }

    private IEnumerator DelayedBarRoutine(Image healthBarDelayed, float targetFill, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        while (Mathf.Abs(healthBarDelayed.fillAmount - targetFill) > 0.001f)
        {
            healthBarDelayed.fillAmount = Mathf.Lerp(
                healthBarDelayed.fillAmount,
                targetFill,
                Time.deltaTime * lerpSpeed
            );
            yield return null;
        }

        healthBarDelayed.fillAmount = targetFill;
        delayCoroutine = null;
    }

    public void UpdateExpBar(float current, float max)
    {
        expBar.fillAmount = current / max;

        if (expText != null)
            expText.text = $"{Mathf.FloorToInt(current)} / {Mathf.FloorToInt(max)}";
    }


    public void UpdateLevelText(int level)
    {
        if (levelText != null)
            levelText.text = $"{level}";
    }

    public void ShowLevelUpEffect(int newLevel)
    {
        // animation || effect
        Debug.Log($"[UI] Level Up → {newLevel}!");
    }
}
