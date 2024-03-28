using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [Header("Card prefabs")]
    [SerializeField] private GameObject storyCardPrefab;

    [Header("Card locations data")]
    [SerializeField] private Transform cardsSpawnerOrigin;
    [SerializeField] private Transform cardsFinalPosition;

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

        GameObject newCard = Instantiate(storyCardPrefab, cardsSpawnerOrigin.position, Quaternion.identity);
        if(nextCard != null)
        {
            StoryCard storyCardComponent = newCard.GetComponent<StoryCard>();
            if (storyCardComponent)
            {
                storyCardComponent.SetDataCard(nextCard);
            }

            MoveCardAnimationComponent moveCardAnimationComponent = GetComponent<MoveCardAnimationComponent>();
            if (moveCardAnimationComponent)
            {
                moveCardAnimationComponent.StartMovingCardTowards(
                    cardToMove: newCard,
                    cardFinalPosition: cardsFinalPosition,
                    () => {
                        SetUpOnSwipeLeftActions(newCard);
                        SetUpOnSwipeRightActions(newCard);
                    }
                );
            }
            else
            {
                SetUpOnSwipeLeftActions(newCard);
                SetUpOnSwipeRightActions(newCard);
            }
        }
        
        //indexNextCard++;
        return newCard;
    }
}
