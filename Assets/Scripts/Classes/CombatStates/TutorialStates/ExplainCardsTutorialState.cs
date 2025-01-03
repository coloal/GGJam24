using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorial : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PresentEnemyCardsState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPresentEnemyCards
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        SetUpPlayerDeck(combatContext);
        SetUpEnemyDeck(combatContext);
        PostProcess(combatContext);
    }

    void SetUpPlayerDeck(CombatManager.CombatContext combatContext)
    {
        InventoryManager inventoryManager = GameManager.Instance.ProvideInventoryManager();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        List<CombatCardTemplate> playerSavedDeck = inventoryManager.GetDeckCopy();
        playerSavedDeck.ForEach((combatCardData) =>
        {
            CombatCard combatCard = CombatSceneManager.Instance
                .ProvideCombatManager()
                .InstantiateCombatCardGameObject()
                .GetComponent<CombatCard>();
            if (combatCard != null)
            {
                combatCard.gameObject.SetActive(false);
                combatCard.transform.SetParent(
                    combatContext.combatContainer.transform,
                    worldPositionStays: false
                );
                combatCard.SetDataCard(combatCardData);
                playerDeckManager.AddCardToDeck(combatCard);
            }
        });
    }

    void SetUpEnemyDeck(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();
        
        List<CombatCardTemplate> enemyDeck = CombatSceneManager.Instance.ProvideEnemyData().CombatCards;
        enemyDeck.ForEach((combatCardData) =>
        {
            CombatCard combatCard = CombatSceneManager.Instance
                .ProvideCombatManager()
                .InstantiateCombatCardGameObject()
                .GetComponent<CombatCard>();
            if (combatCard != null)
            {
                combatCard.gameObject.SetActive(false);
                combatCard.transform.SetParent(
                    combatContext.combatContainer.transform,
                    worldPositionStays: false
                );
                combatCard.SetDataCard(combatCardData);
                enemyDeckManager.AddCardToDeck(combatCard);
            }
        });
    }
}
