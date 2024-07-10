using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : BaseSceneManager
{

    [SerializeField]
    Animator animator;

    private void Start()
    {
        Init();
        GameManager.Instance.ProvideSoundManager().PlayMenuMusic();
    }
    public void GoToMainGame() {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        GameManager.Instance.ChangeSceneWithAnimation(animator, ScenesNames.MainGameScene);

        //SceneManager.LoadScene(ScenesNames.MainGameScene);
    }
    public void GoToCreditsMenu() {
        GameManager.Instance.ProvideSoundManager().StopMenuMusic();
        GameManager.Instance.ChangeSceneWithAnimation(animator, ScenesNames.CreditsMenuScene);
        
        //SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
    }
}
