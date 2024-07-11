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

    private bool HasInitialize = false;

    async void Start()
    {
        /*
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
        */
    }

    private void Update()
    {
        if (!HasInitialize)
        {
            if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
            {
                HasInitialize = true;
                Debug.Log("Se han cargado los bancos");
                GameManager.Instance.ProvideSoundManager().Initialize();
                
                Initializate();
            }

        }
    }

    void GoToMainMenu()
    {
        GameManager.Instance.ChangeSceneWithAnimation(mainMenuAnimation, ScenesNames.MainMenuScene);
    }

    async void Initializate() 
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
}
