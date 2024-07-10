using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static CombatManager;

public class PickPlayerCardTutorialState : PickPlayerCardState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
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
            EnableCards(combatContext);
        });
    }

    private void BlockCards(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ForEachCardInCardsContainer(combatContext.GetPlayerCardInHandContainers(), (cardInHand) =>
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            CombatCard playerCombatCard = cardInHand.GetComponent<CombatCard>();

            if (interactiveCombatCardComponent != null && playerCombatCard != null)
            {
                interactiveCombatCardComponent.DisableInteractiveComponent();
            }
        });
    }

    private void EnableCards(CombatManager.CombatContext combatContext)
    {
        int i = 0;
        CombatUtils.ForEachCardInCardsContainer(combatContext.GetPlayerCardInHandContainers(), (cardInHand) =>
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardInHand.GetComponent<InteractiveCombatCardComponent>();
            CombatCard playerCombatCard = cardInHand.GetComponent<CombatCard>();

            if (interactiveCombatCardComponent != null && playerCombatCard != null)
            {
                if(TutorialManager.SceneTutorial.shouldCardBeActive(i))
                {
                    interactiveCombatCardComponent.SetOnClickAction(() =>
                    {
                        BlockCards(combatContext);
                        TutorialManager.SceneTutorial.trippingCount = 0;
                        TutorialManager.SceneTutorial.UnBlockScreen(async () =>
                        {
                            await PickAPlayerCard(combatContext, playerCombatCard);
                        });
                    });
                }
                else
                {
                    interactiveCombatCardComponent.SetOnClickAction(() =>
                    {
                        BlockCards(combatContext);
                        TutorialManager.SceneTutorial.PlayerTripping(() =>
                        {
                            EnableCards(combatContext);
                        });
                    });
                }
                interactiveCombatCardComponent.EnableInteractiveComponent();
            }
            i++;
        });
    }

}
