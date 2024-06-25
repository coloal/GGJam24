using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

public class CombatFeedbacksManager : MonoBehaviour
{
    [Header("Scene feedbacks")]
    [SerializeField] public MMF_Player ShowEnemyCardsTypesHintsFeedbackPlayer;
    [SerializeField] public MMF_Player PlayerDrawCardFromDeckFeedbackPlayer;
    [SerializeField] public MMF_Player DeckFeedbackPlayer;
    [SerializeField] public MMF_Player PlaceCardOnCombatPlayer;

    public async Task PlayPlayerDrawCardFromDeck(CombatCard playerCard, DeckBehaviourComponent playerDeck, Transform cardInHandPosition)
    {
        MMF_DestinationTransform moveCardFromDeckToHandFeedback =
            PlayerDrawCardFromDeckFeedbackPlayer.GetFeedbackOfType<MMF_DestinationTransform>();
        MMF_Scale horizontalFlipFeedback =
            PlayerDrawCardFromDeckFeedbackPlayer.GetFeedbackOfType<MMF_Scale>();
        MMF_ImageAlpha cardFrontRevealFeedback =
            PlayerDrawCardFromDeckFeedbackPlayer.GetFeedbackOfType<MMF_ImageAlpha>();
        if (moveCardFromDeckToHandFeedback != null && horizontalFlipFeedback != null && cardFrontRevealFeedback != null)
        {
            RectTransform playerCardRectTransform = playerCard.GetComponent<RectTransform>();

            moveCardFromDeckToHandFeedback.TargetTransform = playerCard.gameObject.transform;
            moveCardFromDeckToHandFeedback.Destination = cardInHandPosition;
            horizontalFlipFeedback.AnimateScaleTarget = playerCard.gameObject.transform;
            if (playerCardRectTransform != null)
            {
                horizontalFlipFeedback.RemapCurveZero = -playerCardRectTransform.localScale.x;
                horizontalFlipFeedback.RemapCurveOne = playerCardRectTransform.localScale.x;
            }
            cardFrontRevealFeedback.BoundImage = playerCard.GetCardFrontImage();

            // Shake the deck
            playerDeck.DrawCardFromDeck();
            DeckFeedbackPlayer.PlayFeedbacks();
            // Draw a card
            await PlayerDrawCardFromDeckFeedbackPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayShowEnemyCardsTypesHints(float pauseTime)
    {
        MMF_HoldingPause showPauseFeedback =
            ShowEnemyCardsTypesHintsFeedbackPlayer.GetFeedbackOfType<MMF_HoldingPause>();
        if (showPauseFeedback != null)
        {
            showPauseFeedback.PauseDuration = pauseTime;

            await ShowEnemyCardsTypesHintsFeedbackPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayPlaceCardOnCombat(CombatCard cardToPlaceOnCombat, Transform onCombatTransform)
    {
        MMF_DestinationTransform moveCardFeedback =
            PlaceCardOnCombatPlayer.GetFeedbackOfType<MMF_DestinationTransform>();
        MMF_Scale scaleCardFeedback =
            PlaceCardOnCombatPlayer.GetFeedbackOfType<MMF_Scale>();
        
        if (moveCardFeedback != null && scaleCardFeedback != null)
        {
            moveCardFeedback.TargetTransform = cardToPlaceOnCombat.gameObject.transform;
            moveCardFeedback.Destination = onCombatTransform;

            scaleCardFeedback.AnimateScaleTarget = cardToPlaceOnCombat.gameObject.transform;
            scaleCardFeedback.RemapCurveZero = onCombatTransform.transform.localScale.x;
            scaleCardFeedback.RemapCurveOne = onCombatTransform.transform.localScale.x * 2;

            await PlaceCardOnCombatPlayer.PlayFeedbacksTask();
        }
    }
}
