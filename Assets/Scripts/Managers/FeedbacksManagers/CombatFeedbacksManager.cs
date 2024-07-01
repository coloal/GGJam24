using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
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
    [SerializeField] public MMF_Player KillACardPlayer;
    [SerializeField] public MMF_Player MoveCardToTransformPlayer;
    [SerializeField] public MMF_Player MoveCardToTieZonePlayer;
    [SerializeField] public MMF_Player KillACardInTieZonePlayer;
    [SerializeField] public MMF_Player AttackCardsOnTiePlayer;

    [Header("Cards scale configurations")]
    [SerializeField] public float CardOnCombatScale = 3.5f;
    [SerializeField] public float CardOnTieZoneScale = 2.2f;

    [Header("Feedbacks configuration")]
    [Header("Place a Card on Combat")]
    [SerializeField] public float PlaceCardOnCombatScaleFactor = 2.0f;
    [Header("Reveal a Card")]
    [SerializeField] public float RevealCardScaleFactor = 2.0f;
    [Header("Attack a Card")]
    [SerializeField] public float AttackACardScaleFactor = 2.0f;
    [SerializeField] public AnimCombatExplosionsManagerComponent animCombatExplosionsManager;
    [Header("Attack Cards On Tie")]
    [SerializeField] public float AttackCardsOnTieScaleFactor = 0.25f;
    

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
            PlaceCardOnCombatPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Card"));
        MMF_Scale getCardCloseToCameraFeedback =
            PlaceCardOnCombatPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Get Card close to Camera"));
        
        if (moveCardFeedback != null && getCardCloseToCameraFeedback != null)
        {
            moveCardFeedback.TargetTransform = cardToPlaceOnCombat.gameObject.transform;
            moveCardFeedback.Destination = onCombatTransform;

            getCardCloseToCameraFeedback.AnimateScaleTarget = cardToPlaceOnCombat.gameObject.transform;
            getCardCloseToCameraFeedback.RemapCurveZero = CardOnCombatScale;
            getCardCloseToCameraFeedback.RemapCurveOne = CardOnCombatScale * PlaceCardOnCombatScaleFactor;

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
            scaleCardFeedback.RemapCurveOne = CardOnCombatScale;

            horizontalFlipFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            horizontalFlipFeedback.RemapCurveZero = - cardToPlaceOnCombat.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = CardOnCombatScale;

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
            cardToReveal.transform.SetAsLastSibling();

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
            attackerCard.transform.SetAsLastSibling();

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

    public async Task PlayKillACard(CombatCard cardToKill, CombatCard attackerCard, bool hasToFlipExplosions)
    {
        MMF_RotationShake rotationShakeFeedback =
            KillACardPlayer.GetFeedbacksOfType<MMF_RotationShake>().Find((feedback) => feedback.Label.Equals("Rotation Shake"));
        MMF_ImageAlpha cardDisappearFeedback =
            KillACardPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Disappear"));

        MMRotationShaker cardRotationShakerComponent = cardToKill.GetComponent<MMRotationShaker>();

        if (rotationShakeFeedback != null && cardDisappearFeedback != null 
            && cardRotationShakerComponent != null)
        {
            animCombatExplosionsManager.SetAnimExplosionToPlay(
                attackerCard: attackerCard,
                animPositionTransform: cardToKill.transform,
                hasToFlipExplosions
            );

            rotationShakeFeedback.TargetShaker = cardRotationShakerComponent;

            cardDisappearFeedback.BoundImage = cardToKill.GetCardFrontImage();

            await KillACardPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayReturnCardToDeck(CombatCard cardToReturn, Transform deckTransform)
    {
        MMF_Scale scaleCardFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card"));
        MMF_Scale horizontalFlipFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Horizontal Flip"));
        MMF_ImageAlpha cardFrontHideFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Front Hide"));
        MMF_ImageAlpha cardBackRevealFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Back Reveal"));

        MMF_DestinationTransform moveRotateAndScaleCardFeedback =
            MoveCardToTransformPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move and Rotate Card"));
        
        if (scaleCardFeedback != null && horizontalFlipFeedback != null
            && cardFrontHideFeedback != null && cardBackRevealFeedback != null
            && moveRotateAndScaleCardFeedback != null)
        {
            scaleCardFeedback.AnimateScaleTarget = cardToReturn.transform;
            scaleCardFeedback.RemapCurveZero = cardToReturn.transform.localScale.x;
            scaleCardFeedback.RemapCurveOne = deckTransform.localScale.x;

            horizontalFlipFeedback.AnimateScaleTarget = cardToReturn.transform;
            horizontalFlipFeedback.RemapCurveZero = cardToReturn.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = - deckTransform.localScale.x;

            cardFrontHideFeedback.BoundImage = cardToReturn.GetCardFrontImage();
            cardBackRevealFeedback.BoundImage = cardToReturn.GetCardBackImage();

            moveRotateAndScaleCardFeedback.TargetTransform = cardToReturn.transform;
            moveRotateAndScaleCardFeedback.Destination = deckTransform;

            await HideCardFromPlayerHandPlayer.PlayFeedbacksTask();
            deckTransform.SetAsLastSibling();
            await MoveCardToTransformPlayer.PlayFeedbacksTask();

            // After completing the animation, reset the card to the deck's original scale to prevent image flipped bugs
            cardToReturn.transform.localScale = new Vector2(
                deckTransform.localScale.x,
                cardToReturn.transform.localScale.y
            );

            DeckBehaviourComponent deckBehaviourComponent = deckTransform.gameObject.GetComponent<DeckBehaviourComponent>();
            if (deckBehaviourComponent != null)
            {
                deckBehaviourComponent.AddCardToDeck();
                await DeckFeedbackPlayer.PlayFeedbacksTask();
            }
        }
    }

    public async Task PlayMoveCardPositionInHand(CombatCard cardToMove, Transform newCardPosition)
    {
        MMF_DestinationTransform moveRotateAndScaleCardFeedback =
            MoveCardToTransformPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move and Rotate Card"));

        if (moveRotateAndScaleCardFeedback != null)
        {
            moveRotateAndScaleCardFeedback.TargetTransform = cardToMove.transform;
            moveRotateAndScaleCardFeedback.Destination = newCardPosition;

            await MoveCardToTransformPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayMoveCardToTieZone(CombatCard cardToMove, Transform newCardPosition)
    {
        MMF_DestinationTransform moveCardFeedback =
            MoveCardToTieZonePlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Card"));
        MMF_Rotation rotateCardFeedback =
            MoveCardToTieZonePlayer.GetFeedbacksOfType<MMF_Rotation>().Find((feedback) => feedback.Label.Equals("Rotate Card"));
        MMF_Scale scaleCardFeedback =
            MoveCardToTieZonePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card"));

        if (moveCardFeedback != null && rotateCardFeedback != null)
        {
            cardToMove.transform.SetAsLastSibling();

            moveCardFeedback.TargetTransform = cardToMove.transform;
            moveCardFeedback.Destination = newCardPosition;

            rotateCardFeedback.AnimateRotationTarget = cardToMove.transform;

            scaleCardFeedback.AnimateScaleTarget = cardToMove.transform;
            scaleCardFeedback.RemapCurveZero = cardToMove.transform.localScale.x;
            scaleCardFeedback.RemapCurveOne = CardOnTieZoneScale;

            await MoveCardToTieZonePlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayKillACardInTieZone(CombatCard cardToKill)
    {
        MMF_RotationShake rotationShakeFeedback =
            KillACardInTieZonePlayer.GetFeedbacksOfType<MMF_RotationShake>().Find((feedback) => feedback.Label.Equals("Rotation Shake"));
        MMF_Position elevateCardFeedback =
            KillACardInTieZonePlayer.GetFeedbacksOfType<MMF_Position>().Find((feedback) => feedback.Label.Equals("Elevate Card"));
        MMF_ImageAlpha cardDisappearFeedback =
            KillACardInTieZonePlayer.GetFeedbacksOfType<MMF_ImageAlpha>().Find((feedback) => feedback.Label.Equals("Card Disappear"));

        MMRotationShaker cardRotationShakerComponent = cardToKill.GetComponent<MMRotationShaker>();

        if (rotationShakeFeedback != null && elevateCardFeedback != null
            && cardDisappearFeedback != null)
        {
            rotationShakeFeedback.TargetShaker = cardRotationShakerComponent;

            elevateCardFeedback.AnimatePositionTarget = cardToKill.gameObject;

            cardDisappearFeedback.BoundImage = cardToKill.GetCardFrontImage();

            await KillACardInTieZonePlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayAttackCardsOnTie(CombatCard playerCard, CombatCard enemyCard)
    {
        MMF_DestinationTransform moveEnemyCardFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Enemy Card"));
        MMF_Scale scaleEnemyCardFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Enemy Card"));
        MMF_DestinationTransform movePlayerCardFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Player Card"));
        MMF_Scale scalePlayerCardFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Player Card"));
        
        if (moveEnemyCardFeedback != null && scaleEnemyCardFeedback != null
            && movePlayerCardFeedback != null && scalePlayerCardFeedback != null)
        {
            movePlayerCardFeedback.TargetTransform = playerCard.transform;

            scalePlayerCardFeedback.AnimateScaleTarget = playerCard.transform;
            scalePlayerCardFeedback.RemapCurveZero = playerCard.transform.localScale.x;
            scalePlayerCardFeedback.RemapCurveOne = playerCard.transform.localScale.x + (playerCard.transform.localScale.x * AttackCardsOnTieScaleFactor);

            moveEnemyCardFeedback.TargetTransform = enemyCard.transform;

            scaleEnemyCardFeedback.AnimateScaleTarget = enemyCard.transform;
            scaleEnemyCardFeedback.RemapCurveZero = enemyCard.transform.localScale.x;
            scaleEnemyCardFeedback.RemapCurveOne = enemyCard.transform.localScale.x + (enemyCard.transform.localScale.x * AttackCardsOnTieScaleFactor);

            await AttackCardsOnTiePlayer.PlayFeedbacksTask();
        }
    }
}
