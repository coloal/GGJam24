using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PresentEnemyCardsState : CombatState
{

    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PresentPlayerCardsState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        SetEnemyCardsCombatTypeHints(combatContext);
        ShowEnemyCardsCombatTypeHints(combatContext);
    }

    void SetEnemyCardsCombatTypeHints(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        List<CombatTypes> enemyCardsCombatTypes = 
            enemyDeckManager.GetAllEnemyCards().Select((combatCardTemplate) => combatCardTemplate.CombatType).ToList();

        foreach (CombatTypes combatType in enemyCardsCombatTypes)
        {
            GameObject combatHint = 
                CombatSceneManager.Instance.ProvideCombatV2Manager().InstantiateCombatTypeHintGameObject();
            CombatTypeHintComponent combatTypeHintComponent = combatHint.GetComponent<CombatTypeHintComponent>();
            if (combatTypeHintComponent != null)
            {
                combatTypeHintComponent.SetCombatTypeHint(combatType);
            }

            combatHint.transform.SetParent(
                combatContext.enemyCardsCombatTypeHintsContainer.transform,
                worldPositionStays: false
            );
        }

    }

    void ShowEnemyCardsCombatTypeHints(CombatV2Manager.CombatContext combatContext)
    {
        MoveUIElementComponent animationComponent = 
            combatContext.enemyCardsCombatTypeHintsContainer.GetComponent<MoveUIElementComponent>();
        if (animationComponent)
        {
            Transform enemyCardsContainerOriginalPos = Transform.Instantiate(combatContext.enemyCardsCombatTypeHintsContainer.transform);

            animationComponent.StartMovingTowards(
                objectToMove: combatContext.enemyCardsCombatTypeHintsContainer,
                finalPosition: Vector2.zero,
                () => {
                    GameUtils.CreateTemporizer(() => {
                        animationComponent.StartMovingTowards(
                            objectToMove: combatContext.enemyCardsCombatTypeHintsContainer,
                            finalPosition: enemyCardsContainerOriginalPos.position,
                            () => {
                                PostProcess(combatContext);
                            }
                        );
                    }, 1, CombatSceneManager.Instance);
                }
            );
        }
    }
}
