using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneManager : BaseSceneManager
{
    public static MainGameSceneManager Instance;
    
    [SerializeField] 
    TurnManager turnManager;
    [SerializeField]
    CardsManager cardsManager;
    
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

    private void Start()
    {
        Init();

        if (!GameManager.Instance.HasAStoryStarted())
        {
            StartStory();
        }
    }

    public void StartStory()
    {
        GameManager.Instance.SetHasAStoryStarted(true);
        GameManager.Instance.ProvideSoundManager().StartGame();
        turnManager.SetZoneSprites();
        turnManager.StartTurn();
    }

    public TurnManager ProvideTurnManager()
    {
        return turnManager;
    }
    
    public CardsManager ProvideCardsManager()
    {
        return cardsManager;
    }

    
}
