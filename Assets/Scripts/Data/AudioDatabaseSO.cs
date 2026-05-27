using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class AudioDatabaseSO : ScriptableObject
{
    public List<AudioClipData> player;
    public List<AudioClipData> uiAudio;

    [Header("Music Lists")]
    public List<AudioClipData> mainMenuMusic;
    public List<AudioClipData> levelMusic;

    public Dictionary<string, AudioClipData> clipCollection;

    private void OnEnable()
    {
        clipCollection = new Dictionary<string, AudioClipData>();

        AddToCollection(player);
        AddToCollection(uiAudio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
    }

    public AudioClipData Get(string groupName)
    {
        return (clipCollection.TryGetValue(groupName, out AudioClipData data)) ? data : null;
    }

    private void AddToCollection(List<AudioClipData> listToAdd)
    {
        foreach (var data in listToAdd)
        {
            if (data != null && clipCollection.ContainsKey(data.audioName) == false)
            {
                clipCollection.Add(data.audioName, data);
            }
        }
    }
}

[Serializable]
public class AudioClipData
{
    public string audioName;
    public List<AudioClip> clips = new();
    [Range(0f, 1f)] public float maxVolume = 1f;

    public AudioClip GetRandomClip()
    {
        if (clips.Count == 0 && clips == null)
        {
            Debug.LogWarning($"No audio clips found for {audioName}");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, clips.Count);
        return clips[randomIndex];
    }
}