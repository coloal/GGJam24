using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    GameObject OverlayImage;

    [SerializeField]
    GameObject PhoneObject;
    [SerializeField]
    TextMeshPro NameFeedbackBox;
    [SerializeField]
    TextMeshPro TextFeedbackBox;

    GameStates CurrentGameState;



    CardsManager CardsManager;
    StatsManager StatsManager;

    Card CurrentCard;

    void Start()
    {
        SetUpManagers();
        OverlayImage.SetActive(false);
        PhoneObject.SetActive(false);
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

        Utils.createTemporizer(() => CheckForEndGame(), 0.5f, this);
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
            GivePhoneFeedback(info.FeedbackName,info.FeedbackText);
        }
        
        
    }

    public void GivePhoneFeedback(string name, string text)
    {
        PhoneObject.SetActive( true );
        NameFeedbackBox.text = name;
        TextFeedbackBox.text = text;
    }

    public void AcceptPhoneBuble()
    {
        PhoneObject.SetActive(false);
        OverlayImage.SetActive(false);
        CurrentCard.GetComponent<Draggable>()?.FinalSwipeRight();
        DestroyCard();
        Utils.createTemporizer(() => CheckForEndGame(), 0.5f, this);
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
        OverlayImage.SetActive(true);
        CurrentCard.GoToBackGroundAndDeactivate();
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
        CalculateHitmanStats(selectedHitman);
        PhoneObject.SetActive(true);
    }


    private void DestroyCard()
    {

        GameObject CardToDestroy = CurrentCard.gameObject;
        CardToDestroy.GetComponent<BoxCollider2D>().enabled = false;
        Utils.createTemporizer(() => Destroy(CardToDestroy), 1, this);
    }

}