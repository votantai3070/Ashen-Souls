using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string musicGroupName;

    private void Awake()
    {
        AudioManager.instance.StartBGM(musicGroupName);
    }
}
