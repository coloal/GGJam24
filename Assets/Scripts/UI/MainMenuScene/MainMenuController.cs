using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    Animator CardsFallingAnimation;

    void Start()
    {
        //AudioManager.Instance.Play(SoundNames.MenuBGM);
    }

    public void GoToMainGame() {
        //AudioManager.Instance.Play(SoundNames.StartGame);
        CardsFallingAnimation.Play(AnimationNames.CardsFallingAnimation);
        GameUtils.createTemporizer(() => {
            SceneManager.LoadScene(ScenesNames.MainGameScene);
        }, 1.3f, this);
    }
     public void GoToCredits() {
        SceneManager.LoadScene(ScenesNames.CreditsScene);
    }
    public void GoToMainMenu() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
