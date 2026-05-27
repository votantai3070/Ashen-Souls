using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_Settings>(true).LoadUpVolume();

        //transform.root.GetComponentInChildren<UI_FadeScreen>().FadeIn();
        AudioManager.instance.StartBGM("playlist_mainMenu");
    }
    public void PlayBtn()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
        GameManager.instance.ChangeScene("GameView");
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
