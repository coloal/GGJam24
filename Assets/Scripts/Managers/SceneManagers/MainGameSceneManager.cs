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
    [SerializeField]
    EndManager endManager;
    
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
        StartGame();
    }

    void StartGame()
    {
        turnManager.StartTurn();
    }

    public TurnManager ProvideTurnManager()
    {
        return turnManager;
    }
    public EndManager ProvideEndManager()
    {
        return endManager;
    }
    public CardsManager ProvideCardsManager()
    {
        return cardsManager;
    }

    
}
