using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    public UI_SkillHolder skillHolder { get; private set; }

    [Header("Health Bar")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Exp Bar")]
    [SerializeField] private UI_Bar expBar;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        skillHolder = GetComponentInChildren<UI_SkillHolder>(true);

        if (expBar == null)
            expBar = GetComponentInParent<UI_Bar>();
    }

    private void Start()
    {
        UI.instance.player.health.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBar == null) return;

        int currentHealth = UI.instance.player.health.GetCurrentHealth();
        float maxHealth = UI.instance.player.stats.GetMaxHealth();

        if (maxHealth <= 0f)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
            healthBar.fillAmount = 0f;
            return;
        }

        float percent = currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";

        healthBar.GetComponentInParent<UI_Bar>(true).SetFill(percent);

        if (percent > 0.5f)
            healthBar.color = GameColors.HPHigh;
        else if (percent > 0.25f)
            healthBar.color = Color.Lerp(GameColors.HPMid, GameColors.HPHigh, (percent - 0.25f) / 0.25f);
        else
            healthBar.color = Color.Lerp(GameColors.HPLow, GameColors.HPMid, percent / 0.25f);
    }

    public void UpdateExpBar(float current, float max)
    {
        if (max <= 0) return;

        float percent = current / max;
        expBar.SetFill(percent);

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
        UI.instance.player.sFX.PlayLevelUp();
    }
}
