using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PickPlayerCardState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {   
        foreach (Transform cardInHand in combatContext.playerHandContainer.transform)
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            if (interactiveCombatCardComponent != null)
            {
                interactiveCombatCardComponent.DisableInteractiveComponent();
            }
        }

        CombatSceneManager.Instance.ProvideCombatV2Manager().OverwriteCombatContext(combatContext);
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new ShowCardsState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        foreach (Transform cardInHand in combatContext.playerHandContainer.transform)
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
        }
    }

    async Task PickAPlayerCard(CombatV2Manager.CombatContext combatContext, CombatCard cardInHand)
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
                onCombatTransform: combatContext.playerOnCombatCardFinalPosition
            );

        PostProcess(combatContext);
    }
}
