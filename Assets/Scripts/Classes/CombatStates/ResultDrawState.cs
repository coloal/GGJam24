using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDrawState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().OverwriteCombatContext(combatContext);

        //Al enemigo o al Player le quedan cartas
        if (CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck() > 0
            && GameManager.Instance.ProvideDeckManager().GetNumberOfCardsInHand() > 0 )
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickEnemyCardState());
        }
        else
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new TossCoindState());
        }
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        combatContext.enemiesCardsOnDraw.Add(combatContext.enemyOnCombatCard);
        combatContext.playerCardsOnDraw.Add(combatContext.playerOnCombatCard);

        MoveUIElementComponent EnemyAnimationComponent =
            combatContext.enemyOnCombatCard.GetComponent<MoveUIElementComponent>();

        MoveUIElementComponent PlayerAnimationComponent =
            combatContext.playerOnCombatCard.GetComponent<MoveUIElementComponent>();


        if (combatContext.enemiesCardsOnDraw.Count == 1 || combatContext.enemiesCardsOnDraw.Count == 2)
        {
            if (EnemyAnimationComponent && PlayerAnimationComponent)
            {
                Vector2 finalEnemyPosition;
                Vector2 finalPlayerPosition;

                Vector2 enemyCardOriginalPos = new Vector2(combatContext.enemyOnCombatCard.transform.position.x, combatContext.enemyOnCombatCard.transform.position.y);
                Vector2 playerCardOriginalPos = new Vector2(combatContext.playerOnCombatCard.transform.position.x, combatContext.playerOnCombatCard.transform.position.y);

                if (combatContext.enemiesCardsOnDraw.Count == 1) 
                {
                    finalEnemyPosition = new Vector2(combatContext.firtsEnemyPositionOnDraw.position.x, combatContext.firtsEnemyPositionOnDraw.position.y);
                    finalPlayerPosition = new Vector2(combatContext.firtsPlayerPositionOnDraw.position.x, combatContext.firtsPlayerPositionOnDraw.position.y);
                }
                else
                {
                    finalEnemyPosition = new Vector2(combatContext.secondEnemyPositionOnDraw.position.x, combatContext.secondEnemyPositionOnDraw.position.y);
                    finalPlayerPosition = new Vector2(combatContext.secondPlayerPositionOnDraw.position.x, combatContext.secondPlayerPositionOnDraw.position.y);
                }
                
                EnemyAnimationComponent.StartMovingTowards(
                    objectToMove: combatContext.enemyOnCombatCard,
                    finalPosition: enemyCardOriginalPos,
                    () =>
                    {
                        GameUtils.CreateTemporizer(() =>
                        {
                            EnemyAnimationComponent.StartMovingTowards(
                                objectToMove: combatContext.enemyOnCombatCard,
                                finalPosition: finalEnemyPosition,
                                () =>
                                {
                                    //PostProcess(combatContext);
                                }
                            );
                        }, 1, CombatSceneManager.Instance);
                    }
                );

                PlayerAnimationComponent.StartMovingTowards(
                objectToMove: combatContext.playerOnCombatCard,
                finalPosition: playerCardOriginalPos,
                () =>
                {
                    GameUtils.CreateTemporizer(() =>
                    {
                        PlayerAnimationComponent.StartMovingTowards(
                            objectToMove: combatContext.playerOnCombatCard,
                            finalPosition: finalPlayerPosition,
                            () =>
                            {
                                PostProcess(combatContext);
                            }
                        );
                    }, 1, CombatSceneManager.Instance);
                }
                );
            }
        }
        else
        {
            PostProcess(combatContext);
        }
    }
}
