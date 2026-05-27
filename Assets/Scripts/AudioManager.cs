using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioDatabaseSO audioDatabase;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private Transform player;

    private AudioClip lastMusicPlayed;
    private Coroutine currentBgmCo;
    private string currentMusicGroupName;
    [SerializeField] private bool bgmShouldPlay;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (bgmSource.isPlaying == false && bgmShouldPlay)
        {
            if (string.IsNullOrEmpty(currentMusicGroupName) == false)
                NextBGM(currentMusicGroupName);
        }

        if (bgmSource.isPlaying && bgmShouldPlay == false)
        {
            StopBGM();
        }
    }

    // Start playing BGM from the specified music group. If the same group is already playing,
    // it will continue without interruption.
    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if (musicGroup == currentMusicGroupName)
            return;

        NextBGM(musicGroup);
    }

    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentMusicGroupName = musicGroup;

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);

        currentBgmCo = StartCoroutine(SwitchMusicCo(musicGroup));
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);

        StartCoroutine(FadeVolumeCo(bgmSource, 0f, 1f));
    }

    private IEnumerator SwitchMusicCo(string musicGroup)
    {
        AudioClipData data = audioDatabase.Get(musicGroup);
        AudioClip nextClip = data.GetRandomClip();

        if (data == null || data.clips.Count == 0)
        {
            Debug.LogWarning($"AudioManager: Music group '{musicGroup}' not found or has no clips.");
            yield break;
        }

        if (data.clips.Count > 1)
        {
            while (nextClip == lastMusicPlayed)
                nextClip = data.GetRandomClip();
        }

        if (bgmSource.isPlaying)
            yield return FadeVolumeCo(bgmSource, 0f, 1f);

        lastMusicPlayed = nextClip;
        bgmSource.clip = nextClip;
        bgmSource.volume = 0f;
        bgmSource.Play();

        StartCoroutine(FadeVolumeCo(bgmSource, data.maxVolume, 1f));
    }

    private IEnumerator FadeVolumeCo(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5)
    {
        var data = audioDatabase.Get(soundName);
        if (data == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{soundName}' not found in database.");
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null) return;

        float maxVolume = data.maxVolume;
        float distanceToPlayer = Vector3.Distance(player.position, sfxSource.transform.position);

        float t = Mathf.Clamp01(1 - (distanceToPlayer / minDistanceToHearSound));

        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);  // Use a quadratic curve for smoother falloff
        sfxSource.pitch = Random.Range(0.95f, 1.05f); // Add slight pitch variation for more natural sound
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDatabase.Get(soundName);
        if (data == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{soundName}' not found in database.");
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f); // Add slight pitch variation for more natural sound
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }

    public void SetPlayer(Player playerTransform)
    {
        player = playerTransform.transform;
    }
}
