using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PresentEnemyCardsState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        combatContext.CleanEnemyCardsContainer();
        combatContext.DeactivateEnemyCardsContainer();

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PresentPlayerCardsState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPresentPlayerCards
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
        combatContext.ActivateEnemyCardsContainer();
    }

    public async override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        SetEnemyCardsCombatTypeHints(combatContext);
        await ShowEnemyCardsCombatTypeHints(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        PostProcess(combatContext);
    }

    protected void SetEnemyCardsCombatTypeHints(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();

        List<GameObject> enemyCardTypesHints =
            enemyDeckManager
                .GetAllDeckCardsData()
                .Select((combatCardTemplate) => 
                {
                    GameObject combatTypeHint =
                        CombatSceneManager.Instance.ProvideCombatManager().InstantiateCombatTypeHintGameObject();
                    CombatTypeHintComponent combatTypeHintComponent = combatTypeHint.GetComponent<CombatTypeHintComponent>();
                    if (combatTypeHintComponent != null)
                    {
                        combatTypeHintComponent.SetCombatTypeHint(combatCardTemplate.CombatType);
                    }

                    return combatTypeHint;
                })
                .ToList();

        int maxAllowedEnemyCards = CombatSceneManager.Instance.ProvideCombatManager().GetMaxAllowedEnemyCards();
        int maxCardsPerRow = maxAllowedEnemyCards / CombatUtils.NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS;

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
            GameObject emptyCardDummy = CombatSceneManager.Instance.ProvideCombatManager().InstantiateEmptyCardDummyGameObject();
            enemyCardTypesHints.Add(emptyCardDummy);
        }

        // Case when there's only 1 row of card type hints to fill
        if (enemyCardTypesHints.Count <= maxCardsPerRow)
        {
            combatContext.enemyCardsRow0.SetActive(true);
            combatContext.enemyCardsRow1.SetActive(false);
            foreach (GameObject enemyCardTypesHint in enemyCardTypesHints)
            {
                enemyCardTypesHint.transform.SetParent(
                    combatContext.enemyCardsRow0.transform,
                    worldPositionStays: false
                );
            }
        }
        // Case when there's more than 1 row of card type hints to fill
        else if (enemyCardTypesHints.Count > maxCardsPerRow && enemyCardTypesHints.Count <= maxAllowedEnemyCards)
        {
            combatContext.enemyCardsRow0.SetActive(true);
            combatContext.enemyCardsRow1.SetActive(true);
            int cardIndex;
            // Fills the row 0 for the enemy card type hints
            for (cardIndex = 0; cardIndex < maxCardsPerRow; cardIndex++)
            {
                enemyCardTypesHints[cardIndex].transform.SetParent(
                    combatContext.enemyCardsRow0.transform,
                    worldPositionStays: false
                );
            }
            // Fills the row 1 for the enemy card type hints
            for (int i = cardIndex; i < maxAllowedEnemyCards; i++)
            {
                enemyCardTypesHints[i].transform.SetParent(
                    combatContext.enemyCardsRow1.transform,
                    worldPositionStays: false
                );
            }
        }
    }

    async Task ShowEnemyCardsCombatTypeHints(CombatManager.CombatContext combatContext)
    {
        float showHintsTime = CombatSceneManager.Instance.ProvideEnemyData().ShowHintsTimeInSeconds;

        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayShowEnemyCardsTypesHints(
                pauseTime: showHintsTime
            );
    }
}
