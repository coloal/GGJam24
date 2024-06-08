using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEnemyCardsState : CombatState
{
    Transform enemyCardsContainerOriginalPos;

    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PresentPlayerCardsState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
        enemyCardsContainerOriginalPos = combatContext.enemyCardsContainer.transform;
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        MoveUIElementComponent animationComponent = 
            combatContext.enemyCardsContainer.GetComponent<MoveUIElementComponent>();
        if (animationComponent)
        {
            Transform enemyCardsContainerOriginalPos = Transform.Instantiate(combatContext.enemyCardsContainer.transform);

            animationComponent.StartMovingTowards(
                objectToMove: combatContext.enemyCardsContainer,
                finalPosition: Vector2.zero,
                () => {
                    GameUtils.CreateTemporizer(() => {
                        animationComponent.StartMovingTowards(
                            objectToMove: combatContext.enemyCardsContainer,
                            finalPosition: new Vector2(0f, 2000f),
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
