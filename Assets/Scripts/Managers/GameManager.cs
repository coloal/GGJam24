using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    TurnManager TurnManager;

    [SerializeField]
    StatsManager StatsManager;

    [SerializeField]
    CardsManager CardsManager;

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
        StartGame();
    }

    void StartGame() 
    {
        TurnManager.StartTurn();
    }

    // Maybe we need several FinishGame functions for every final that the game has
    public void FinishGame() 
    {
        Debug.Log("The game has finished! Congratulations ...or maybe not?");
    }

    public StatsManager ProvideStatsManager()
    {
        return StatsManager;
    }

    public CardsManager ProvideCardsManager()
    {
        return CardsManager;
    }

    public TurnManager ProvideTurnManager()
    {
        return TurnManager;
    }
}
