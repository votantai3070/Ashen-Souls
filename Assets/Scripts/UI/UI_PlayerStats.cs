using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI ui;
    private UI_StatSlot[] statSlots;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        statSlots = GetComponentsInChildren<UI_StatSlot>(true);
    }

    private void Start()
    {
        ui.OnPlayerSet += OnPlayerReady;
    }

    private void OnPlayerReady()
    {
        ui.player.stats.OnStatChanged += UpdateStatUI;
        UpdateStatUI();
    }

    private void OnDestroy()
    {
        ui.OnPlayerSet -= OnPlayerReady;

        if (ui.player != null)
            ui.player.stats.OnStatChanged -= UpdateStatUI;
    }

    private void UpdateStatUI()
    {
        foreach (var stat in statSlots)
            stat.UpdateStatValue();
    }
}
