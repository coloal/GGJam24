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
    [SerializeField] public MMF_Player ShowEnemyCardsToChooseFromPlayer;
    [SerializeField] public MMF_Player HideEnemyCardsToChooseFromPlayer;

    [Header("Cards scale configurations")]
    [SerializeField] public float CardOnCombatScaleFactor = 1.5f;
    [SerializeField] public float CardOnTieZoneScaleFactor = 0.75f;

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

        if (moveCardFromDeckToHandFeedback != null && horizontalFlipFeedback != null)
        {
            playerCard.FlipCardUpsideDown();

            moveCardFromDeckToHandFeedback.TargetTransform = playerCard.gameObject.transform;
            moveCardFromDeckToHandFeedback.Destination = cardInHandPosition;
            
            horizontalFlipFeedback.AnimateScaleTarget = playerCard.transform;
            horizontalFlipFeedback.RemapCurveZero = playerCard.transform.localScale.x;
            // By negating its scale, the card is flipped
            horizontalFlipFeedback.RemapCurveOne = - playerCard.transform.localScale.x;

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

    public async Task PlayPlaceEnemyCardOnCombat(CombatCard cardToPlaceOnCombat, Transform onCombatTransform)
    {
        cardToPlaceOnCombat.FlipCardUpsideDown();

        cardToPlaceOnCombat.transform.localScale = new Vector2(
            cardToPlaceOnCombat.transform.localScale.x * CardOnCombatScaleFactor,
            cardToPlaceOnCombat.transform.localScale.y * CardOnCombatScaleFactor
        );

        await PlayPlaceCardOnCombat(cardToPlaceOnCombat, onCombatTransform);
    }

    async Task PlayPlaceCardOnCombat(CombatCard cardToPlaceOnCombat, Transform onCombatTransform)
    {
        MMF_DestinationTransform moveCardFeedback =
            PlaceCardOnCombatPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Card"));
        MMF_Scale getCardCloseToCameraOnXFeedback =
            PlaceCardOnCombatPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Get Card close to Camera On X"));
        MMF_Scale getCardCloseToCameraOnYFeedback =
            PlaceCardOnCombatPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Get Card close to Camera On Y"));
        
        if (moveCardFeedback != null && getCardCloseToCameraOnXFeedback != null
            && getCardCloseToCameraOnYFeedback != null)
        {
            moveCardFeedback.TargetTransform = cardToPlaceOnCombat.gameObject.transform;
            moveCardFeedback.Destination = onCombatTransform;

            getCardCloseToCameraOnXFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            getCardCloseToCameraOnXFeedback.RemapCurveZero = cardToPlaceOnCombat.transform.localScale.x;
            getCardCloseToCameraOnXFeedback.RemapCurveOne = cardToPlaceOnCombat.transform.localScale.x * PlaceCardOnCombatScaleFactor;

            getCardCloseToCameraOnYFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            getCardCloseToCameraOnYFeedback.RemapCurveZero = cardToPlaceOnCombat.transform.localScale.y;
            getCardCloseToCameraOnYFeedback.RemapCurveOne = cardToPlaceOnCombat.transform.localScale.y * PlaceCardOnCombatScaleFactor;

            await PlaceCardOnCombatPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayPlacePlayerCardOnCombat(CombatCard cardToPlaceOnCombat, Transform onCombatTransform)
    {
        MMF_Scale scaleCardOnYFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On Y"));
        MMF_Scale horizontalFlipFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Horizontal Flip"));

        if (scaleCardOnYFeedback != null && horizontalFlipFeedback != null)
        {
            scaleCardOnYFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            scaleCardOnYFeedback.RemapCurveZero = cardToPlaceOnCombat.transform.localScale.y;
            scaleCardOnYFeedback.RemapCurveOne = cardToPlaceOnCombat.transform.localScale.y * CardOnCombatScaleFactor;

            horizontalFlipFeedback.AnimateScaleTarget = cardToPlaceOnCombat.transform;
            horizontalFlipFeedback.RemapCurveZero = cardToPlaceOnCombat.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = - cardToPlaceOnCombat.transform.localScale.x * CardOnCombatScaleFactor;

            await HideCardFromPlayerHandPlayer.PlayFeedbacksTask();
            await PlayPlaceCardOnCombat(cardToPlaceOnCombat, onCombatTransform);
        }
    }

    public async Task PlayRevealCard(CombatCard cardToReveal)
    {
        MMF_Scale scaleCardOnXFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On X"));
        MMF_Scale scaleCardOnYFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On Y"));
        MMF_Scale horizontalFlipFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Horizontal Flip"));
        MMF_Scale scaleBackCardOnXFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Back Card On X"));
        MMF_Scale scaleBackCardOnYFeedback =
            RevealCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Back Card On Y"));

        if (scaleCardOnXFeedback != null && scaleCardOnYFeedback != null 
            && horizontalFlipFeedback != null && scaleBackCardOnXFeedback != null
            && scaleBackCardOnYFeedback != null)
        {
            cardToReveal.transform.SetAsLastSibling();

            scaleCardOnXFeedback.AnimateScaleTarget = cardToReveal.transform;
            scaleCardOnXFeedback.RemapCurveZero = cardToReveal.transform.localScale.x;
            scaleCardOnXFeedback.RemapCurveOne = cardToReveal.transform.localScale.x * RevealCardScaleFactor;

            scaleCardOnYFeedback.AnimateScaleTarget = cardToReveal.transform;
            scaleCardOnYFeedback.RemapCurveZero = cardToReveal.transform.localScale.y;
            scaleCardOnYFeedback.RemapCurveOne = cardToReveal.transform.localScale.y * RevealCardScaleFactor;

            horizontalFlipFeedback.AnimateScaleTarget = cardToReveal.transform;
            horizontalFlipFeedback.RemapCurveZero = cardToReveal.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = - cardToReveal.transform.localScale.x * RevealCardScaleFactor;

            scaleBackCardOnXFeedback.AnimateScaleTarget = cardToReveal.transform;
            scaleBackCardOnXFeedback.RemapCurveZero = cardToReveal.transform.localScale.x * RevealCardScaleFactor;
            scaleBackCardOnXFeedback.RemapCurveOne = cardToReveal.transform.localScale.x;

            scaleBackCardOnYFeedback.AnimateScaleTarget = cardToReveal.transform;
            scaleBackCardOnYFeedback.RemapCurveZero = cardToReveal.transform.localScale.y * RevealCardScaleFactor;
            scaleBackCardOnYFeedback.RemapCurveOne = cardToReveal.transform.localScale.y;

            await RevealCardPlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayAttackCard(CombatCard attackerCard)
    {
        MMF_DestinationTransform moveCardToCenterFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Card to Center"));
        MMF_Scale scaleCardOnXFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On X"));
        MMF_Scale scaleCardOnYFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On Y"));
        MMF_Rotation rotateCardFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Rotation>().Find((feedback) => feedback.Label.Equals("Rotate Card"));
        MMF_Scale attackHitOnXFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Attack Hit On X"));
        MMF_Scale attackHitOnYFeedback =
            AttackCardPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Attack Hit On Y"));

        if (scaleCardOnXFeedback != null && scaleCardOnYFeedback != null 
            && rotateCardFeedback != null && moveCardToCenterFeedback != null 
            && attackHitOnXFeedback != null && attackHitOnYFeedback != null)
        {
            attackerCard.transform.SetAsLastSibling();

            moveCardToCenterFeedback.TargetTransform = attackerCard.transform;

            scaleCardOnXFeedback.AnimateScaleTarget = attackerCard.transform;
            scaleCardOnXFeedback.RemapCurveZero = attackerCard.transform.localScale.x;
            scaleCardOnXFeedback.RemapCurveOne = attackerCard.transform.localScale.x * AttackACardScaleFactor;

            scaleCardOnYFeedback.AnimateScaleTarget = attackerCard.transform;
            scaleCardOnYFeedback.RemapCurveZero = attackerCard.transform.localScale.y;
            scaleCardOnYFeedback.RemapCurveOne = attackerCard.transform.localScale.y * AttackACardScaleFactor;

            rotateCardFeedback.AnimateRotationTarget = attackerCard.transform;

            attackHitOnXFeedback.AnimateScaleTarget = attackerCard.transform;
            attackHitOnXFeedback.RemapCurveZero = attackerCard.transform.localScale.x * AttackACardScaleFactor;
            attackHitOnXFeedback.RemapCurveOne = attackerCard.transform.localScale.x;

            attackHitOnYFeedback.AnimateScaleTarget = attackerCard.transform;
            attackHitOnYFeedback.RemapCurveZero = attackerCard.transform.localScale.y * AttackACardScaleFactor;
            attackHitOnYFeedback.RemapCurveOne = attackerCard.transform.localScale.y;

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
        MMF_Scale scaleCardOnYFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On Y"));
        MMF_Scale horizontalFlipFeedback =
            HideCardFromPlayerHandPlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Horizontal Flip"));

        MMF_DestinationTransform moveRotateAndScaleCardFeedback =
            MoveCardToTransformPlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move and Rotate Card"));
        
        if (scaleCardOnYFeedback != null && horizontalFlipFeedback != null
            && moveRotateAndScaleCardFeedback != null)
        {
            scaleCardOnYFeedback.AnimateScaleTarget = cardToReturn.transform;
            scaleCardOnYFeedback.RemapCurveZero = cardToReturn.transform.localScale.y;
            scaleCardOnYFeedback.RemapCurveOne = deckTransform.localScale.y;

            horizontalFlipFeedback.AnimateScaleTarget = cardToReturn.transform;
            horizontalFlipFeedback.RemapCurveZero = cardToReturn.transform.localScale.x;
            horizontalFlipFeedback.RemapCurveOne = - deckTransform.localScale.x;

            moveRotateAndScaleCardFeedback.TargetTransform = cardToReturn.transform;
            moveRotateAndScaleCardFeedback.Destination = deckTransform;

            await HideCardFromPlayerHandPlayer.PlayFeedbacksTask();
            deckTransform.SetAsLastSibling();
            await MoveCardToTransformPlayer.PlayFeedbacksTask();

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
        MMF_Scale scaleCardOnXFeedback =
            MoveCardToTieZonePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On X"));
        MMF_Scale scaleCardOnYFeedback =
            MoveCardToTieZonePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Card On Y"));

        if (moveCardFeedback != null && rotateCardFeedback != null
            && scaleCardOnXFeedback != null && scaleCardOnYFeedback != null)
        {
            cardToMove.transform.SetAsLastSibling();

            moveCardFeedback.TargetTransform = cardToMove.transform;
            moveCardFeedback.Destination = newCardPosition;

            rotateCardFeedback.AnimateRotationTarget = cardToMove.transform;

            scaleCardOnXFeedback.AnimateScaleTarget = cardToMove.transform;
            scaleCardOnXFeedback.RemapCurveZero = cardToMove.transform.localScale.x;
            scaleCardOnXFeedback.RemapCurveOne = cardToMove.transform.localScale.x * CardOnTieZoneScaleFactor;

            scaleCardOnYFeedback.AnimateScaleTarget = cardToMove.transform;
            scaleCardOnYFeedback.RemapCurveZero = cardToMove.transform.localScale.y;
            scaleCardOnYFeedback.RemapCurveOne = cardToMove.transform.localScale.y * CardOnTieZoneScaleFactor;

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
        MMF_Scale scaleEnemyCardOnXFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Enemy Card On X"));
        MMF_Scale scaleEnemyCardOnYFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Enemy Card On Y"));
        MMF_DestinationTransform movePlayerCardFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_DestinationTransform>().Find((feedback) => feedback.Label.Equals("Move Player Card"));
        MMF_Scale scalePlayerCardOnXFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Player Card On X"));
        MMF_Scale scalePlayerCardOnYFeedback =
            AttackCardsOnTiePlayer.GetFeedbacksOfType<MMF_Scale>().Find((feedback) => feedback.Label.Equals("Scale Player Card On Y"));
        
        if (moveEnemyCardFeedback != null && scaleEnemyCardOnXFeedback != null
            && scaleEnemyCardOnYFeedback != null && movePlayerCardFeedback != null 
            && scalePlayerCardOnXFeedback != null && scalePlayerCardOnYFeedback != null)
        {
            movePlayerCardFeedback.TargetTransform = playerCard.transform;

            scalePlayerCardOnXFeedback.AnimateScaleTarget = playerCard.transform;
            scalePlayerCardOnXFeedback.RemapCurveZero = playerCard.transform.localScale.x;
            scalePlayerCardOnXFeedback.RemapCurveOne = playerCard.transform.localScale.x + (playerCard.transform.localScale.x * AttackCardsOnTieScaleFactor);

            scalePlayerCardOnYFeedback.AnimateScaleTarget = playerCard.transform;
            scalePlayerCardOnYFeedback.RemapCurveZero = playerCard.transform.localScale.y;
            scalePlayerCardOnYFeedback.RemapCurveOne = playerCard.transform.localScale.y + (playerCard.transform.localScale.y * AttackCardsOnTieScaleFactor);

            moveEnemyCardFeedback.TargetTransform = enemyCard.transform;

            scaleEnemyCardOnXFeedback.AnimateScaleTarget = enemyCard.transform;
            scaleEnemyCardOnXFeedback.RemapCurveZero = enemyCard.transform.localScale.x;
            scaleEnemyCardOnXFeedback.RemapCurveOne = enemyCard.transform.localScale.x + (enemyCard.transform.localScale.x * AttackCardsOnTieScaleFactor);

            scaleEnemyCardOnYFeedback.AnimateScaleTarget = enemyCard.transform;
            scaleEnemyCardOnYFeedback.RemapCurveZero = enemyCard.transform.localScale.y;
            scaleEnemyCardOnYFeedback.RemapCurveOne = enemyCard.transform.localScale.y + (enemyCard.transform.localScale.y * AttackCardsOnTieScaleFactor);

            await AttackCardsOnTiePlayer.PlayFeedbacksTask();
        }
    }

    public async Task PlayShowEnemyCardsToChooseFrom()
    {
        await ShowEnemyCardsToChooseFromPlayer.PlayFeedbacksTask();
    }

    public async Task PlayHideEnemyCardsToChooseFrom()
    {
        await HideEnemyCardsToChooseFromPlayer.PlayFeedbacksTask();
    }
}
