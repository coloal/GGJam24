using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : BaseSceneManager
{
    private void Start()
    {
        Init();
        GameManager.Instance.ProvideSoundManager().PlayMenuMusic();
    }
    public void GoToMainGame() {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }
    public void GoToCreditsMenu() {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
    }
}
