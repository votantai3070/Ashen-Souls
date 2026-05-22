using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public event Action OnSkillSlotChange;
    public event Action OnPlayerSet;

    public static UI instance { get; private set; }
    public Player player { get; private set; }

    public GameObject[] uiElements;
    public UI_Ingame ingameUI { get; private set; }
    public UI_SkillBoard skillBoardUI { get; private set; }
    public UI_Stats statsUI { get; private set; }

    private void Awake()
    {
        instance = this;

        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        skillBoardUI = GetComponentInChildren<UI_SkillBoard>(true);
        statsUI = GetComponentInChildren<UI_Stats>(true);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        OnSkillSlotChange += ingameUI.skillHolder.SetupSkillSlots;
        OnPlayerSet?.Invoke();

        ingameUI.skillHolder.SetupSkillSlots();
    }

    private void OnDestroy()
    {
        if (ingameUI != null && ingameUI.skillHolder != null)
            OnSkillSlotChange -= ingameUI.skillHolder.SetupSkillSlots;
    }

    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            ControlsManager.instance.inputActions.Player.Disable();
        else
            ControlsManager.instance.inputActions.Player.Enable();
    }

    private void StopPlayerControlIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }

        StopPlayerControls(false);
    }

    private void SwitchTo(GameObject objectSwitching)
    {
        foreach (var element in uiElements)
            element.SetActive(false);

        objectSwitching.SetActive(true);
    }

    public void OpenStatBoard()
    {
        SwitchTo(statsUI.gameObject);
        StopPlayerControlIfNeeded();
    }

    public void OpenSkillBoard()
    {
        SwitchTo(skillBoardUI.gameObject);
        statsUI.playerStatsUI.UpdateStatUI();
        StopPlayerControlIfNeeded();
    }

    public void SwitchToIngameUI()
    {
        StopPlayerControls(false);
        SwitchTo(ingameUI.gameObject);
    }

    public void OnSkillSlotChangeInvoke()
    {
        OnSkillSlotChange?.Invoke();
    }
}
