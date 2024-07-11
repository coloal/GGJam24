using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] Image background;
    [SerializeField] GameObject transitionObject;
    [SerializeField] float waitTimeBetweenTransition;
    StoryManager StoryManager {
        get {
            return GameManager.Instance.ProvideStoryManager();
        }
    }
    CardsManager CardsManager {
        get {
            if (GameManager.Instance.ProvideBrainManager().GetActualGraphType() == GraphTypes.Story)
            {
                return MainGameSceneManager.Instance.ProvideCardsManager();
            }
            else
            {
                return CreditsSceneManager.Instance.ProvideCardsManager();
            }
        }
    }

    StoryCard CurrentCard;

    void Start()
    {
        
    }

    void GetNewCard(StoryCardTemplate nextCard)
    {
        GameObject SpawnedCard;
        if (GameManager.Instance.ProvideBrainManager().GetActualGraphType() == GraphTypes.Story)
        {
            SpawnedCard = CardsManager.SpawnNextCard(
            nextCard,
            onSwipeLeft: () => { MainGameSceneManager.Instance.ProvideTurnManager().SwipeLeft(); },
            onSwipeRight: () => { MainGameSceneManager.Instance.ProvideTurnManager().SwipeRight(); });
            CurrentCard = SpawnedCard.GetComponent<StoryCard>();
        }
        else
        {
            SpawnedCard = CardsManager.SpawnNextCard(
            nextCard,
            onSwipeLeft: () => { CreditsSceneManager.Instance.ProvideTurnManager().SwipeLeft(); },
            onSwipeRight: () => { CreditsSceneManager.Instance.ProvideTurnManager().SwipeRight(); });
            CurrentCard = SpawnedCard.GetComponent<StoryCard>();
        }
        
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
            if (combatStep.Enemy == null)
            {
                Debug.LogError("Combat Node with no Enemy");
                Debug.LogError("Something went wrong");
                //GameManager.Instance.ProvideEndManager().FinishGameDeckEmpty();
            }
            else
            {
                
                //Call start combat
                
                GameManager.Instance.StartCombat(combatStep.Enemy, combatStep.IsBossFigth);
            }
        }
        else if (nextStepInfo is ChangeZoneStep zoneStep)
        {
            if (zoneStep.Zone == null)
            {
                Debug.LogError("Zone Node with no Zone Info");
                Debug.LogError("Something went wrong");
            }
            else
            {
                GameManager.Instance.ProvideBrainManager().ChangeZone(zoneStep.Zone);
                GameManager.Instance.ProvideSoundManager().ChangeZone(zoneStep.Zone.StoryMusicZone);

                TransitionToZone(zoneStep.Zone);
            }
        }
        else if (nextStepInfo is WaitStep waitStep)
        {
            GameUtils.CreateTemporizer(() => StartTurn(), waitStep.Seconds, this);
        }
        //Nodo de carta de final
        else if (nextStepInfo is EndStep endStep)
        {
            
            GraphTypes graphType = GameManager.Instance.ProvideBrainManager().GetActualGraphType();

            if (graphType == GraphTypes.Story)
            {
                GameManager.Instance.ProvideSoundManager().StopLevelMusic();
                GameManager.Instance.SetHasAStoryStarted(false);

                GameManager.Instance.ChangeSceneWithAnimation(GameManager.Instance.ProvideBrainManager().ZoneInfo.ZoneTransition, ScenesNames.CreditsMenuScene);
                //SceneManager.LoadScene(ScenesNames.CreditsMenuScene);
            }
            else
            {
                GameManager.Instance.ProvideSoundManager().StopCreditsMusic();
                GameManager.Instance.SetHasAStoryStarted(false);

                GameManager.Instance.ChangeSceneWithAnimation(GameManager.Instance.ProvideBrainManager().ZoneInfo.ZoneTransition, ScenesNames.MainMenuScene);

                //SceneManager.LoadScene(ScenesNames.MainMenuScene);
            }
            //SceneManager.LoadScene(ScenesNames.GameOverScene);
        }
        else
        {
            Debug.LogError("Something went wrong");
        }
    }


    public void TransitionToZone(ZoneTemplate zoneInfo)
    {
        Animator transition = zoneInfo.ZoneTransition;
        Animator instantedAnimator = Instantiate(transition.gameObject).GetComponent<Animator>();
        if(instantedAnimator != null)
        {
            instantedAnimator.SetTrigger("ExitAnimation");
        }
        GameUtils.CreateTemporizer(() => {
            SetZoneSprites();
            instantedAnimator.SetTrigger("EnterAnimation");
        }, 1 + waitTimeBetweenTransition, this);
        GameUtils.CreateTemporizer(() =>
        {
            GameManager.Instance.ProvideSoundManager().ChangeZone(zoneInfo.StoryMusicZone);
            StartTurn();
            Destroy(instantedAnimator.gameObject);
        }, 2 + waitTimeBetweenTransition, this);
    }

    public void SetZoneSprites()
    {

        Sprite zoneBackgroundSprite = GameManager.Instance.ProvideBrainManager().ZoneInfo.StoryBackgroundSprite;
        if (zoneBackgroundSprite != null)
        {
            background.sprite = zoneBackgroundSprite;
        }
        if(GameManager.Instance.ProvideBrainManager().ZoneInfo.ZoneMaterial != null)
        {
            background.material = GameManager.Instance.ProvideBrainManager().ZoneInfo.ZoneMaterial;
        }
        else
        {
            background.material = null;
        }
    }

    public void SwipeLeft()
    {
        if (CurrentCard != null)
        {
            DestroyCard();
        }
        GameUtils.CreateTemporizer(() => StartTurn(), 0.5f, this);
        GameManager.Instance.ProvideSoundManager().PlayCardSound(CardSounds.Left);
        StoryManager.SwipeLeft();
    }

    public void SwipeRight()
    {
        if (CurrentCard != null)
        {
            DestroyCard();
        }
        GameUtils.CreateTemporizer(() => StartTurn(), 0.5f, this);
        GameManager.Instance.ProvideSoundManager().PlayCardSound(CardSounds.Right);
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

    private void DestroyCard()
    {
        GameObject CardToDestroy = CurrentCard.gameObject;
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