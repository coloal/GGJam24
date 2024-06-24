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

    public async Task PlayPlayerDrawCardFromDeck(CombatCard playerCard, Transform cardInHandPosition)
    {
        MMF_DestinationTransform moveCardFromDeckToHandFeedback =
            PlayerDrawCardFromDeckFeedbackPlayer.GetFeedbackOfType<MMF_DestinationTransform>();
        MMF_Scale horizontalFlipFeedback =
            PlayerDrawCardFromDeckFeedbackPlayer.GetFeedbackOfType<MMF_Scale>();
        MMF_ImageAlpha cardFrontRevealFeedback =
            PlayerDrawCardFromDeckFeedbackPlayer.GetFeedbackOfType<MMF_ImageAlpha>();
        if (moveCardFromDeckToHandFeedback != null && horizontalFlipFeedback != null && cardFrontRevealFeedback != null)
        {
            moveCardFromDeckToHandFeedback.TargetTransform = playerCard.gameObject.transform;
            moveCardFromDeckToHandFeedback.Destination = cardInHandPosition;
            horizontalFlipFeedback.AnimateScaleTarget = playerCard.gameObject.transform;
            cardFrontRevealFeedback.BoundImage = playerCard.GetCardFrontImage();

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
}
