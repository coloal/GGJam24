using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : BaseSceneManager
{
    public void GoToMainGame() {
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }
    public void GoToCreditsMenu() {
        SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
    }
}
