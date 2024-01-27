using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    GameStates CurrentGameState;

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
        SetGameState(GameStates.SHOW_CARD);
        GameObject SpawnedCard = CardsManager.SpawnNextCard();
        CurrentCard = SpawnedCard.GetComponent<Card>();
        SetGameState(GameStates.MAKE_DECISION);
    }

    void CalculateStats() 
    {
        SetGameState(GameStates.STATS_CALCULATION);

        if (StatsManager != null && CurrentCard != null)
        {
            StatsManager.ModifyStats(
                CurrentCard.ViolenceStat,
                CurrentCard.MoneyStat,
                CurrentCard.InfluenceStat
            );
        }

        CheckForEndGame();
    }

    void CalculateHitmanStats(HitManTypes SelectedHitman)
    {
        SetGameState(GameStates.STATS_CALCULATION);

        if (StatsManager != null && CurrentCard != null)
        {
            HitmanInfo info = null;
            switch (SelectedHitman)
            {
                case HitManTypes.Maton:
                    info = CurrentCard.Maton;
                    break;
                case HitManTypes.Contable:
                    info = CurrentCard.Contable;
                    break;
                case HitManTypes.Comisario:
                    info = CurrentCard.Comisario;
                    break;
                default: break;
            }
            StatsManager.ModifyStats(
                info.ViolenceStat,
                info.MoneyStat,
                info.InfluenceStat
            );

        }

        CheckForEndGame();
    }

    public void CheckForEndGame()
    {
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
            DestroyCard();
        }
        CalculateStats();
    }

    public void SwipeRight()
    {
        Debug.Log("I swiped right");
        SetGameState(GameStates.PICK_A_HITMAN);
        // HitmenManager
    }

    void SetGameState(GameStates State)
    {
        CurrentGameState = State;
    }

    public GameStates GetCurrentGameState()
    {
        return CurrentGameState;
    }

    public void OnHitmenSelected(HitManTypes selectedHitman) {
        CalculateStats();
        CalculateHitmanStats(selectedHitman);
        CurrentCard.GetComponent<Draggable>()?.FinalSwipeRight();
        DestroyCard();
    }

    private void DestroyCard()
    {
        Utils.createTemporizer(() => Destroy(CurrentCard.gameObject), 1, this);
    }

}