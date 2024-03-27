using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    // Lists of cards to be added to the game
    [SerializeField]
    GameObject CardPrefab;

    [SerializeField]
    GameObject CombatCardPrefab;

    /*deprecated ????*/
    [SerializeField]
    List<StoryCardTemplate> DataCardsList;

    //For testing by Psyduck(adri :) )
    [SerializeField] private CombatCardTemplate CartaTESTING;

    [SerializeField]
    private bool isSequential;

    [SerializeField]
    private Transform CardSpawnerOrigin;

    private int indexNextCard = 0;

    public GameObject SpawnNextCard(StoryCardTemplate nextCard)
    {
        void SetUpOnSwipeLeftActions(GameObject storyCard)
        {
            InteractiveStoryCardComponent interactiveStoryCardComponent =
                storyCard.GetComponent<InteractiveStoryCardComponent>();
            StoryCard storyCardComponent = storyCard.GetComponent<StoryCard>();
            HorizontalEscapeMovementComponent storyCardHorizontalEscapeComponent =
                storyCard.GetComponent<HorizontalEscapeMovementComponent>();
            HorizontalDraggableComponent storyCardHorizontalDraggableComponent =
                storyCard.GetComponent<HorizontalDraggableComponent>();
            if (interactiveStoryCardComponent && storyCardComponent 
                && storyCardHorizontalEscapeComponent && storyCardHorizontalDraggableComponent)
            {
                interactiveStoryCardComponent.SetOnSwipeLeftAction(() =>
                {
                    MainGameSceneManager.Instance.ProvideTurnManager().SwipeLeft();
                    storyCardHorizontalEscapeComponent.StartLeftEscapeMovement(
                        storyCardHorizontalDraggableComponent.GetCurrentSpeed()
                    );
                });

                interactiveStoryCardComponent.SetOnSwipeLeftEscapeZoneActions(
                    () => { storyCardComponent.ShowText(isLeftText: true); },
                    () => { storyCardComponent.HideText(); }
                );
            }
        }

        void SetUpOnSwipeRightActions(GameObject storyCard)
        {
            InteractiveStoryCardComponent interactiveStoryCardComponent =
                storyCard.GetComponent<InteractiveStoryCardComponent>();
            StoryCard storyCardComponent = storyCard.GetComponent<StoryCard>();
            HorizontalEscapeMovementComponent storyCardHorizontalEscapeComponent =
                storyCard.GetComponent<HorizontalEscapeMovementComponent>();
            HorizontalDraggableComponent storyCardHorizontalDraggableComponent =
                storyCard.GetComponent<HorizontalDraggableComponent>();
            if (interactiveStoryCardComponent && storyCardComponent
                && storyCardHorizontalEscapeComponent && storyCardHorizontalDraggableComponent)
            {
                interactiveStoryCardComponent.SetOnSwipeRightAction(() =>
                {
                    MainGameSceneManager.Instance.ProvideTurnManager().SwipeRight();
                    storyCardHorizontalEscapeComponent.StartRightEscapeMovement(
                        storyCardHorizontalDraggableComponent.GetCurrentSpeed()
                    );
                });

                interactiveStoryCardComponent.SetOnSwipeRightEscapeZoneActions(
                    () => { storyCardComponent.ShowText(isLeftText: false); },
                    () => { storyCardComponent.HideText(); }
                );
            }
        }

        GameObject newCard = Instantiate(CardPrefab, CardSpawnerOrigin.position, Quaternion.identity);
        if(nextCard != null)
        {
            StoryCard storyCardComponent = newCard.GetComponent<StoryCard>();
            if (storyCardComponent)
            {
                storyCardComponent.SetDataCard(nextCard);
            }

            SetUpOnSwipeLeftActions(newCard);
            SetUpOnSwipeRightActions(newCard);
        }
        
        //indexNextCard++;
        return newCard;
    }

    public GameObject SpawnCombatCard()
    {
        GameObject newCard = Instantiate(CombatCardPrefab, new Vector3 (-6,0,0), Quaternion.identity);

        if (CartaTESTING != null)
        {
            newCard.GetComponent<CombatCard>().SetDataCard(CartaTESTING);
        }

        //indexNextCard++;
        return newCard;
    }

    public bool IsDeckEmpty()
    {
        return indexNextCard == DataCardsList.Count;
    }
}
