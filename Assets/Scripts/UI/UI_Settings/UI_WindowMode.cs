using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WindowMode : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private const string WINDOW_MODE_KEY = "WindowMode";

    private readonly List<string> options = new()
    {
        "Fullscreen",
        "Borderless Windowed",
        "Windowed",
    };

    private void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(OnChanged);

        LoadWindowMode();
    }

    private void LoadWindowMode()
    {
        int savedIndex = PlayerPrefs.GetInt(WINDOW_MODE_KEY, GetCurrentModeIndex());
        savedIndex = Mathf.Clamp(savedIndex, 0, options.Count - 1);

        dropdown.SetValueWithoutNotify(savedIndex);
        dropdown.RefreshShownValue();

        ApplyWindowMode(savedIndex);
    }

    private int GetCurrentModeIndex()
    {
        return Screen.fullScreenMode switch
        {
            FullScreenMode.FullScreenWindow => 0,
            FullScreenMode.ExclusiveFullScreen => 1,
            FullScreenMode.Windowed => 2,
            _ => 0
        };
    }

    public void OnChanged(int index)
    {
        ApplyWindowMode(index);

        PlayerPrefs.SetInt(WINDOW_MODE_KEY, index);
        PlayerPrefs.Save();
    }

    private void ApplyWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
}