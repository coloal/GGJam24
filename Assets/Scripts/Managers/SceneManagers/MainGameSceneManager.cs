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
        turnManager.SetZoneSprites();

        if (GameManager.Instance.HasToResetGame())
        {
            turnManager.StartTurn();
            GameManager.Instance.SetHasToResetGame(false);
        }
    }


    public void StartGame()
    {
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