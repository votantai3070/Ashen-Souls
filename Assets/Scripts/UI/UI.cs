using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public event Action OnSkillSlotChange;
    public event Action OnPlayerSet;

    private Button[] buttons;

    public static UI instance { get; private set; }
    public Player player { get; private set; }

    public GameObject[] uiElements;
    public UI_Ingame ingameUI { get; private set; }
    public UI_SkillBoard skillBoardUI { get; private set; }
    public UI_Stats statsUI { get; private set; }
    public UI_TotalSummary totalSummaryUI { get; private set; }
    public UI_Settings settingsUI { get; private set; }
    public UI_FadeScreen fadeUI { get; private set; }

    private Coroutine openSummaryCo;

    private void Awake()
    {
        instance = this;

        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        skillBoardUI = GetComponentInChildren<UI_SkillBoard>(true);
        statsUI = GetComponentInChildren<UI_Stats>(true);
        totalSummaryUI = GetComponentInChildren<UI_TotalSummary>(true);
        settingsUI = GetComponentInChildren<UI_Settings>(true);
        fadeUI = GetComponentInChildren<UI_FadeScreen>(true);
    }

    private void Start()
    {
        RegisterAllButtonSounds();
    }

    private void RegisterAllButtonSounds()
    {
        buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(AudioManager.instance.PlayButtonClickSFX);
            button.onClick.AddListener(AudioManager.instance.PlayButtonClickSFX);
        }
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
        {
            Time.timeScale = 0f;
            ControlsManager.instance.inputActions.Player.Disable();
        }
        else
        {
            Time.timeScale = 1f;
            ControlsManager.instance.inputActions.Player.Enable();
        }
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

    public void OpenSkillBoard()
    {
        SwitchTo(skillBoardUI.gameObject);
        statsUI.playerStatsUI.UpdateStatUI();
        StopPlayerControlIfNeeded();
    }

    public void OpenSettings()
    {
        SwitchTo(settingsUI.gameObject);
        StopPlayerControlIfNeeded();
    }

    public void OpenSummary(float duration)
    {
        if (openSummaryCo != null)
            StopCoroutine(openSummaryCo);

        openSummaryCo = StartCoroutine(OpenTotalSummaryCo(duration));
    }

    private IEnumerator OpenTotalSummaryCo(float duration)
    {
        yield return new WaitForSeconds(duration);

        SwitchTo(totalSummaryUI.gameObject);
        totalSummaryUI.UpdateTotalSummary();
        StopPlayerControlIfNeeded();

        openSummaryCo = null;
    }

    public void SwitchToIngameUI()
    {
        StopPlayerControls(false);
        SwitchTo(ingameUI.gameObject);
    }

    public bool IsInGameUI() => ingameUI.gameObject.activeSelf;

    public bool IsSkillBoardUI() => skillBoardUI.gameObject.activeSelf;

    public void OnSkillSlotChangeInvoke()
    {
        OnSkillSlotChange?.Invoke();
    }
}
