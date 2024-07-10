using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PresentEnemyCardsTutorialState : PresentEnemyCardsState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        combatContext.CleanEnemyCardsContainer();
        combatContext.DeactivateEnemyCardsContainer();

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PickEnemyCardTutorialState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPresentPlayerCards
        );
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartEnemyCardExplanationPreShow(async () =>
        {
            SetEnemyCardsCombatTypeHints(combatContext);

            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayMoveEnemyCardsTypesHints(
                    origin: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintOriginPosition(),
                    destination: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintDestinationPosition()
                );
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            TutorialManager.SceneTutorial.StartEnemyCardExplanationWhileShow(async () =>
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayMoveEnemyCardsTypesHints(
                        origin: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintDestinationPosition(),
                        destination: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintOriginPosition()
                    );
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayPlaceEnemyDeckOnBoard();
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }

                PostProcess(combatContext);
            });
        });   
    }
}
