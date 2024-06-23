using System.Collections;
using System.Collections.Generic;
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
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        foreach (Transform cardInHand in combatContext.playerHandContainer.transform)
        {            
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            CombatCard playerCombatCard = cardInHand.GetComponent<CombatCard>();

            if (interactiveCombatCardComponent != null && playerCombatCard != null)
            {
                interactiveCombatCardComponent.SetOnClickAction(() => {
                    playerDeckManager.PutCardFromHandToCombatZone(playerCombatCard);
                    combatContext.playerOnCombatCard = cardInHand.gameObject;

                    cardInHand.gameObject.transform.SetParent(
                        combatContext.playerOnCombatCardFinalPosition.transform.parent,
                        worldPositionStays: false
                    );
                    cardInHand.transform.position = combatContext.playerOnCombatCardFinalPosition.transform.position;

                    PostProcess(combatContext);
                });
                interactiveCombatCardComponent.EnableInteractiveComponent();
            }
        }
    }
}
