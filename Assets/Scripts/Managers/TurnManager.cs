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
    }

    void GetNewCard()
    {
        SetGameState(GameStates.SHOW_CARD);
        GameObject SpawnedCard = CardsManager.SpawnNextCard();
        CurrentCard = SpawnedCard.GetComponent<Card>();
        SetGameState(GameStates.MAKE_DECISION);
    }


    public void GivePhoneFeedback(string name, string text)
    {
        PhoneObject.SetActive( true );
        NameFeedbackBox.text = name;
        TextFeedbackBox.text = text;

        //TextFeedbackBox.GetComponent<PhoneTextWriter>().WriteTextCharByChar(text);
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
        if (CardsManager.IsDeckEmpty())
        {
            GameManager.Instance.ProvideEndManager().FinishGameDeckEmpty();
        }
        else //if (stoymanager.getNextCard != null)
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
        Utils.createTemporizer(() => CheckForEndGame(), 0.5f, this);
    }

    public void SwipeRight()
    {
        if (CurrentCard != null)
        {
            DestroyCard();
        }
        Utils.createTemporizer(() => CheckForEndGame(), 0.5f, this);

        /*
        SetGameState(GameStates.PICK_A_HITMAN);
        OverlayImage.SetActive(true);
        CurrentCard.GoToBackGroundAndDeactivate();
        */
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
        AudioManager.Instance.Play(SoundNames.PickPhone);

        // --Deprecated--
        //CalculateHitmanStats(selectedHitman);
    }

    void CalculateHitmanStats(HitManTypes SelectedHitman)
    {
        // --Deprecated--
        /*
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

            Utils.createTemporizer(() => {
                PhoneObject.SetActive(true);
                GivePhoneFeedback(info.FeedbackName, info.FeedbackText);
            }, 2.3f, this);

        }

        */
    }


    private void DestroyCard()
    {

        GameObject CardToDestroy = CurrentCard.gameObject;
        CardToDestroy.GetComponent<BoxCollider2D>().enabled = false;
        Utils.createTemporizer(() => Destroy(CardToDestroy), 1, this);
    }

}