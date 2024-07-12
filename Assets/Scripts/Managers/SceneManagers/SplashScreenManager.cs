using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenManager : BaseSceneManager
{
    [Header("Scene configurations")]
    [SerializeField] private SplashFeedbacksManager splashFeedbacksManager;
    [SerializeField] private float secondsBeforeStartingMainMenuScene = 1.0f;
    [SerializeField] private Animator mainMenuAnimation;
    [SerializeField] private Image gameLogo;
    [SerializeField] private GameObject button;

    private bool hasFinishedLoading = false;
    private bool hasFinishedLastCicle = false;
    private bool HasInitialize = false;
    private bool hasFinishedTemporizer = false;
    private bool faseOneHasFinished = false;
    private float faseOneTime = 2;
    private bool faseTwoHasFinished = false;
    private bool hasStarted = false;
    private float faseTwoTime = 4;
    private float lastCicleSign = 0;

    private float temporizer = 0;
    private float waitTime = 5;

    async void Start()
    {
        gameLogo.color = new Vector4(gameLogo.color.r, gameLogo.color.g, gameLogo.color.b, 0);
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

        if (hasFinishedLoading && !hasFinishedTemporizer)
        {
            temporizer += Time.deltaTime;
            if (!faseOneHasFinished && temporizer > faseOneTime)
            {
                faseOneHasFinished=true;
                GameManager.Instance.ProvideSoundManager().CreateEventFMOD("Menu");
            } 
            else if (!faseTwoHasFinished && temporizer > faseTwoTime)
            {
                faseTwoHasFinished=true;
                GameManager.Instance.ProvideSoundManager().CreateEventFMOD("Credits");
            }
            else if (temporizer > waitTime)
            {
                hasFinishedTemporizer = true;
                lastCicleSign = Mathf.Sign(Mathf.Sin(Time.time));
            }
        }

        if (hasStarted) 
        {
            if (!hasFinishedLastCicle)
            {
                if (hasFinishedTemporizer)
                {
                    if (Mathf.Sign(Mathf.Sin(Time.time * 3)) != lastCicleSign)
                    {
                        hasFinishedLastCicle = true;
                    }
                }
                gameLogo.color = new Vector4(gameLogo.color.r, gameLogo.color.g, gameLogo.color.b, Mathf.Abs(Mathf.Sin(Time.time * 3)));
            }
            else if (!HasInitialize)
            {
                HasInitialize = true;
                gameLogo.color = new Vector4(gameLogo.color.r, gameLogo.color.g, gameLogo.color.b, 0);
                Initializate();
            }
        }
        

    }


    public void StartGame() 
    {
        hasStarted = true;
        button.SetActive(false);
        gameLogo.gameObject.SetActive(true);
        StartCoroutine(LoadSplash());
    }

    IEnumerator LoadSplash()
    {
        yield return new WaitForSeconds(1);

        if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            hasFinishedLoading = true;
            GameManager.Instance.ProvideSoundManager().Initialize();
            GameManager.Instance.ProvideSoundManager().CreateEventFMOD("SplashAudio");
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
