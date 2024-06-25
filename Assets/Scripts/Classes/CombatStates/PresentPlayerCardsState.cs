using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class PresentPlayerCardsState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickEnemyCardState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override async void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
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
            await DrawCardFromDeckToHand(combatContext, cardsToDraw);
            PostProcess(combatContext);
        }
        else if (playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            PostProcess(combatContext);
        }
    }

    async Task DrawCardFromDeckToHand(CombatV2Manager.CombatContext combatContext, int cardsToDraw)
    {
        Transform GetCardInHandTransform(int cardIndex)
        {
            Transform cardInHandTransform = combatContext.playerCardInHandPosition0;
            switch (cardIndex)
            {
                case 0:
                    cardInHandTransform = combatContext.playerCardInHandPosition0;
                    break;
                case 1:
                    cardInHandTransform = combatContext.playerCardInHandPosition1;
                    break;
                case 2:
                    cardInHandTransform = combatContext.playerCardInHandPosition2;
                    break;
                default:
                    break;
            }

            return cardInHandTransform;
        }

        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        CombatFeedbacksManager combatFeedbacksManager = CombatSceneManager.Instance.ProvideCombatFeedbacksManager();
        
        for (int i = 0; i < cardsToDraw; ++i)
        {
            int cardIndex = playerDeckManager.GetNumberOfCardsInHand();

            CombatCard cardToDraw = playerDeckManager.DrawCardFromDeckToHand();
            cardToDraw.gameObject.SetActive(true);
            cardToDraw.transform.SetParent(
                combatContext.playerHandContainer.transform,
                worldPositionStays: false
            );

            await combatFeedbacksManager.PlayPlayerDrawCardFromDeck(
                playerCard: cardToDraw,
                playerDeck: combatContext.playerDeck,
                cardInHandPosition: GetCardInHandTransform(cardIndex)
            );
        }
    }
}
