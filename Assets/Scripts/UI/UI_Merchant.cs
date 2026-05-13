using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private UI ui;
    public UI_PlayerStats playerStatsUI { get; private set; }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        playerStatsUI = GetComponentInChildren<UI_PlayerStats>(true);
        ui.OnPlayerSet += OnPlayerReady;
    }

    public void OnPlayerReady()
    {
        ui.player.stats.OnStatChanged += playerStatsUI.UpdateStatUI;
        playerStatsUI.UpdateStatUI();
    }

    private void OnDestroy()
    {
        ui.OnPlayerSet -= OnPlayerReady;
        if (ui.player != null)
            ui.player.stats.OnStatChanged -= playerStatsUI.UpdateStatUI;
    }
}
