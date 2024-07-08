using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PickPlayerCardTutorialState : PickPlayerCardState
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
            nextState: new ShowCardsTutorialState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForShowCards
        );
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartPlayerPickConversation(() =>
        {
            int i = 0;
            CombatUtils.ForEachCardInCardsContainer(combatContext.GetPlayerCardInHandContainers(), (cardInHand) =>
            {
                InteractiveCombatCardComponent interactiveCombatCardComponent =
                    cardInHand.GetComponent<InteractiveCombatCardComponent>();
                CombatCard playerCombatCard = cardInHand.GetComponent<CombatCard>();
                
                if (interactiveCombatCardComponent != null && playerCombatCard != null && TutorialManager.SceneTutorial.shouldCardBeActive(i))
                {
                    interactiveCombatCardComponent.SetOnClickAction(() =>
                    {
                        TutorialManager.SceneTutorial.UnBlockScreen(async () =>
                        {
                            await PickAPlayerCard(combatContext, playerCombatCard);
                        });
                    });
                    interactiveCombatCardComponent.EnableInteractiveComponent();
                }
                i++;
            });
        });
        
    }

}
