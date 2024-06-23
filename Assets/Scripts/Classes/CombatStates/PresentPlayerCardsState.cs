using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PresentPlayerCardsState : CombatState
{
    private bool hasFinishedPresentigCards = false;

    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        if (hasFinishedPresentigCards)
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickEnemyCardState());
        }
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
        hasFinishedPresentigCards = false;
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        Preprocess(combatContext);

        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        // Calculate how much cards needs to draw to fill the hand
        int availableCardsToDraw = Mathf.Min(
            playerDeckManager.GetNumberOfCardsInDeck(),
            playerDeckManager.GetMaxAllowedCardsInHand()
        );
        int cardsLeftToFillHand = playerDeckManager.GetMaxAllowedCardsInHand() - playerDeckManager.GetNumberOfCardsInHand();
        int cardsToDraw = Mathf.Min(availableCardsToDraw, cardsLeftToFillHand);

        if (cardsToDraw > 0)
        {
            for (int i = 0; i < cardsToDraw; i++)
            {
                CombatCard cardToSpawnOnHand = playerDeckManager.DrawCardFromDeckToHand();
                if (cardToSpawnOnHand != null)
                {
                    int cardIndex = i;
                    GameUtils.CreateTemporizer(() => 
                    {
                        cardToSpawnOnHand.gameObject.SetActive(true);
                        cardToSpawnOnHand.gameObject.transform.SetParent(
                            combatContext.playerHandContainer.transform,
                            worldPositionStays: false
                        );
                        
                        hasFinishedPresentigCards = cardIndex == cardsToDraw - 1;
                        PostProcess(combatContext);
                    }, i * .5f, GameManager.Instance);
                }
            }   
        }
        else if (playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            hasFinishedPresentigCards = true;
            PostProcess(combatContext);
        }
    }
}
