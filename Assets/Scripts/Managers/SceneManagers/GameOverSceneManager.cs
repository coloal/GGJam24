using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : BaseSceneManager
{
    public static GameOverSceneManager Instance;

    [Header("Scene configurations")]
    [SerializeField] private StoryCardTemplate gameOverCardTemplate;

    [Header("Scene managers")]
    [SerializeField] private CardsManager cardsManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Init();
        cardsManager.SpawnNextCard(
            gameOverCardTemplate,
            onSwipeLeft: () => {
                if (Application.isEditor)
                {
                    ResetGame();
                }
                else
                {
                    Application.Quit();
                }
            },
            onSwipeRight: () => { ResetGame(); });
    }

    void ResetGame()
    {
        GameManager.Instance.ResetGame();
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }

    public CardsManager ProvideCardsManager()
    {
        return cardsManager;
    }
}
