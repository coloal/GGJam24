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
    [SerializeField] public MMF_Player HideCardFromPlayerHandPlayer;
    [SerializeField] public MMF_Player RevealCardPlayer;
    [SerializeField] public MMF_Player AttackCardPlayer;

    [Header("Feedbacks configuration")]
    [Header("Place Card on Combat")]
    [SerializeField] public float PlaceCardOnCombatScaleFactor = 2.0f;
    [Header("Reveal Card")]
    [SerializeField] public float RevealCardScaleFactor = 2.0f;
    [Header("Attack a Card")]
    [SerializeField] public float AttackACardScaleFactor = 2.0f;
    

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
            scaleCardFeedback.RemapCurveOne = onCombatTransform.transform.localScale.x * PlaceCardOnCombatScaleFactor;

            await PlaceCardOnCombatPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayPlacePlayerCardOnCombat(CombatCard cardToPlaceOnCombat, Transform onCombatTransform)
    {
        MMF_Scale scaleCardFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card"));
        MMF_Scale horizontalFlipFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Horizontal Flip"));
        MMF_ImageAlpha cardFrontHideFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Front Hide"));
        MMF_ImageAlpha cardBackRevealFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Back Reveal"));

        if (scaleCardFeedback != null && horizontalFlipFeedback != null
            && cardFrontHideFeedback != null && cardBackRevealFeedback != null)
        {
            scaleCardFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            scaleCardFeedback.RemapCurveZero = cardToPlaceOnCombat.transform.localScale.x;
            scaleCardFeedback.RemapCurveOne = onCombatTransform.transform.localScale.x;

            horizontalFlipFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            horizontalFlipFeedback.RemapCurveZero = - cardToPlaceOnCombat.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = onCombatTransform.transform.localScale.x;

            cardFrontHideFeedback.BoundImage = cardToPlaceOnCombat.GetCardFrontImage();
            cardBackRevealFeedback.BoundImage = cardToPlaceOnCombat.GetCardBackImage();

            await HideCardFromPlayerHandPlayer.PlayFeedbacksTask();
            await PlayPlaceCardOnCombat(cardToPlaceOnCombat, onCombatTransform);
        }
    }

    public async Task PlayRevealCard(CombatCard cardToReveal)
    {
        MMF_Scale scaleCardFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card"));
        MMF_Scale horizontalFlipFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Horizontal Flip"));
        MMF_ImageAlpha cardFrontRevealFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Front Reveal"));
        MMF_ImageAlpha cardBackHideFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Back Hide"));
        MMF_Scale scaleBackCardFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Back Card"));

        if (scaleCardFeedback != null && horizontalFlipFeedback != null
            && cardFrontRevealFeedback != null && cardBackHideFeedback != null
            && scaleBackCardFeedback != null)
        {
            scaleCardFeedback.AnimateScaleTarget = cardToReveal.transform;
            scaleCardFeedback.RemapCurveZero = cardToReveal.transform.localScale.x;
            scaleCardFeedback.RemapCurveOne = cardToReveal.transform.localScale.x * RevealCardScaleFactor;

            horizontalFlipFeedback.AnimateScaleTarget = cardToReveal.transform;
            horizontalFlipFeedback.RemapCurveZero = - cardToReveal.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = cardToReveal.transform.localScale.x * RevealCardScaleFactor;

            cardFrontRevealFeedback.BoundImage = cardToReveal.GetCardFrontImage();
            cardBackHideFeedback.BoundImage = cardToReveal.GetCardBackImage();

            scaleBackCardFeedback.AnimateScaleTarget = cardToReveal.transform;
            scaleBackCardFeedback.RemapCurveZero = cardToReveal.transform.localScale.x * RevealCardScaleFactor;
            scaleBackCardFeedback.RemapCurveOne = cardToReveal.transform.localScale.x;

            await RevealCardPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayAttackCard(CombatCard attackerCard)
    {
        MMF_DestinationTransform moveCardToCenterFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Card to Center"));
        MMF_Scale scaleCardFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card"));
        MMF_Rotation rotateCardFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Rotation>().Find((feedback) => feedback.Label.Equals("Rotate Card"));
        MMF_Scale attackHitFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Attack Hit"));

        if (scaleCardFeedback != null && rotateCardFeedback != null
            && moveCardToCenterFeedback != null && attackHitFeedback != null)
        {
            moveCardToCenterFeedback.TargetTransform = attackerCard.transform;

            scaleCardFeedback.AnimateScaleTarget = attackerCard.transform;
            scaleCardFeedback.RemapCurveZero = attackerCard.transform.localScale.x;
            scaleCardFeedback.RemapCurveOne = attackerCard.transform.localScale.x * AttackACardScaleFactor;

            rotateCardFeedback.AnimateRotationTarget = attackerCard.transform;

            attackHitFeedback.AnimateScaleTarget = attackerCard.transform;
            attackHitFeedback.RemapCurveZero = attackerCard.transform.localScale.x * AttackACardScaleFactor;
            attackHitFeedback.RemapCurveOne = attackerCard.transform.localScale.x;

            await AttackCardPlayer.PlayFeedbacksTask();
        }
    }
}
