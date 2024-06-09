using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPlayerCardState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        DeckManager deckManager = GameManager.Instance.ProvideDeckManager();
        List<CombatCard> cardsInHand = deckManager.GetCardsInHand();
        
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
        combatContext.playerOnCombatCard = null;

        DeckManager deckManager = GameManager.Instance.ProvideDeckManager();
        List<CombatCard> cardsInHand = deckManager.GetCardsInHand();
        
        foreach (CombatCard cardInHand in cardsInHand)
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            if (interactiveCombatCardComponent != null)
            {
                interactiveCombatCardComponent.SetOnClickAction(() => {
                    RectTransform rectTransformComponent = 
                        cardInHand.GetComponent<RectTransform>();
                    if (rectTransformComponent != null)
                    {
                        rectTransformComponent.gameObject.transform.SetParent(
                            combatContext.playerOnCombatCardFinalPosition.transform.parent,
                            worldPositionStays: false
                        );
                        rectTransformComponent.position = combatContext.playerOnCombatCardFinalPosition.transform.position;
                    }

                    combatContext.playerOnCombatCard = cardInHand.gameObject;

                    PostProcess(combatContext);
                });
                interactiveCombatCardComponent.EnableInteractiveComponent();
            }
        }
    }
}
