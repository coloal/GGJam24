using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PresentEnemyCardsState : CombatState
{
    private const int NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS = 2;

    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        combatContext.enemyCardsHintRow0.SetActive(false);
        combatContext.enemyCardsHintRow1.SetActive(false);

        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PresentPlayerCardsState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public async override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        SetEnemyCardsCombatTypeHints(combatContext);
        await ShowEnemyCardsCombatTypeHints(combatContext);
        PostProcess(combatContext);
    }

    void SetEnemyCardsCombatTypeHints(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        List<GameObject> enemyCardTypesHints =
            enemyDeckManager
                .GetAllDeckCardsData()
                .Select((combatCardTemplate) => 
                {
                    GameObject combatTypeHint =
                        CombatSceneManager.Instance.ProvideCombatV2Manager().InstantiateCombatTypeHintGameObject();
                    CombatTypeHintComponent combatTypeHintComponent = combatTypeHint.GetComponent<CombatTypeHintComponent>();
                    if (combatTypeHintComponent != null)
                    {
                        combatTypeHintComponent.SetCombatTypeHint(combatCardTemplate.CombatType);
                    }

                    return combatTypeHint;
                })
                .ToList();

        int maxAllowedEnemyCards = CombatSceneManager.Instance.ProvideCombatV2Manager().GetMaxAllowedEnemyCards();
        int maxCardsPerRow = maxAllowedEnemyCards / NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS;

        int cardsLeftToFill = 0;
        if (enemyCardTypesHints.Count <= maxCardsPerRow)
        {
            cardsLeftToFill = maxCardsPerRow - enemyCardTypesHints.Count;
        }
        else if (enemyCardTypesHints.Count > maxCardsPerRow && enemyCardTypesHints.Count <= maxAllowedEnemyCards)
        {
            cardsLeftToFill = maxAllowedEnemyCards - enemyCardTypesHints.Count;
        }

        // Fills the list of card type hints with dummies to fill the empty gaps in the row
        // (a workaround for the default HorizontalLayout component behaviour)
        for (int i = 0; i < cardsLeftToFill; i++)
        {
            GameObject emptyCardDummy = CombatSceneManager.Instance.ProvideCombatV2Manager().InstantiateEmptyCardDummyGameObject();
            enemyCardTypesHints.Add(emptyCardDummy);
        }

        // Case when there's only 1 row of card type hints to fill
        if (enemyCardTypesHints.Count <= maxCardsPerRow)
        {
            combatContext.enemyCardsHintRow0.SetActive(true);
            combatContext.enemyCardsHintRow1.SetActive(false);
            foreach (GameObject enemyCardTypesHint in enemyCardTypesHints)
            {
                enemyCardTypesHint.transform.SetParent(
                    combatContext.enemyCardsHintRow0.transform,
                    worldPositionStays: false
                );
            }
        }
        // Case when there's more than 1 row of card type hints to fill
        else if (enemyCardTypesHints.Count > maxCardsPerRow && enemyCardTypesHints.Count <= maxAllowedEnemyCards)
        {
            combatContext.enemyCardsHintRow0.SetActive(true);
            combatContext.enemyCardsHintRow1.SetActive(true);
            int cardIndex;
            // Fills the row 0 for the enemy card type hints
            for (cardIndex = 0; cardIndex < maxCardsPerRow; cardIndex++)
            {
                enemyCardTypesHints[cardIndex].transform.SetParent(
                    combatContext.enemyCardsHintRow0.transform,
                    worldPositionStays: false
                );
            }
            // Fills the row 1 for the enemy card type hints
            for (int i = cardIndex; i < maxAllowedEnemyCards; i++)
            {
                enemyCardTypesHints[i].transform.SetParent(
                    combatContext.enemyCardsHintRow1.transform,
                    worldPositionStays: false
                );
            }
        }
    }

    async Task ShowEnemyCardsCombatTypeHints(CombatV2Manager.CombatContext combatContext)
    {
        float showHintsTime = CombatSceneManager.Instance.ProvideEnemyData().ShowHintsTimeInSeconds;

        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayShowEnemyCardsTypesHints(
                pauseTime: showHintsTime
            );
    }
}
