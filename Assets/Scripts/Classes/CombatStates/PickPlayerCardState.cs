using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPlayerCardState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        List<CombatCard> cardsInHand = playerDeckManager.GetCardsInHand();
        
        foreach (CombatCard cardInHand in cardsInHand)
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
        List<CombatCard> cardsInHand = playerDeckManager.GetCardsInHand();
        
        foreach (CombatCard cardInHand in cardsInHand)
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            if (interactiveCombatCardComponent != null)
            {
                interactiveCombatCardComponent.SetOnClickAction(() => {
                    cardInHand.gameObject.transform.SetParent(
                        combatContext.playerOnCombatCardFinalPosition.transform.parent,
                        worldPositionStays: false
                    );
                    cardInHand.transform.position = combatContext.playerOnCombatCardFinalPosition.transform.position;

                    combatContext.playerOnCombatCard = cardInHand.gameObject;

                    PostProcess(combatContext);
                });
                interactiveCombatCardComponent.EnableInteractiveComponent();
            }
        }
    }
}
