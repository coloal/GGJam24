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
        StartCoroutine(LoadSplash());
    }

    private void Update()
    {
        /*if (!HasInitialize)
        {
            if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
            {
                HasInitialize = true;
                Debug.Log("Se han cargado los bancos");
                GameManager.Instance.ProvideSoundManager().Initialize();
                
                Initializate();
            }

        }*/
    }

    IEnumerator LoadSplash()
    {
        yield return new WaitForSeconds(1);

        if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            Initializate();
            Debug.Log("Master bank cargado");

        }
        else
        {
            Debug.Log("Master bank not loaded looping");
            StartCoroutine(LoadSplash());
        }
    }

        void GoToMainMenu()
    {
        GameManager.Instance.ChangeSceneWithAnimation(mainMenuAnimation, "CombatScene");
    }

    async void Initializate() 
    {
        Init();

        GameManager.Instance.ProvideSoundManager().Initialize();
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
