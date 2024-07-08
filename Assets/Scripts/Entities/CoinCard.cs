using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinCard : MonoBehaviour
{
    const int DESTROY_COIN_CARD_DELAY_SECONDS = 1;
    const string COIN_CARD_TEXT_SEPARATOR = "|";

    [Header("Card information")]
    [SerializeField] private StoryCardTemplate coinCardInformation;

    [Header("Card coin images")]
    [SerializeField] private Image coinImage;
    [SerializeField] private Sprite coinHeadsImage;
    [SerializeField] private Sprite coinTailsImage;

    public void SetUpCard(Action onSwipeLeft, Action<CoinCard> onSwipeLeftEscapeZone,
        Action onSwipeRight, Action<CoinCard> onSwipeRightEscapeZone)
    {
        SetUpStoryCard();
        SetUpOnSwipeLeftActions(onSwipeLeft, onSwipeLeftEscapeZone);
        SetUpOnSwipeRightActions(onSwipeRight, onSwipeRightEscapeZone);
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
            coinImage.sprite = coinTailsImage;
        }
    }

    void SetUpOnSwipeLeftActions(Action onSwipeLeft, Action<CoinCard> onSwipeLeftEscapeZone)
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
                () => 
                { 
                    storyCardComponent.ShowText(isLeftText: true);
                    onSwipeLeftEscapeZone(this);
                },
                () => { storyCardComponent.HideText(); }
            );
        }
    }

    void SetUpOnSwipeRightActions(Action onSwipeRight, Action<CoinCard> onSwipeRightEscapeZone)
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
                () => 
                {
                    onSwipeRightEscapeZone(this); 
                    storyCardComponent.ShowText(isLeftText: false); 
                },
                () => { storyCardComponent.HideText(); }
            );
        }
    }

    void DestroyCard()
    {
        GameUtils.CreateTemporizer(() => { Destroy(gameObject); }, DESTROY_COIN_CARD_DELAY_SECONDS, this);      
    }

    public void SetImageAsCoinHeads()
    {
        coinImage.sprite = coinHeadsImage;
    }

    public void SetImageAsCoinTails()
    {
        coinImage.sprite = coinTailsImage;
    }
}
