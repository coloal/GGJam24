using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void GoToMainGame() {
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }
     public void GoToCredits() {
        SceneManager.LoadScene(ScenesNames.CreditsScene);
    }
    public void GoToMainMenu() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
