using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuContainer;

    private bool isGameCurrentlyPaused;

    void Start()
    {
        isGameCurrentlyPaused = false;
        GameManager.Instance.ProvideInputManager().onPauseEvent += TogglePauseMenu;
    }

    void OnDisable()
    {
        GameManager.Instance.ProvideInputManager().onPauseEvent -= TogglePauseMenu;
    }

    void TogglePauseMenu()
    {
        if (isGameCurrentlyPaused)
        {
            ClosePauseMenu();
        }
        else 
        {
            pauseMenuContainer.SetActive(true);
            isGameCurrentlyPaused = true;
        }
    }

    void ClosePauseMenu()
    {
        pauseMenuContainer.SetActive(false);
        isGameCurrentlyPaused = false;
    }

    public void ResumeCurrentGame()
    {
        ClosePauseMenu();
    }

    public void ExitCurrentGame()
    {
        ClosePauseMenu();
        GraphTypes currentGraphType = GameManager.Instance.ProvideBrainManager().GetActualGraphType();

        // When we are in the Main Game or Battle Rush mode
        if (currentGraphType == GraphTypes.Story)
        {
            GameManager.Instance.ProvideSoundManager().StopLevelMusic();
            GameManager.Instance.ProvideSoundManager().RestartMusicFromCombat();
        }
        // When we are in the Credits scene
        else
        {
            GameManager.Instance.ProvideSoundManager().StopCreditsMusic();
        }

        GameManager.Instance.SetHasAStoryStarted(false);
        GameManager.Instance.ResetGame();
        GameManager.Instance.ChangeSceneWithAnimation(
            GameManager.Instance.ProvideBrainManager().ZoneInfo.ZoneTransition, 
            ScenesNames.MainMenuScene
        );
    }
}
