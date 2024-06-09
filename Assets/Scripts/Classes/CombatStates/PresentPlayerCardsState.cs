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

        DeckManager deckManager = GameManager.Instance.ProvideDeckManager();
        int cardsLeftToFillHand = deckManager.GetMaxAllowedCardsInHand() - deckManager.GetNumberOfCardsInHand();

        if (deckManager.GetNumberOfCardsInDeck() > 0)
        {
            for (int i = 0; i < cardsLeftToFillHand; i++)
            {
                CombatCard cardToSpawnOnHand = deckManager.GiveTopCardToHand();
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
                        
                        hasFinishedPresentigCards = cardIndex == cardsLeftToFillHand - 1;
                        PostProcess(combatContext);
                    }, i * .5f, GameManager.Instance);
                }
            }   
        }
        else if (deckManager.GetNumberOfCardsInHand() > 0)
        {
            hasFinishedPresentigCards = true;
            PostProcess(combatContext);
        }
    }
}
