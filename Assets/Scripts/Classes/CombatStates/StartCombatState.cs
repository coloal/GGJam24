using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombatState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PresentEnemyCardsState(),
            seconds: CombatSceneManager.Instance.ProvideCombatV2Manager().timeForPresentEnemyCards
        );
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        SetUpPlayerDeck(combatContext);
        SetUpEnemyDeck(combatContext);
        PostProcess(combatContext);
    }

    void SetUpPlayerDeck(CombatV2Manager.CombatContext combatContext)
    {
        InventoryManager inventoryManager = GameManager.Instance.ProvideInventoryManager();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        List<CombatCardTemplate> playerSavedDeck = inventoryManager.GetDeckCopy();
        playerSavedDeck.ForEach((combatCardData) =>
        {
            CombatCard combatCard = CombatSceneManager.Instance
                .ProvideCombatV2Manager()
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

    void SetUpEnemyDeck(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        
        List<CombatCardTemplate> enemyDeck = CombatSceneManager.Instance.ProvideEnemyData().CombatCards;
        enemyDeck.ForEach((combatCardData) =>
        {
            CombatCard combatCard = CombatSceneManager.Instance
                .ProvideCombatV2Manager()
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
