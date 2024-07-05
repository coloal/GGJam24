using System;
using Unity.VisualScripting;
using UnityEngine;

public class CoinCard : MonoBehaviour
{
    const int DESTROY_COIN_CARD_DELAY_SECONDS = 1;
    const string COIN_CARD_TEXT_SEPARATOR = "|";

    [SerializeField] private StoryCardTemplate coinCardInformation;

    public void SetUpCard(Action onSwipeLeft, Action onSwipeRight)
    {
        SetUpStoryCard();
        SetUpOnSwipeLeftActions(onSwipeLeft);
        SetUpOnSwipeRightActions(onSwipeRight);
    }

    void SetUpStoryCard()
    {
        StoryCard storyCardComponent = GetComponent<StoryCard>();
        if (storyCardComponent != null)
        {
            storyCardComponent.SetDataCard(coinCardInformation);
            string[] coinCardDescriptions = coinCardInformation.Text.Split(COIN_CARD_TEXT_SEPARATOR);
            storyCardComponent.SetCardDescription(
                coinCardDescriptions[UnityEngine.Random.Range(0, coinCardDescriptions.Length)]
            );
            storyCardComponent.HideText();
        }
    }

    void SetUpOnSwipeLeftActions(Action onSwipeLeft)
    {
        InteractiveStoryCardComponent interactiveStoryCardComponent = GetComponent<InteractiveStoryCardComponent>();
        StoryCard storyCardComponent = GetComponent<StoryCard>();
        HorizontalEscapeMovementComponent horizontalEscapeComponent = GetComponent<HorizontalEscapeMovementComponent>();
        HorizontalDraggableComponent horizontalDraggableComponent = GetComponent<HorizontalDraggableComponent>();

        if (interactiveStoryCardComponent != null && storyCardComponent != null
            && horizontalEscapeComponent != null && horizontalDraggableComponent != null)
        {
            interactiveStoryCardComponent.SetOnSwipeLeftAction(() =>
            {
                onSwipeLeft();
                horizontalEscapeComponent.StartLeftEscapeMovement(
                    horizontalDraggableComponent.GetCurrentSpeed()
                );
                DestroyCard();
            });

            interactiveStoryCardComponent.SetOnSwipeLeftEscapeZoneActions(
                () => { storyCardComponent.ShowText(isLeftText: true); },
                () => { storyCardComponent.HideText(); }
            );
        }
    }

    void SetUpOnSwipeRightActions(Action onSwipeRight)
    {
        InteractiveStoryCardComponent interactiveStoryCardComponent = GetComponent<InteractiveStoryCardComponent>();
        StoryCard storyCardComponent = GetComponent<StoryCard>();
        HorizontalEscapeMovementComponent horizontalEscapeComponent = GetComponent<HorizontalEscapeMovementComponent>();
        HorizontalDraggableComponent horizontalDraggableComponent = GetComponent<HorizontalDraggableComponent>();

        if (interactiveStoryCardComponent != null && storyCardComponent != null
            && horizontalEscapeComponent != null && horizontalDraggableComponent != null)
        {
            interactiveStoryCardComponent.SetOnSwipeRightAction(() =>
            {
                onSwipeRight();
                horizontalEscapeComponent.StartRightEscapeMovement(
                    horizontalDraggableComponent.GetCurrentSpeed()
                );
                DestroyCard();
            });

            interactiveStoryCardComponent.SetOnSwipeRightEscapeZoneActions(
                () => { storyCardComponent.ShowText(isLeftText: false); },
                () => { storyCardComponent.HideText(); }
            );
        }
    }

    void DestroyCard()
    {
        GameUtils.CreateTemporizer(() => { Destroy(gameObject); }, DESTROY_COIN_CARD_DELAY_SECONDS, this);      
    }
}
