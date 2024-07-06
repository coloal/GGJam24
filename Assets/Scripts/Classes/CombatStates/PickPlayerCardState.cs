using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PickPlayerCardState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {   
        CombatUtils.ForEachCardInCardsContainer(combatContext.GetPlayerCardInHandContainers(), (cardInHand) =>
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            if (interactiveCombatCardComponent != null)
            {
                interactiveCombatCardComponent.DisableInteractiveComponent();
            }
        });

        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new ShowCardsState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForShowCards
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ForEachCardInCardsContainer(combatContext.GetPlayerCardInHandContainers(), (cardInHand) =>
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            CombatCard playerCombatCard = cardInHand.GetComponent<CombatCard>();

            if (interactiveCombatCardComponent != null && playerCombatCard != null)
            {
                interactiveCombatCardComponent.SetOnClickAction(async () => {
                    await PickAPlayerCard(combatContext, playerCombatCard);
                });
                interactiveCombatCardComponent.EnableInteractiveComponent();
            }
        });
    }

    async Task PickAPlayerCard(CombatManager.CombatContext combatContext, CombatCard cardInHand)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        playerDeckManager.PutCardFromHandToCombatZone(cardInHand);
        combatContext.playerOnCombatCard = cardInHand.gameObject;

        // Sets the selected card to live as a combatContext.combatContainer child transform
        cardInHand.gameObject.transform.SetParent(
            combatContext.combatContainer.transform.parent,
            worldPositionStays: true
        );
        // Disables all interaction with the selected card
        InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
        if (interactiveCombatCardComponent != null)
        {
            interactiveCombatCardComponent.DisableInteractiveComponent();
        }

        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayPlacePlayerCardOnCombat(
                cardToPlaceOnCombat: cardInHand,
                onCombatTransform: combatContext.playerOnCombatCardTransform
            );
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }

        PostProcess(combatContext);
    }
}
