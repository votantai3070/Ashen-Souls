using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private string musicGroupName;

    public LevelSystem levelSystem = new LevelSystem();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);

        AudioManager.instance.StartBGM(musicGroupName);
    }

    public bool CanRewardMilestone()
    {
        return levelSystem.SkillCardReward();
    }
}
