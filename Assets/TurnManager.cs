using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    CardsManager CardsManager;
    StatsManager StatsManager;

    Card CurrentCard;

    void Start()
    {
        SetUpManagers();
    }

    void SetUpManagers()
    {
        CardsManager = GameManager.Instance.ProvideCardsManager();
        StatsManager = GameManager.Instance.ProvideStatsManager();
    }

    void GetNewCard()
    {
        GameObject SpawnedCard = CardsManager.SpawnNextCard();
        CurrentCard = SpawnedCard.GetComponent<Card>();
    }

    void CalculateStats() 
    {
        if (StatsManager != null && CurrentCard != null)
        {
            StatsManager.ModifyStats(
                CurrentCard.ViolenceStat,
                CurrentCard.MoneyStat,
                CurrentCard.InfluenceStat
            );
        }

        if (StatsManager.HasAStatBeenDepletedOrCompleted())
        {
            GameManager.Instance.FinishGame();
        }
        else if (CardsManager.IsDeckEmpty())
        {
            GameManager.Instance.FinishGame();
        }
        else
        {
            StartTurn();
        }
    }

    public void StartTurn() 
    {
        GetNewCard();
    }

    public void SwipeLeft() 
    {
        if (CurrentCard != null)
        {
            Destroy(CurrentCard.gameObject);
        }
        CalculateStats();
    }

    public void SwipeRight()
    {
        Debug.Log("I swiped right");
    }
}
