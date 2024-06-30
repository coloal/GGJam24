using CodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [Header("Card prefabs")]
    [SerializeField] private GameObject storyCardPrefab;

    [Header("Card locations data")]
    [SerializeField] private Transform cardsContainerTransform;
    [SerializeField] private Transform cardsSpawnerOrigin;
    [SerializeField] private Transform cardsFinalPosition;

    public GameObject SpawnNextCard(StoryCardTemplate nextCard,
        Action onSwipeLeft, Action onSwipeRight)
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
                    onSwipeLeft();
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
                    onSwipeRight();
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

        // Spawns the next new card to show
        GameObject newCard = Instantiate(storyCardPrefab);
        newCard.transform.SetParent(cardsContainerTransform, worldPositionStays: false);
        newCard.transform.position = cardsSpawnerOrigin.position;

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
                        newCard.GetComponent<HorizontalDraggableComponent>()?.SetInitialPosition();
                    }
                );
            }
            else
            {
                SetUpOnSwipeLeftActions(newCard);
                SetUpOnSwipeRightActions(newCard);
            }

            if (!nextCard.NameOfCard.Equals("Mobile Phone"))
            {
                GameManager.Instance.ProvideBrainSoundManager().PlayCardSound(CardSounds.Center);
            }
            else
            {
                GameManager.Instance.ProvideBrainSoundManager().PlayCardSound(CardSounds.Phone);
            }

        }
        
        //indexNextCard++;
        return newCard;
    }
}
