using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PresentEnemyCardsTutorialState : PresentEnemyCardsState
{
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
            await ShowEnemyCardsCombatTypeHints(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            TutorialManager.SceneTutorial.StartEnemyCardExplanationWhileShow(() =>
            {
                PostProcess(combatContext);
            });
        });   
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
