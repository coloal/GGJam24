using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneManager : BaseSceneManager
{
    // Start is called before the first frame update
    [SerializeField] 
    TurnManager turnManager;
    [SerializeField]
    CardsManager cardsManager;
    [SerializeField]
    EndManager endManager;
    

    private void Start()
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
