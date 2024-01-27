using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void GoToMainGame() {
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }
}
