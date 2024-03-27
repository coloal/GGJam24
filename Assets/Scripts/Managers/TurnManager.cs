using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TurnManager : MonoBehaviour
{
    GameStates CurrentGameState;
    StoryManager StoryManager {
        get {
            return GameManager.Instance.ProvideStoryManager();
        }
    }
    CardsManager CardsManager {
        get {
            return MainGameSceneManager.Instance.ProvideCardsManager();
        }
    }

    StoryCard CurrentCard;

    void Start()
    {
        
    }

    void GetNewCard(StoryCardTemplate nextCard)
    {
        SetGameState(GameStates.SHOW_CARD);
        GameObject SpawnedCard = CardsManager.SpawnNextCard(nextCard);
        CurrentCard = SpawnedCard.GetComponent<StoryCard>();
        SetGameState(GameStates.MAKE_DECISION);
    }

    public void StartTurn() 
    {

        StepInfo nextStepInfo = StoryManager.ContinueStoryExecution();
       
        if(nextStepInfo == null )
        {
            Debug.LogError("Something went wrong");
        }
        //Nodo de carta de historia
        else if (nextStepInfo is StoryStep storyStep)
        {
            if(storyStep.StoryCard == null)
            {
                Debug.LogError("Story Card Node with no Card");
                Debug.LogError("Something went wrong");
                //GameManager.Instance.ProvideEndManager().FinishGameDeckEmpty();
            }
            else
            {
                GetNewCard(storyStep.StoryCard);
            }
        }
        //Nodo de carta de batalla
        else if (nextStepInfo is CombatStep combatStep)
        {
            if (combatStep.CombatCard == null)
            {
                Debug.LogError("Combat Node with no Card");
                Debug.LogError("Something went wrong");
                //GameManager.Instance.ProvideEndManager().FinishGameDeckEmpty();
            }
            else
            {
                GameManager.Instance.StartCombat(combatStep.CombatCard);
            }
        }
        //Nodo de carta de final
        else if (nextStepInfo is EndStep endStep)
        {
            //GameManager.Instance.ProvideEndManager().FinishGameDeckEmpty();
            Debug.LogWarning("Your deck is empty!");
        }
        else
        {
            Debug.LogError("Something went wrong");
        }
    }

    public void SwipeLeft()
    {
        GameManager.Instance.ProvideBrainManager().ExecuteActions(CurrentCard.leftActions);
        if (CurrentCard != null)
        {
            DestroyCard();
        }
        GameUtils.CreateTemporizer(() => StartTurn(), 0.5f, this);
        StoryManager.SwipeLeft();
    }

    public void SwipeRight()
    {
        GameManager.Instance.ProvideBrainManager().ExecuteActions(CurrentCard.rightActions);
        if (CurrentCard != null)
        {
            DestroyCard();
        }
        GameUtils.CreateTemporizer(() => StartTurn(), 0.5f, this);
        StoryManager.SwipeRight();
    }

    public void WinCombat(bool captured)
    {
        StoryManager.WinCombat(captured);
        StartTurn();
    }

    public void LoseCombat()
    {
        StoryManager.LoseCombat();
        StartTurn();
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
        //AudioManager.Instance.Play(SoundNames.PickPhone);

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

            GameUtils.createTemporizer(() => {
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
        GameUtils.CreateTemporizer(() => Destroy(CardToDestroy), 1, this);
    }

    /*private void ExecuteActions(List<Option> Actions)
    {
        foreach (Option action in Actions)
        {
            switch (action.TagType)
            {
                case BrainTagType.Bool:
                    //action.BrainBoolTagAction.Invoke(action.BoolTag, action.NewValue);
                    GameManager.Instance.ProvideBrainManager().SetTag(action.BoolTag, action.NewValue );
                    break;
                case BrainTagType.Numeric:
                    //action.BrainNumericTagAction.Invoke(action.NumericTag, action.Increment);
                    GameManager.Instance.ProvideBrainManager().IncrementNumericTag(action.NumericTag, action.Increment);

                    break;
                case BrainTagType.State:
                    //action.BrainStateIntTagAction.Invoke(action.StateTuple.selectedTag, action.StateTuple.selectedTagState);
                    GameManager.Instance.ProvideBrainManager().SetState(action.TagState, action.NewState);
                    break;
                case BrainTagType.Combat:
                    //Enter in combat
                    Debug.Log("Combateeee");
                    GameObject a = GameManager.Instance.ProvideCardsManager().SpawnCombatCard();
                    break;
            }
        }
    }*/
}