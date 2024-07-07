using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class PresentPlayerCardsState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PickEnemyCardState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickEnemyCard
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        // Calculate how much cards needs to draw to fill the hand
        int availableCardsToDraw = Mathf.Min(
            playerDeckManager.GetNumberOfCardsInDeck(),
            playerDeckManager.GetMaxAllowedCardsInHand()
        );
        int cardsLeftToFillHand = playerDeckManager.GetMaxAllowedCardsInHand() - playerDeckManager.GetNumberOfCardsInHand();
        int cardsToDraw = Mathf.Min(availableCardsToDraw, cardsLeftToFillHand);
    
        await MakeSpaceInHandForNewCards(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        
        if (cardsToDraw > 0)
        {
            await DrawCardFromDeckToHand(combatContext, cardsToDraw);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            PostProcess(combatContext);
        }
        else if (playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            PostProcess(combatContext);
        }
    }

    async Task MakeSpaceInHandForNewCards(CombatManager.CombatContext combatContext)
    {
        bool HasACard(Transform cardInHandContainer)
        {
            return cardInHandContainer.childCount > 0;    
        }

        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        
        List<Transform> playerCardInHandContainers = combatContext.GetPlayerCardInHandContainers();

        for (int i = 0; i < playerDeckManager.GetMaxAllowedCardsInHand() - 1; i++)
        {
            // Is there a card in the current hand position?
            if (!HasACard(playerCardInHandContainers[i]) 
                && HasACard(playerCardInHandContainers[i + 1]))
            {
                GameObject cardToMove = playerCardInHandContainers[i + 1].GetChild(0).gameObject;

                cardToMove.transform.SetParent(playerCardInHandContainers[i]);

                CombatCard combatCardToMove = cardToMove.GetComponent<CombatCard>();
                if (combatCardToMove != null)
                {
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayMoveCardPositionInHand(
                            cardToMove: combatCardToMove,
                            playerCardInHandContainers[i]
                        );
                    if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }
    }

    protected async Task DrawCardFromDeckToHand(CombatManager.CombatContext combatContext, int cardsToDraw)
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
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
        }
    }
}
