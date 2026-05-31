using TMPro;
using UnityEngine;

public class UI_Wave : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveName;
    [SerializeField] private TextMeshProUGUI countdown;

    public void SetWaveInfo(string waveName, float elapsedTime, WaveData currentWave)
    {
        this.waveName.text = waveName;
        this.waveName.color = GameColors.SoulCorrupted;

        if (currentWave == null)
        {
            countdown.text = "";
            return;
        }
        float timeLeft = currentWave.endTime - elapsedTime;
        if (timeLeft < 0f) timeLeft = 0f;
        countdown.text = $"{timeLeft:F1}s";
        countdown.color = timeLeft <= 5f ? Color.red : GameColors.Soul;
    }
}
