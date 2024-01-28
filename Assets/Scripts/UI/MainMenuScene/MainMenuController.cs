using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Play(SoundNames.MenuBGM);
    }

    public void GoToMainGame() {
        AudioManager.Instance.Play(SoundNames.StartGame);
        Utils.createTemporizer(() => {
            SceneManager.LoadScene(ScenesNames.MainGameScene);
        }, 1.0f, this);
    }
     public void GoToCredits() {
        SceneManager.LoadScene(ScenesNames.CreditsScene);
    }
    public void GoToMainMenu() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
