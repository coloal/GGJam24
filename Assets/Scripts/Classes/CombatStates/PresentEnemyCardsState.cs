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
        MoveCardAnimationComponent animationComponent = 
            combatContext.enemyCardsContainer.GetComponent<MoveCardAnimationComponent>();
        if (animationComponent)
        {
            animationComponent.StartMovingCardTowards(
                cardToMove: combatContext.enemyCardsContainer,
                cardFinalPosition: combatContext.enemyCardsContainerFinalPosition,
                () => {
                    PostProcess(combatContext);
                }
            );
        }   
    }
}
