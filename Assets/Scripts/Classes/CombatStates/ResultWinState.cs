using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatManager;

public class ResultWinState : CombatState
{
    private const int NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS = 2;

    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        combatContext.enemyCardsPickUpRow0.SetActive(false);
        combatContext.enemyCardsPickUpRow1.SetActive(false);
        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

        //TODO: return to history
        GameManager.Instance.EndCombat(TurnResult.COMBAT_WON_CAPTURE);
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        List<CombatCardTemplate> enemyCombatCards = CombatSceneManager.Instance.ProvideEnemyData().CombatCards;//enemyDeckManager.GetAllDeckCardsData();
        List<GameObject> CombatCards = new List<GameObject>();

        //Instantiate all cards of the enemyDeck
        foreach (CombatCardTemplate card in enemyCombatCards)
        {
            GameObject combatCard =
                CombatSceneManager.Instance.ProvideCombatManager().InstantiateCombatCardGameObject();
            CombatCard combatCardComponent = combatCard.GetComponent<CombatCard>();
            if (combatCardComponent != null)
            {
                combatCardComponent.SetDataCard(card);
                CombatCards.Add(combatCard);
            }
        }

        int maxAllowedEnemyCards = CombatSceneManager.Instance.ProvideCombatManager().GetMaxAllowedEnemyCards();
        int maxCardsPerRow = maxAllowedEnemyCards / NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS;

        int cardsLeftToFill = 0;
        if (CombatCards.Count <= maxCardsPerRow)
        {
            cardsLeftToFill = maxCardsPerRow - CombatCards.Count;
        }
        else if (CombatCards.Count > maxCardsPerRow && CombatCards.Count <= maxAllowedEnemyCards)
        {
            cardsLeftToFill = maxAllowedEnemyCards - CombatCards.Count;
        }

        //ASIGNAR Click Action

        foreach (GameObject EnemyCard in CombatCards)
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                EnemyCard.GetComponent<InteractiveCombatCardComponent>();
            CombatCard playerCombatCard = EnemyCard.GetComponent<CombatCard>();

            if (interactiveCombatCardComponent != null && playerCombatCard != null)
            {
                interactiveCombatCardComponent.SetOnClickAction(() => {
                    
                    PickAEnemyCard(combatContext, playerCombatCard.GetCardData());
                });
                interactiveCombatCardComponent.EnableInteractiveComponent();

                playerCombatCard.ActivateCardFront(true);
                playerCombatCard.ActivateCardBack(false);
            }
        }



        // Fills the list of card type hints with dummies to fill the empty gaps in the row
        // (a workaround for the default HorizontalLayout component behaviour)
        for (int i = 0; i < cardsLeftToFill; i++)
        {
            GameObject emptyCardDummy = CombatSceneManager.Instance.ProvideCombatManager().InstantiateEmptyCardDummyGameObject();
            CombatCards.Add(emptyCardDummy);
        }

        // Case when there's only 1 row of card type hints to fill
        if (CombatCards.Count <= maxCardsPerRow)
        {
            combatContext.enemyCardsPickUpRow0.SetActive(true);
            combatContext.enemyCardsPickUpRow1.SetActive(false);
            foreach (GameObject enemyCard in CombatCards)
            {
                enemyCard.transform.SetParent(
                    combatContext.enemyCardsPickUpRow0.transform,
                    worldPositionStays: false
                );
            }
        }
        // Case when there's more than 1 row of card type hints to fill
        else if (CombatCards.Count > maxCardsPerRow && CombatCards.Count <= maxAllowedEnemyCards)
        {
            combatContext.enemyCardsPickUpRow0.SetActive(true);
            combatContext.enemyCardsPickUpRow1.SetActive(true);
            int cardIndex;
            // Fills the row 0 for the enemy card type hints
            for (cardIndex = 0; cardIndex < maxCardsPerRow; cardIndex++)
            {
                CombatCards[cardIndex].transform.SetParent(
                    combatContext.enemyCardsPickUpRow0.transform,
                    worldPositionStays: false
                );
            }
            // Fills the row 1 for the enemy card type hints
            for (int i = cardIndex; i < maxAllowedEnemyCards; i++)
            {
                CombatCards[i].transform.SetParent(
                    combatContext.enemyCardsPickUpRow1.transform,
                    worldPositionStays: false
                );
            }
        }

        
    }


    private void PickAEnemyCard(CombatManager.CombatContext combatContext, CombatCardTemplate CombatCardData) 
    {
        //Add card al deck
        GameManager.Instance.ProvideInventoryManager().AddCombatCardToVault(CombatCardData);

        PostProcess(combatContext);
    }
}
