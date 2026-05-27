using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private Player player;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("BGM Volume Settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParameter;

    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParameter;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    public void BGMSliderValue(float value)
    {
        audioMixer.SetFloat(bgmParameter, Mathf.Log10(value) * mixerMultiplier);
    }

    public void SFXSliderValue(float value)
    {
        audioMixer.SetFloat(sfxParameter, Mathf.Log10(value) * mixerMultiplier);
    }

    //public void GoMainMenuBtn() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .6f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }

    public void LoadUpVolume()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .6f);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
    }
}
