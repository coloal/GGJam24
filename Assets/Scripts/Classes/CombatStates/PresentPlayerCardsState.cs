using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
            await MakeSpaceInHandForNewCards(combatContext);
            await DrawCardFromDeckToHand(combatContext, cardsToDraw);
            PostProcess(combatContext);
        }
        else if (playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            PostProcess(combatContext);
        }
    }

    async Task MakeSpaceInHandForNewCards(CombatV2Manager.CombatContext combatContext)
    {
        bool HasACard(Transform cardInHandContainer)
        {
            return cardInHandContainer.childCount > 0;    
        }

        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        
        List<Transform> playerCardInHandContainers = combatContext.GetPlayerCardInHandContainers();

        for (int i = playerDeckManager.GetMaxAllowedCardsInHand() - 1; i > 0; i--)
        {
            // Is there a card in the current hand position?
            if (HasACard(playerCardInHandContainers[i]) 
                && !HasACard(playerCardInHandContainers[i - 1]))
            {
                GameObject cardToMove = playerCardInHandContainers[i].GetChild(0).gameObject;

                cardToMove.transform.SetParent(playerCardInHandContainers[i - 1]);

                CombatCard combatCardToMove = cardToMove.GetComponent<CombatCard>();
                if (combatCardToMove != null)
                {
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayMoveCardPositionInHand(
                            cardToMove: combatCardToMove,
                            playerCardInHandContainers[i - 1]
                        );
                }
            }
        }
    }

    async Task DrawCardFromDeckToHand(CombatV2Manager.CombatContext combatContext, int cardsToDraw)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        CombatFeedbacksManager combatFeedbacksManager = CombatSceneManager.Instance.ProvideCombatFeedbacksManager();
        
        for (int i = 0; i < cardsToDraw; ++i)
        {
            Transform firstAvailbalePositionInHand = combatContext
                .GetPlayerCardInHandContainers()
                .Find((cardInHandContainer) => cardInHandContainer.childCount == 0);

            CombatCard cardToDraw = playerDeckManager.DrawCardFromDeckToHand();
            cardToDraw.gameObject.SetActive(true);
            cardToDraw.transform.SetParent(
                firstAvailbalePositionInHand,
                worldPositionStays: false
            );

            await combatFeedbacksManager.PlayPlayerDrawCardFromDeck(
                playerCard: cardToDraw,
                playerDeck: combatContext.playerDeck,
                cardInHandPosition: firstAvailbalePositionInHand
            );
        }
    }
}
