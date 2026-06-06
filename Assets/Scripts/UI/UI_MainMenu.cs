using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_Settings>(true).LoadUpVolume();
        transform.root.GetComponentInChildren<UI_FadeScreen>().FadeIn();

        AudioManager.instance.StartBGM("playlist_mainMenu");
    }


}
