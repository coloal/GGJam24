using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : BaseSceneManager
{
    [Header("Scene configurations")]
    [SerializeField] private SplashFeedbacksManager splashFeedbacksManager;
    [SerializeField] private float secondsBeforeStartingMainMenuScene = 1.0f;
    [SerializeField] private Animator mainMenuAnimation;

    async void Start()
    {
        Init();

        GameManager.Instance.ProvideSoundManager().PlaySFX("SplashAudio");

        await splashFeedbacksManager.PlayFadeImages();
        if (this == null || destroyCancellationToken.IsCancellationRequested)
        {
            return;
        }

        GameUtils.CreateTemporizer(() => 
        { 
            GoToMainMenu(); 
        }, secondsBeforeStartingMainMenuScene, this);
    }

    void GoToMainMenu()
    {
        GameManager.Instance.ChangeSceneWithAnimation(mainMenuAnimation, ScenesNames.MainMenuScene);
    }
}
