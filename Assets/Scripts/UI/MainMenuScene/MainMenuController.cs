using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    Animator CardsFallingAnimation;

    void Start()
    {

    }

    public void GoToMainGame() {
        CardsFallingAnimation.Play(AnimationNames.CardsFallingAnimation);
        GameUtils.CreateTemporizer(() => {
            SceneManager.LoadScene(ScenesNames.MainGameScene);
        }, 1.3f, this);
    }
}
